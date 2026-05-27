import { HttpErrorResponse, HttpInterceptorFn } from '@angular/common/http';
import { inject } from '@angular/core';
import { Router } from '@angular/router';
import { catchError, throwError } from 'rxjs';

import { AuthService } from '../../auth/services/auth.service';
import { ErrorService } from '../../errors/error.service';
import { SKIP_ERROR_HANDLER } from '../tokens/http-context.tokens';

//--Métodos
// Interpreta errores HTTP y los muestra en el error global.
export const errorInterceptor: HttpInterceptorFn = (request, next) => {
  const router = inject(Router);
  const authService = inject(AuthService);
  const errorService = inject(ErrorService);
  const shouldSkipErrorHandler = request.context.get(SKIP_ERROR_HANDLER);

  if (shouldSkipErrorHandler) {
    return next(request);
  }

  return next(request).pipe(
    catchError((error: unknown) => {
      if (error instanceof HttpErrorResponse) {
        handleHttpError(error, router, authService, errorService);
      }

      return throwError(() => error);
    })
  );
};

// Maneja cada código HTTP con un mensaje claro para el usuario.
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
      errorService.show(getErrorMessage(error, 'La solicitud no es válida.'));
      return;

    case 401:
      errorService.show('Tu sesión ha expirado. Inicia sesión nuevamente.');
      authService.cerrarSesion();
      void router.navigate(['/login']);
      return;

    case 403:
      errorService.show('No tienes permisos para realizar esta acción.');
      void router.navigate(['/home']);
      return;

    case 404:
      errorService.show(getErrorMessage(error, 'El recurso solicitado no existe.'));
      return;

    case 409:
      errorService.show(getErrorMessage(error, 'Existe un conflicto con la información enviada.'));
      return;

    case 500:
      errorService.show(getErrorMessage(error, 'Ocurrió un error interno. Intenta nuevamente más tarde.'));
      return;

    default:
      errorService.show(getErrorMessage(error, 'Ocurrió un error inesperado.'));
      return;
  }
}

// Extrae mensajes desde ProblemDetails o ValidationProblemDetails.
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
