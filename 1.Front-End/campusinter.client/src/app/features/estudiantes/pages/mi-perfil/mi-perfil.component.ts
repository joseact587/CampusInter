import { CommonModule } from '@angular/common';
import { Component, OnInit, inject } from '@angular/core';
import { FormBuilder, ReactiveFormsModule, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { FontAwesomeModule } from '@fortawesome/angular-fontawesome';
import { faCircleCheck, faEnvelope, faIdCard, faSave, faTrash } from '@fortawesome/free-solid-svg-icons';
import { AuthService } from '../../../../core/auth/services/auth.service';
import { CurrentUserStore } from '../../../../core/auth/services/current-user.store';
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
  private readonly router = inject(Router);
  private readonly fb = inject(FormBuilder);

  //--Variables
  perfil: MiPerfilResponse | null = null;
  isLoading = false;

  readonly faSave = faSave;
  readonly faTrash = faTrash;
  readonly faCircleCheck = faCircleCheck;
  readonly faIdCard = faIdCard;
  readonly faEnvelope = faEnvelope;

  //--Formularios
  // Formulario para editar los datos visibles del perfil.
  readonly form = this.fb.nonNullable.group({
    primerNombre: ['', [Validators.required]],
    segundoNombre: [''],
    primerApellido: ['', [Validators.required]],
    segundoApellido: [''],
    correo: ['', [Validators.required, Validators.email]],
    documento: ['', [Validators.required]]
  });

  //--Métodos
  // Carga el perfil al abrir la pantalla.
  ngOnInit(): void {
    this.cargarPerfil();
  }

  // Consulta el perfil del estudiante autenticado.
  cargarPerfil(): void {
    this.isLoading = true;

    this.estudianteService.getMiPerfil().subscribe({
      next: perfil => {
        this.establecerPerfil(perfil);
        this.isLoading = false;
      },
      error: () => {
        this.isLoading = false;
      }
    });
  }

  // Obtiene el nombre completo para mostrar en la card de perfil.
  obtenerNombreCompleto(): string {
    if (!this.perfil) {
      return 'Estudiante';
    }

    return this.armarNombreCompleto(this.perfil);
  }

  // Calcula iniciales para el avatar del perfil.
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

  // Indica si la cuenta del estudiante está activa.
  estaActivo(): boolean {
    return this.perfil?.estado.toLowerCase() === 'activo';
  }

  // Guarda los cambios del perfil y actualiza el usuario actual en memoria.
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
      correo: rawValue.correo,
      documento: rawValue.documento
    };

    this.isLoading = true;

    this.estudianteService.actualizarMiPerfil(request).subscribe({
      next: perfil => {
        this.establecerPerfil(perfil);
        this.actualizarUsuarioActual(perfil);
        this.form.markAsPristine();
        this.isLoading = false;
      },
      error: () => {
        this.isLoading = false;
      }
    });
  }

  // Inhabilita el perfil académico y cierra la sesión local.
  inhabilitarCuenta(): void {
    const confirmed = confirm('¿Seguro que quieres inhabilitar tu cuenta? No podrás iniciar sesión nuevamente.');

    if (!confirmed) {
      return;
    }

    this.isLoading = true;

    this.estudianteService.inhabilitarMiPerfil().subscribe({
      next: () => {
        this.authService.cerrarSesion();
        this.isLoading = false;
        void this.router.navigate(['/login']);
      },
      error: () => {
        this.isLoading = false;
      }
    });
  }

  // Valida si un campo debe mostrar un error visual.
  mostrarError(controlName: PerfilControlName, errorName: string): boolean {
    const control = this.form.controls[controlName];

    return control.touched && control.hasError(errorName);
  }

  // Actualiza el estado local del perfil y sincroniza el formulario.
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
  }

  // Actualiza el usuario guardado para reflejar nombre y correo nuevos.
  private actualizarUsuarioActual(perfil: MiPerfilResponse): void {
    const currentUser = this.currentUserStore.currentUser();

    if (!currentUser) {
      return;
    }

    this.currentUserStore.establecerUsuario({
      ...currentUser,
      correo: perfil.correo,
      displayName: this.armarNombreCompleto(perfil),
      estado: perfil.estado
    });
  }

  // Arma el nombre completo ignorando campos vacíos.
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
