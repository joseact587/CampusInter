import { HttpInterceptorFn } from '@angular/common/http';
import { inject } from '@angular/core';
import { finalize } from 'rxjs';

import { LoadingService } from '../../loading/loading.service';
import { SKIP_LOADING } from '../tokens/http-context.tokens';

//--Métodos
// Muestra y oculta el loading global durante peticiones HTTP.
export const loadingInterceptor: HttpInterceptorFn = (request, next) => {
  const loadingService = inject(LoadingService);
  const shouldSkipLoading = request.context.get(SKIP_LOADING);

  if (shouldSkipLoading) {
    return next(request);
  }

  loadingService.show();

  return next(request).pipe(
    finalize(() => loadingService.hide())
  );
};
