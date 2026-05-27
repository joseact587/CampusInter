import { CommonModule } from '@angular/common';
import { Component, inject } from '@angular/core';
import { FormBuilder, ReactiveFormsModule, Validators } from '@angular/forms';
import { Router, RouterLink } from '@angular/router';
import { AuthService } from '../../../../core/auth/services/auth.service';

@Component({
  selector: 'app-login',
  standalone: true,
  imports: [
    CommonModule,
    ReactiveFormsModule,
    RouterLink
  ],
  templateUrl: './login.component.html',
  styleUrl: './login.component.css'
})
export class LoginComponent {
  //--Inyecciones
  private readonly fb = inject(FormBuilder);
  private readonly authService = inject(AuthService);
  private readonly router = inject(Router);

  //--Variables
  mostrarPassword = false;

  //--Formularios
  // Formulario de inicio de sesión.
  readonly form = this.fb.nonNullable.group({
    correo: ['', [Validators.required, Validators.email]],
    password: ['', [Validators.required]]
  });

  //--Métodos
  // Envía las credenciales y redirige al panel principal si el login es correcto.
  iniciarSesion(): void {
    if (this.form.invalid) {
      this.form.markAllAsTouched();
      return;
    }

    this.authService.login(this.form.getRawValue()).subscribe({
      next: () => {
        void this.router.navigate(['/home']);
      }
    });
  }

  // Alterna la visibilidad del campo contraseña.
  alternarVisibilidadPassword(): void {
    this.mostrarPassword = !this.mostrarPassword;
  }

  // Valida si un campo debe mostrar un error visual.
  mostrarError(controlName: 'correo' | 'password', errorName: string): boolean {
    const control = this.form.controls[controlName];

    return control.touched && control.hasError(errorName);
  }
}
