import { Routes } from '@angular/router';
import { authGuard } from './core/auth/guards/auth.guard';
import { LoginComponent } from './features/auth/pages/login/login.component';
import { RegistroComponent } from './features/auth/pages/registro/registro.component';
import { HomeComponent } from './features/dashboard/pages/home/home.component';

export const routes: Routes = [
  // Ruta inicial
  {
    path: '',
    pathMatch: 'full',
    redirectTo: 'home'
  },

  // Autenticación
  {
    path: 'login',
    component: LoginComponent
  },
  {
    path: 'registro',
    component: RegistroComponent
  },

  // Zona privada
  {
    path: 'home',
    component: HomeComponent,
    canActivate: [authGuard]
  },

  // Ruta no encontrada
  {
    path: '**',
    redirectTo: 'home'
  }
];
