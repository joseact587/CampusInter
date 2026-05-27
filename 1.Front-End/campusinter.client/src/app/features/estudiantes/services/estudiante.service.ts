import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { API_ROUTES } from '../../../core/config/api-routes';
import { ActualizarMiPerfilRequest, EstudianteResumenResponse, MiPerfilResponse } from '../models/estudiante.models';

@Injectable({
  providedIn: 'root'
})
export class EstudianteService {
  //--Inyecciones
  constructor(private http: HttpClient) {}

  //--Métodos
  // Lista los estudiantes activos registrados.
  getEstudiantes(): Observable<EstudianteResumenResponse[]> {
    return this.http.get<EstudianteResumenResponse[]>(API_ROUTES.estudiantes.obtenerTodos);
  }

  // Consulta el perfil del estudiante autenticado.
  getMiPerfil(): Observable<MiPerfilResponse> {
    return this.http.get<MiPerfilResponse>(API_ROUTES.estudiantes.miPerfil);
  }

  // Actualiza el perfil del estudiante autenticado.
  actualizarMiPerfil(request: ActualizarMiPerfilRequest): Observable<MiPerfilResponse> {
    return this.http.put<MiPerfilResponse>(API_ROUTES.estudiantes.actualizarMiPerfil, request);
  }

  // Inhabilita el perfil académico del estudiante autenticado.
  inhabilitarMiPerfil(): Observable<void> {
    return this.http.patch<void>(API_ROUTES.estudiantes.inhabilitarMiPerfil, {});
  }

  // Habilita el perfil académico del estudiante autenticado.
  habilitarMiPerfil(): Observable<void> {
    return this.http.patch<void>(API_ROUTES.estudiantes.habilitarMiPerfil, {});
  }
}
