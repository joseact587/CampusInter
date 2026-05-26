import { inject } from '@angular/core';
import { CanActivateFn, Router } from '@angular/router';
import { CurrentUserStore } from '../services/current-user.store';

export const authGuard: CanActivateFn = () => {
  const currentUserStore = inject(CurrentUserStore);
  const router = inject(Router);

  // Usuario autenticado
  if (currentUserStore.isAuthenticated()) {
    return true;
  }

  // Usuario no autenticado
  return router.createUrlTree(['/login']);
};
