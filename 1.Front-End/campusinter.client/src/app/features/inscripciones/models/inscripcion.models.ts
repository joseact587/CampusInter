export interface CrearInscripcionRequest {
  materiasIds: number[];
}

export interface InscripcionMateriaResponse {
  materiaId: number;
  nombre: string;
  creditos: number;
  profesorId: number;
  profesorNombre: string;
}

export interface InscripcionResponse {
  inscripcionId: number;
  estudianteId: number;
  fechaInscripcion: string;
  totalCreditos: number;
  estado: string;
  materias: InscripcionMateriaResponse[];
}

export interface CompaneroResponse {
  nombreCompleto: string;
}

export interface MateriaCompanerosResponse {
  materiaId: number;
  materiaNombre: string;
  companeros: CompaneroResponse[];
}
