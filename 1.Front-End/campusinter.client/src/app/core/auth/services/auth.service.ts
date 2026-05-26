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
  constructor(
    private http: HttpClient,
    private currentUserStore: CurrentUserStore
  ) {}

  // Registro publico
  registrar(request: RegisterRequest): Observable<AuthResponse> {
    return this.http
      .post<AuthResponse>(API_ROUTES.auth.registrar, request, {
        context: new HttpContext().set(SKIP_AUTH, true)
      })
      .pipe(tap(response => this.currentUserStore.setUser(this.toCurrentUser(response))));
  }

  // Login publico
  login(request: LoginRequest): Observable<AuthResponse> {
    return this.http
      .post<AuthResponse>(API_ROUTES.auth.login, request, {
        context: new HttpContext().set(SKIP_AUTH, true)
      })
      .pipe(tap(response => this.currentUserStore.setUser(this.toCurrentUser(response))));
  }

  // Cierre de sesion local
  cerrarSesion(): void {
    this.currentUserStore.clear();
  }

  // Alias usado por interceptores y pantallas
  logout(): void {
    this.cerrarSesion();
  }

  // Adaptar respuesta del backend al usuario actual del frontend
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

  // Nombre visible
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
