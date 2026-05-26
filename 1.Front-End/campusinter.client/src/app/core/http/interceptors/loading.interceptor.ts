import { HttpInterceptorFn } from '@angular/common/http';
import { inject } from '@angular/core';
import { finalize } from 'rxjs';

import { LoadingService } from '../../loading/loading.service';
import { SKIP_LOADING } from '../tokens/http-context.tokens';

export const loadingInterceptor: HttpInterceptorFn = (request, next) => {
  const loadingService = inject(LoadingService);
  const shouldSkipLoading = request.context.get(SKIP_LOADING);

  // Llamadas sin loading visual
  if (shouldSkipLoading) {
    return next(request);
  }

  loadingService.show();

  return next(request).pipe(
    finalize(() => loadingService.hide())
  );
};
