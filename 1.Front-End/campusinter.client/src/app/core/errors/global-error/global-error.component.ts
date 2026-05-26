import { Component, inject } from '@angular/core';
import { ErrorService } from '../error.service';

// Toast global de errores
@Component({
  selector: 'app-global-error',
  standalone: true,
  templateUrl: './global-error.component.html',
  styleUrl: './global-error.component.css'
})
export class GlobalErrorComponent {
  readonly errorService = inject(ErrorService);
}
