import { computed, Injectable, signal } from '@angular/core';

@Injectable({
  providedIn: 'root'
})
export class LoadingService {
  //--Variables
  private readonly activeRequests = signal(0);
  readonly isLoading = computed(() => this.activeRequests() > 0);

  //--Métodos
  // Aumenta el contador de peticiones activas.
  show(): void {
    this.activeRequests.update(current => current + 1);
  }

  // Disminuye el contador de peticiones activas.
  hide(): void {
    this.activeRequests.update(current => Math.max(0, current - 1));
  }

  // Reinicia el estado global de carga.
  reset(): void {
    this.activeRequests.set(0);
  }
}
