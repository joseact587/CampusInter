import { HttpContextToken } from '@angular/common/http';

// Permite llamar endpoints públicos sin enviar JWT
export const SKIP_AUTH = new HttpContextToken<boolean>(() => false);

// Permite omitir el manejo global de errores en llamadas puntuales
export const SKIP_ERROR_HANDLER = new HttpContextToken<boolean>(() => false);

// Permite omitir el loading global en llamadas puntuales
export const SKIP_LOADING = new HttpContextToken<boolean>(() => false);
