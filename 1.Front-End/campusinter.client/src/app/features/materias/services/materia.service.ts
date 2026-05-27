import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { API_ROUTES } from '../../../core/config/api-routes';
import { MateriaResponse } from '../models/materia.models';

@Injectable({
  providedIn: 'root'
})
export class MateriaService {
  //--Inyecciones
  constructor(private http: HttpClient) {}

  //--Métodos
  // Consulta el catálogo de materias disponibles.
  getMaterias(): Observable<MateriaResponse[]> {
    return this.http.get<MateriaResponse[]>(API_ROUTES.materias.getAll);
  }
}
