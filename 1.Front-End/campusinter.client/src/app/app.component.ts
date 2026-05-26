import { Component } from '@angular/core';
import { RouterOutlet } from '@angular/router';
import { GlobalErrorComponent } from './core/errors/global-error/global-error.component';
import { LoadingOverlayComponent } from './core/loading/loading-overlay/loading-overlay.component';

@Component({
  selector: 'app-root',
  standalone: true,
  imports: [
    RouterOutlet,
    LoadingOverlayComponent,
    GlobalErrorComponent
  ],
  templateUrl: './app.component.html',
  styleUrl: './app.component.css'
})
export class AppComponent {
  title = 'campusinter.client';
}
