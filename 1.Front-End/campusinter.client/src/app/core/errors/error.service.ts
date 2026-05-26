import { Injectable, signal } from '@angular/core';

@Injectable({
  providedIn: 'root'
})
export class ErrorService {
  // Mensaje global activo
  private readonly message = signal<string | null>(null);

  // Estado de solo lectura para la vista
  readonly currentMessage = this.message.asReadonly();

  show(message: string): void {
    this.message.set(message);
  }

  clear(): void {
    this.message.set(null);
  }
}
