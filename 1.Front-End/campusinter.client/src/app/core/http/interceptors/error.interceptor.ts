import { HttpErrorResponse, HttpInterceptorFn } from '@angular/common/http';
import { inject } from '@angular/core';
import { Router } from '@angular/router';
import { catchError, throwError } from 'rxjs';

import { AuthService } from '../../auth/services/auth.service';
import { ErrorService } from '../../errors/error.service';
import { SKIP_ERROR_HANDLER } from '../tokens/http-context.tokens';

export const errorInterceptor: HttpInterceptorFn = (request, next) => {
  const router = inject(Router);
  const authService = inject(AuthService);
  const errorService = inject(ErrorService);
  const shouldSkipErrorHandler = request.context.get(SKIP_ERROR_HANDLER);

  // Llamadas que manejan sus propios errores
  if (shouldSkipErrorHandler) {
    return next(request);
  }

  return next(request).pipe(
    catchError((error: unknown) => {
      // Errores HTTP controlados
      if (error instanceof HttpErrorResponse) {
        handleHttpError(error, router, authService, errorService);
      }

      return throwError(() => error);
    })
  );
};

function handleHttpError(
  error: HttpErrorResponse,
  router: Router,
  authService: AuthService,
  errorService: ErrorService
): void {
  switch (error.status) {
    case 0:
      errorService.show('No se pudo conectar con el servidor.');
      return;

    case 400:
      errorService.show(getErrorMessage(error, 'La solicitud no es valida.'));
      return;

    case 401:
      errorService.show('Tu sesion ha expirado. Inicia sesion nuevamente.');
      authService.logout();
      void router.navigate(['/login']);
      return;

    case 403:
      errorService.show('No tienes permisos para realizar esta accion.');
      void router.navigate(['/home']);
      return;

    case 404:
      errorService.show(getErrorMessage(error, 'El recurso solicitado no existe.'));
      return;

    case 409:
      errorService.show(getErrorMessage(error, 'Existe un conflicto con la informacion enviada.'));
      return;

    case 500:
      errorService.show(getErrorMessage(error, 'Ocurrio un error interno. Intenta nuevamente mas tarde.'));
      return;

    default:
      errorService.show(getErrorMessage(error, 'Ocurrio un error inesperado.'));
      return;
  }
}

function getErrorMessage(error: HttpErrorResponse, fallback: string): string {
  const detail = error.error?.detail;
  const message = error.error?.message;
  const title = error.error?.title;
  const errors = error.error?.errors;

  if (typeof detail === 'string' && detail.trim().length > 0) {
    return detail;
  }

  if (typeof message === 'string' && message.trim().length > 0) {
    return message;
  }

  if (errors && typeof errors === 'object') {
    const validationMessages = Object.values(errors)
      .flat()
      .filter((value): value is string => typeof value === 'string');

    if (validationMessages.length > 0) {
      return validationMessages.join(' ');
    }
  }

  if (typeof title === 'string' && title.trim().length > 0) {
    return title;
  }

  return fallback;
}
