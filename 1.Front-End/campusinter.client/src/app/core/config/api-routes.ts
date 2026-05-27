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
    getAll: `${API_BASE_URL}/api/materias`
  },

  // Inscripciones del estudiante
  inscripciones: {
    create: `${API_BASE_URL}/api/inscripciones`,
    myEnrollment: `${API_BASE_URL}/api/inscripciones/mi-inscripcion`,
    classmates: `${API_BASE_URL}/api/inscripciones/mi-inscripcion/companeros`
  },

  // Estudiantes
  estudiantes: {
    getAll: `${API_BASE_URL}/api/estudiantes`,
    me: `${API_BASE_URL}/api/estudiantes/me`,
    updateMe: `${API_BASE_URL}/api/estudiantes/me`,
    inhabilitarMe: `${API_BASE_URL}/api/estudiantes/me/inhabilitar`,
    habilitarMe: `${API_BASE_URL}/api/estudiantes/me/habilitar`
  }
} as const;
