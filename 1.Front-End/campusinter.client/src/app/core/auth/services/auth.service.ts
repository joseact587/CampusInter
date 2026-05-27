import { HttpClient, HttpContext } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable, tap } from 'rxjs';
import { API_ROUTES } from '../../config/api-routes';
import { SKIP_AUTH } from '../../http/tokens/http-context.tokens';
import { AuthResponse, LoginRequest, RegisterRequest } from '../models/auth.models';
import { CurrentUser } from '../models/current-user.model';
import { CurrentUserStore } from './current-user.store';

@Injectable({
  providedIn: 'root'
})
export class AuthService {
  //--Inyecciones
  constructor(
    private http: HttpClient,
    private currentUserStore: CurrentUserStore
  ) {}

  //--Métodos
  // Registra un estudiante y guarda la sesión recibida.
  registrar(request: RegisterRequest): Observable<AuthResponse> {
    return this.http
      .post<AuthResponse>(API_ROUTES.auth.registrar, request, {
        context: new HttpContext().set(SKIP_AUTH, true)
      })
      .pipe(tap(response => this.currentUserStore.establecerUsuario(this.toCurrentUser(response))));
  }

  // Inicia sesión y guarda el usuario autenticado.
  login(request: LoginRequest): Observable<AuthResponse> {
    return this.http
      .post<AuthResponse>(API_ROUTES.auth.login, request, {
        context: new HttpContext().set(SKIP_AUTH, true)
      })
      .pipe(tap(response => this.currentUserStore.establecerUsuario(this.toCurrentUser(response))));
  }

  // Limpia la sesión local del usuario.
  cerrarSesion(): void {
    this.currentUserStore.limpiar();
  }

  // Adapta la respuesta del backend al modelo de usuario actual del frontend.
  private toCurrentUser(response: AuthResponse): CurrentUser {
    return {
      accessToken: response.accessToken,
      usuarioId: response.usuarioId,
      estudianteId: response.estudianteId,
      correo: response.correo,
      displayName: this.armarNombreVisible(response),
      rol: response.rol,
      roles: response.roles,
      estado: response.estado
    };
  }

  // Arma el nombre visible ignorando partes vacías.
  private armarNombreVisible(response: AuthResponse): string {
    return [
      response.primerNombre,
      response.segundoNombre,
      response.primerApellido,
      response.segundoApellido
    ]
      .filter(value => !!value?.trim())
      .join(' ');
  }
}
