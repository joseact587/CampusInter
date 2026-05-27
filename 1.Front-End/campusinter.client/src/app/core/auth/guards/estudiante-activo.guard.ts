import { inject } from '@angular/core';
import { CanActivateChildFn, Router } from '@angular/router';
import { CurrentUserStore } from '../services/current-user.store';

export const estudianteActivoGuard: CanActivateChildFn = (route) => {
  const currentUserStore = inject(CurrentUserStore);
  const router = inject(Router);

  //--Variables
  const estado = currentUserStore.currentUser()?.estado ?? '';
  const estaActivo = estado.toLowerCase() === 'activo';
  const rutaActual = route.routeConfig?.path ?? '';

  //--Métodos
  // Permite siempre consultar el perfil propio.
  if (rutaActual === 'mi-perfil') {
    return true;
  }

  // Si el estudiante está activo puede navegar por todo el módulo privado.
  if (estaActivo) {
    return true;
  }

  // Si está inactivo, lo limita a su perfil.
  return router.createUrlTree(['/mi-perfil']);
};
