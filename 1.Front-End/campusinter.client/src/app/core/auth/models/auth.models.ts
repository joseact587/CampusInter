// Datos para registrar un estudiante
export interface RegisterRequest {
  primerNombre: string;
  segundoNombre?: string | null;
  primerApellido: string;
  segundoApellido?: string | null;
  correo: string;
  documento: string;
  password: string;
}

// Datos para iniciar sesión
export interface LoginRequest {
  correo: string;
  password: string;
}

// Respuesta de autenticación entregada por la API
export interface AuthResponse {
  accessToken: string;
  usuarioId: number;
  estudianteId: number;
  correo: string;
  primerNombre: string;
  segundoNombre?: string | null;
  primerApellido: string;
  segundoApellido?: string | null;
  rol: string;
  roles: string[];
  estado: string;
}
