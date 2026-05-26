import { computed, Injectable, signal } from '@angular/core';
import { CurrentUser } from '../models/current-user.model';
import { TokenStorageService } from './token-storage.service';

@Injectable({
  providedIn: 'root'
})
export class CurrentUserStore {
  // Estado del usuario autenticado
  currentUser = signal<CurrentUser | null>(null);

  // Estado derivado para guards y vistas
  isAuthenticated = computed(() => !!this.currentUser()?.accessToken);

  constructor(private tokenStorage: TokenStorageService) {
    this.loadFromStorage();
  }

  // Iniciar sesión en memoria y storage
  setUser(user: CurrentUser): void {
    this.tokenStorage.setSession(user);
    this.currentUser.set(user);
  }

  // Limpiar sesión
  clear(): void {
    this.tokenStorage.clear();
    this.currentUser.set(null);
  }

  // Restaurar sesión al iniciar la aplicación
  loadFromStorage(): void {
    this.currentUser.set(this.tokenStorage.getCurrentUser());
  }
}
