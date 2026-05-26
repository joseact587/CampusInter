// Usuario autenticado guardado en el frontend
export interface CurrentUser {
  accessToken: string;
  usuarioId: number;
  estudianteId: number;
  correo: string;
  displayName: string;
  rol: string;
  roles: string[];
  estado: string;
}
