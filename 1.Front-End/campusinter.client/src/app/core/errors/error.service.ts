import { computed, Injectable, signal } from '@angular/core';

export type AlertType = 'error' | 'success' | 'info';

export interface GlobalAlert {
  message: string;
  type: AlertType;
}

@Injectable({
  providedIn: 'root'
})
export class ErrorService {
  //--Variables
  private closeTimer: ReturnType<typeof setTimeout> | null = null;
  private readonly alert = signal<GlobalAlert | null>(null);

  readonly currentAlert = this.alert.asReadonly();
  readonly currentMessage = computed(() => this.alert()?.message ?? null);

  //--Métodos
  // Mantiene compatibilidad con los interceptores que muestran errores.
  show(message: string): void {
    this.mostrarError(message);
  }

  // Muestra un error global con cierre automático.
  mostrarError(message: string): void {
    this.mostrarAlerta(message, 'error');
  }

  // Muestra una confirmación global con cierre automático.
  mostrarExito(message: string): void {
    this.mostrarAlerta(message, 'success');
  }

  // Muestra un mensaje informativo con cierre automático.
  mostrarInfo(message: string): void {
    this.mostrarAlerta(message, 'info');
  }

  // Limpia el mensaje global visible.
  clear(): void {
    this.limpiarTemporizador();
    this.alert.set(null);
  }

  // Centraliza la creación del toast global.
  private mostrarAlerta(message: string, type: AlertType): void {
    this.limpiarTemporizador();
    this.alert.set({ message, type });

    this.closeTimer = setTimeout(() => {
      this.alert.set(null);
      this.closeTimer = null;
    }, 4500);
  }

  // Evita temporizadores duplicados cuando llega una nueva alerta.
  private limpiarTemporizador(): void {
    if (!this.closeTimer) {
      return;
    }

    clearTimeout(this.closeTimer);
    this.closeTimer = null;
  }
}
