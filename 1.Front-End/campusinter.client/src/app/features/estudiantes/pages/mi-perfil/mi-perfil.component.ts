import { CommonModule } from '@angular/common';
import { Component, OnInit, inject } from '@angular/core';
import { FormBuilder, ReactiveFormsModule, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { FontAwesomeModule } from '@fortawesome/angular-fontawesome';
import { faCircleCheck, faEnvelope, faSave, faTrash } from '@fortawesome/free-solid-svg-icons';

import { AuthService } from '../../../../core/auth/services/auth.service';
import { CurrentUserStore } from '../../../../core/auth/services/current-user.store';
import { ErrorService } from '../../../../core/errors/error.service';
import { ActualizarMiPerfilRequest, MiPerfilResponse } from '../../models/estudiante.models';
import { EstudianteService } from '../../services/estudiante.service';

type PerfilControlName =
  | 'primerNombre'
  | 'segundoNombre'
  | 'primerApellido'
  | 'segundoApellido'
  | 'correo'
  | 'documento';

@Component({
  selector: 'app-mi-perfil',
  standalone: true,
  imports: [
    CommonModule,
    ReactiveFormsModule,
    FontAwesomeModule
  ],
  templateUrl: './mi-perfil.component.html',
  styleUrl: './mi-perfil.component.css'
})
export class MiPerfilComponent implements OnInit {
  //--Inyecciones
  private readonly estudianteService = inject(EstudianteService);
  private readonly currentUserStore = inject(CurrentUserStore);
  private readonly authService = inject(AuthService);
  private readonly errorService = inject(ErrorService);
  private readonly router = inject(Router);
  private readonly fb = inject(FormBuilder);

  //--Variables
  perfil: MiPerfilResponse | null = null;
  isLoading = false;

  readonly faSave = faSave;
  readonly faTrash = faTrash;
  readonly faCircleCheck = faCircleCheck;
  readonly faEnvelope = faEnvelope;

  //--Formularios
  readonly form = this.fb.nonNullable.group({
    primerNombre: ['', [Validators.required]],
    segundoNombre: [''],
    primerApellido: ['', [Validators.required]],
    segundoApellido: [''],
    correo: ['', [Validators.required, Validators.email]],
    documento: ['', [Validators.required]]
  });

  //--Métodos
  ngOnInit(): void {
    this.cargarPerfil();
  }

  cargarPerfil(): void {
    this.isLoading = true;

    this.estudianteService.getMiPerfil().subscribe({
      next: perfil => {
        this.establecerPerfil(perfil);
        this.actualizarUsuarioActual(perfil);
        this.isLoading = false;
      },
      error: () => {
        this.isLoading = false;
      }
    });
  }

  obtenerNombreCompleto(): string {
    if (!this.perfil) {
      return 'Estudiante';
    }

    return this.armarNombreCompleto(this.perfil);
  }

  obtenerIniciales(): string {
    const palabras = this.obtenerNombreCompleto()
      .split(' ')
      .map(palabra => palabra.trim())
      .filter(palabra => palabra.length > 0);

    if (palabras.length === 0) {
      return 'ES';
    }

    return palabras
      .slice(0, 2)
      .map(palabra => palabra[0].toUpperCase())
      .join('');
  }

  estaActivo(): boolean {
    return this.perfil?.estado?.toLowerCase() === 'activo';
  }

  guardarCambios(): void {
    if (this.form.invalid) {
      this.form.markAllAsTouched();
      return;
    }

    const rawValue = this.form.getRawValue();

    const request: ActualizarMiPerfilRequest = {
      primerNombre: rawValue.primerNombre,
      segundoNombre: rawValue.segundoNombre || null,
      primerApellido: rawValue.primerApellido,
      segundoApellido: rawValue.segundoApellido || null,
      documento: rawValue.documento
    };

    this.isLoading = true;

    this.estudianteService.actualizarMiPerfil(request).subscribe({
      next: perfil => {
        this.establecerPerfil(perfil);
        this.actualizarUsuarioActual(perfil);
        this.form.markAsPristine();
        this.errorService.mostrarExito('Datos actualizados correctamente.');
        this.isLoading = false;
      },
      error: () => {
        this.isLoading = false;
      }
    });
  }

  inhabilitarCuenta(): void {
    const confirmed = confirm(
      '¿Seguro que quieres inhabilitar tu cuenta? No podrás usar el portal académico mientras esté inactiva.'
    );

    if (!confirmed) {
      return;
    }

    this.isLoading = true;

    this.estudianteService.inhabilitarMiPerfil().subscribe({
      next: () => {
        this.isLoading = false;
        this.errorService.mostrarError('Cuenta inhabilitada correctamente.');
        this.cargarPerfil();
      },
      error: () => {
        this.isLoading = false;
      }
    });
  }

  habilitarCuenta(): void {
    const confirmed = confirm('¿Seguro que quieres habilitar tu cuenta nuevamente?');

    if (!confirmed) {
      return;
    }

    this.isLoading = true;

    this.estudianteService.habilitarMiPerfil().subscribe({
      next: () => {
        this.errorService.mostrarExito('Cuenta habilitada correctamente.');
        this.cargarPerfil();
      },
      error: () => {
        this.isLoading = false;
      }
    });
  }

  mostrarError(controlName: PerfilControlName, errorName: string): boolean {
    const control = this.form.controls[controlName];

    return control.touched && control.hasError(errorName);
  }

  private establecerPerfil(perfil: MiPerfilResponse): void {
    this.perfil = perfil;

    this.form.patchValue({
      primerNombre: perfil.primerNombre,
      segundoNombre: perfil.segundoNombre ?? '',
      primerApellido: perfil.primerApellido,
      segundoApellido: perfil.segundoApellido ?? '',
      correo: perfil.correo,
      documento: perfil.documento
    });

    this.actualizarUsuarioActual(perfil);
  }

  private actualizarUsuarioActual(perfil: MiPerfilResponse): void {
    const currentUser = this.currentUserStore.currentUser();

    if (!currentUser) {
      return;
    }

    this.currentUserStore.establecerUsuario({
      ...currentUser,
      displayName: this.armarNombreCompleto(perfil),
      estado: perfil.estado
    });
  }

  private armarNombreCompleto(perfil: MiPerfilResponse): string {
    return [
      perfil.primerNombre,
      perfil.segundoNombre,
      perfil.primerApellido,
      perfil.segundoApellido
    ]
      .filter(value => !!value?.trim())
      .join(' ');
  }
}
