import { Component } from '@angular/core';
import { RouterOutlet } from '@angular/router';

// Layout base para pantallas privadas
@Component({
  selector: 'app-layout-principal',
  standalone: true,
  imports: [RouterOutlet],
  templateUrl: './layout-principal.component.html',
  styleUrl: './layout-principal.component.css'
})
export class LayoutPrincipalComponent {}
