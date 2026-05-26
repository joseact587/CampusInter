import { Component, inject } from '@angular/core';
import { LoadingService } from '../loading.service';

// Overlay global para peticiones HTTP
@Component({
  selector: 'app-loading-overlay',
  standalone: true,
  templateUrl: './loading-overlay.component.html',
  styleUrl: './loading-overlay.component.css'
})
export class LoadingOverlayComponent {
  readonly loadingService = inject(LoadingService);
}
