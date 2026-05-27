import { Injectable } from '@angular/core';
import { CurrentUser } from '../models/current-user.model';

const ACCESS_TOKEN_KEY = 'campus_inter_access_token';
const CURRENT_USER_KEY = 'campus_inter_current_user';

@Injectable({
  providedIn: 'root'
})
export class TokenStorageService {
  //--Métodos
  // Obtiene el token JWT almacenado.
  getAccessToken(): string | null {
    return localStorage.getItem(ACCESS_TOKEN_KEY);
  }

  // Guarda la sesión completa del usuario.
  setSession(user: CurrentUser): void {
    localStorage.setItem(ACCESS_TOKEN_KEY, user.accessToken);
    localStorage.setItem(CURRENT_USER_KEY, JSON.stringify(user));
  }

  // Obtiene el usuario actual almacenado.
  getCurrentUser(): CurrentUser | null {
    const rawUser = localStorage.getItem(CURRENT_USER_KEY);

    if (!rawUser) {
      return null;
    }

    try {
      return JSON.parse(rawUser) as CurrentUser;
    } catch {
      this.clear();
      return null;
    }
  }

  // Limpia la sesión almacenada.
  clear(): void {
    localStorage.removeItem(ACCESS_TOKEN_KEY);
    localStorage.removeItem(CURRENT_USER_KEY);
  }
}
