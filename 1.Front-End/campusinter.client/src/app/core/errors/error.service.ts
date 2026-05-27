import { Injectable, signal } from '@angular/core';

@Injectable({
  providedIn: 'root'
})
export class ErrorService {
  //--Variables
  private readonly message = signal<string | null>(null);
  readonly currentMessage = this.message.asReadonly();

  //--Métodos
  // Muestra un mensaje global de error.
  show(message: string): void {
    this.message.set(message);
  }

  // Limpia el mensaje global de error.
  clear(): void {
    this.message.set(null);
  }
}
