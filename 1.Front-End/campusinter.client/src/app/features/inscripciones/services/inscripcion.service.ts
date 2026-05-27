import { HttpClient, HttpContext } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { API_ROUTES } from '../../../core/config/api-routes';
import { SKIP_ERROR_HANDLER } from '../../../core/http/tokens/http-context.tokens';
import { CrearInscripcionRequest, InscripcionResponse, MateriaCompanerosResponse } from '../models/inscripcion.models';

@Injectable({
  providedIn: 'root'
})
export class InscripcionService {
  //--Inyecciones
  constructor(private http: HttpClient) {}

  //--Métodos
  // Consulta la inscripción activa del estudiante autenticado.
  getMiInscripcion(omitirErrorGlobal = false): Observable<InscripcionResponse> {
    return this.http.get<InscripcionResponse>(
      API_ROUTES.inscripciones.myEnrollment,
      {
        context: this.construirContexto(omitirErrorGlobal)
      }
    );
  }

  // Crea la inscripción del estudiante autenticado.
  crearInscripcion(request: CrearInscripcionRequest): Observable<InscripcionResponse> {
    return this.http.post<InscripcionResponse>(API_ROUTES.inscripciones.create, request);
  }

  // Consulta los compañeros agrupados por materia.
  getCompanerosPorMiInscripcion(omitirErrorGlobal = false): Observable<MateriaCompanerosResponse[]> {
    return this.http.get<MateriaCompanerosResponse[]>(
      API_ROUTES.inscripciones.classmates,
      {
        context: this.construirContexto(omitirErrorGlobal)
      }
    );
  }

  // Construye el contexto HTTP para omitir errores globales cuando aplica.
  private construirContexto(omitirErrorGlobal: boolean): HttpContext {
    const context = new HttpContext();

    return omitirErrorGlobal
      ? context.set(SKIP_ERROR_HANDLER, true)
      : context;
  }
}
