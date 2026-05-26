import { HttpInterceptorFn } from '@angular/common/http';
import { inject } from '@angular/core';

import { TokenStorageService } from '../../auth/services/token-storage.service';
import { SKIP_AUTH } from '../tokens/http-context.tokens';

export const authInterceptor: HttpInterceptorFn = (request, next) => {
  const tokenStorage = inject(TokenStorageService);
  const shouldSkipAuth = request.context.get(SKIP_AUTH);

  // Endpoints públicos
  if (shouldSkipAuth) {
    return next(request);
  }

  const accessToken = tokenStorage.getAccessToken();

  if (!accessToken) {
    return next(request);
  }

  // Token Bearer
  const requestWithAuth = request.clone({
    setHeaders: {
      Authorization: `Bearer ${accessToken}`
    }
  });

  return next(requestWithAuth);
};
