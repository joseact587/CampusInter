import { Routes } from '@angular/router';
import { authGuard } from './core/auth/guards/auth.guard';
import { estudianteActivoGuard } from './core/auth/guards/estudiante-activo.guard';
import { PrivateLayoutComponent } from './core/layout/private-layout/private-layout.component';
import { LoginComponent } from './features/auth/pages/login/login.component';
import { RegistroComponent } from './features/auth/pages/registro/registro.component';
import { HomeComponent } from './features/dashboard/pages/home/home.component';
import { EstudianteListComponent } from './features/estudiantes/pages/estudiante-list/estudiante-list.component';
import { MiPerfilComponent } from './features/estudiantes/pages/mi-perfil/mi-perfil.component';
import { MiInscripcionComponent } from './features/inscripciones/pages/mi-inscripcion/mi-inscripcion.component';
import { MateriasPageComponent } from './features/materias/pages/materias-page/materias-page.component';

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
    path: 'register',
    component: RegistroComponent
  },

  // Zona privada
  {
    path: '',
    component: PrivateLayoutComponent,
    canActivate: [authGuard],
    canActivateChild: [estudianteActivoGuard],
    children: [
      {
        path: 'home',
        component: HomeComponent
      },
      {
        path: 'materias',
        component: MateriasPageComponent
      },
      {
        path: 'mi-inscripcion',
        component: MiInscripcionComponent
      },
      {
        path: 'estudiantes',
        component: EstudianteListComponent
      },
      {
        path: 'mi-perfil',
        component: MiPerfilComponent
      }
    ]
  },

  // Ruta no encontrada
  {
    path: '**',
    redirectTo: 'home'
  }
];
