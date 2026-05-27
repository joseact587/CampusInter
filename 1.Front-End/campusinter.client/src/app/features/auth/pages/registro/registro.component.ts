import { CommonModule } from '@angular/common';
import { Component, inject } from '@angular/core';
import { FormBuilder, ReactiveFormsModule, Validators } from '@angular/forms';
import { Router, RouterLink } from '@angular/router';
import { RegisterRequest } from '../../../../core/auth/models/auth.models';
import { AuthService } from '../../../../core/auth/services/auth.service';

@Component({
  selector: 'app-registro',
  standalone: true,
  imports: [
    CommonModule,
    ReactiveFormsModule,
    RouterLink
  ],
  templateUrl: './registro.component.html',
  styleUrl: './registro.component.css'
})
export class RegistroComponent {
  //--Inyecciones
  private readonly fb = inject(FormBuilder);
  private readonly authService = inject(AuthService);
  private readonly router = inject(Router);

  //--Variables
  showPassword = false;

  //--Formularios
  // Formulario de creación de cuenta de estudiante.
  readonly form = this.fb.nonNullable.group({
    primerNombre: ['', [Validators.required]],
    segundoNombre: [''],
    primerApellido: ['', [Validators.required]],
    segundoApellido: [''],
    correo: ['', [Validators.required, Validators.email]],
    password: ['', [Validators.required, Validators.minLength(6)]],
    documento: ['', [Validators.required]]
  });

  //--Métodos
  // Envía los datos de registro y redirige al panel principal si todo sale bien.
  submit(): void {
    if (this.form.invalid) {
      this.form.markAllAsTouched();
      return;
    }

    const rawValue = this.form.getRawValue();
    const request: RegisterRequest = {
      primerNombre: rawValue.primerNombre,
      segundoNombre: rawValue.segundoNombre || null,
      primerApellido: rawValue.primerApellido,
      segundoApellido: rawValue.segundoApellido || null,
      correo: rawValue.correo,
      documento: rawValue.documento,
      password: rawValue.password
    };

    this.authService.registrar(request).subscribe({
      next: () => {
        void this.router.navigate(['/home']);
      }
    });
  }

  // Alterna la visibilidad del campo contraseña.
  togglePasswordVisibility(): void {
    this.showPassword = !this.showPassword;
  }

  // Valida si un campo debe mostrar un error visual.
  hasError(
    controlName: 'primerNombre' | 'primerApellido' | 'correo' | 'documento' | 'password',
    errorName: string
  ): boolean {
    const control = this.form.controls[controlName];

    return control.touched && control.hasError(errorName);
  }
}
