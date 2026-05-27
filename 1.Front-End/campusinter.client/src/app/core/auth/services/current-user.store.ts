import { computed, Injectable, signal } from '@angular/core';
import { CurrentUser } from '../models/current-user.model';
import { TokenStorageService } from './token-storage.service';

@Injectable({
  providedIn: 'root'
})
export class CurrentUserStore {
  //--Inyecciones
  constructor(private tokenStorage: TokenStorageService) {
    this.cargarDesdeStorage();
  }

  //--Variables
  currentUser = signal<CurrentUser | null>(null);
  estaAutenticado = computed(() => !!this.currentUser()?.accessToken);

  //--Métodos
  // Guarda el usuario en memoria y localStorage.
  establecerUsuario(user: CurrentUser): void {
    this.tokenStorage.setSession(user);
    this.currentUser.set(user);
  }

  // Limpia el usuario actual y los datos persistidos.
  limpiar(): void {
    this.tokenStorage.clear();
    this.currentUser.set(null);
  }

  // Restaura la sesión guardada al iniciar la aplicación.
  cargarDesdeStorage(): void {
    this.currentUser.set(this.tokenStorage.getCurrentUser());
  }
}
