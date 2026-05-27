export interface EstudianteResumenResponse {
  estudianteId: number;
  primerNombre: string;
  segundoNombre?: string | null;
  primerApellido: string;
  segundoApellido?: string | null;
  correo?: string | null;
  documento?: string | null;
  estado: string;
}

export interface MiPerfilResponse {
  estudianteId: number;
  usuarioId?: number;
  primerNombre: string;
  segundoNombre?: string | null;
  primerApellido: string;
  segundoApellido?: string | null;
  correo: string;
  documento: string;
  estado: string;
}

export interface ActualizarMiPerfilRequest {
  primerNombre: string;
  segundoNombre?: string | null;
  primerApellido: string;
  segundoApellido?: string | null;
  documento: string;
}
