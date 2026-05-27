import { environment } from '../../../environments/environment';

const API_BASE_URL = environment.apiBaseUrl;

// Endpoints centralizados de la API
export const API_ROUTES = {
  // Autenticación
  auth: {
    registrar: `${API_BASE_URL}/api/auth/register`,
    login: `${API_BASE_URL}/api/auth/login`
  },

  // Materias disponibles
  materias: {
    obtenerTodas: `${API_BASE_URL}/api/materias`
  },

  // Inscripciones del estudiante
  inscripciones: {
    crear: `${API_BASE_URL}/api/inscripciones`,
    miInscripcion: `${API_BASE_URL}/api/inscripciones/mi-inscripcion`,
    cancelarMiInscripcion: `${API_BASE_URL}/api/inscripciones/mi-inscripcion`,
    companeros: `${API_BASE_URL}/api/inscripciones/mi-inscripcion/companeros`
  },

  // Estudiantes
  estudiantes: {
    obtenerTodos: `${API_BASE_URL}/api/estudiantes`,
    miPerfil: `${API_BASE_URL}/api/estudiantes/me`,
    actualizarMiPerfil: `${API_BASE_URL}/api/estudiantes/me`,
    inhabilitarMiPerfil: `${API_BASE_URL}/api/estudiantes/me/inhabilitar`,
    habilitarMiPerfil: `${API_BASE_URL}/api/estudiantes/me/habilitar`
  }
} as const;
