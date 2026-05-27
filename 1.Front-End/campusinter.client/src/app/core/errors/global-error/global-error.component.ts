import { Component, inject } from '@angular/core';
import { ErrorService } from '../error.service';

@Component({
  selector: 'app-global-error',
  standalone: true,
  templateUrl: './global-error.component.html',
  styleUrl: './global-error.component.css'
})
export class GlobalErrorComponent {
  //--Inyecciones
  readonly errorService = inject(ErrorService);
}
