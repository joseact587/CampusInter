import { CommonModule } from '@angular/common';
import { HttpErrorResponse } from '@angular/common/http';
import { Component, OnInit, inject } from '@angular/core';
import { Router } from '@angular/router';
import { FontAwesomeModule } from '@fortawesome/angular-fontawesome';
import {
  faBookBookmark,
  faBookOpen,
  faChevronDown,
  faChevronUp,
  faGraduationCap,
  faUser
} from '@fortawesome/free-solid-svg-icons';
import { catchError, of, switchMap, throwError } from 'rxjs';
import { CompaneroResponse, InscripcionResponse, MateriaCompanerosResponse } from '../../models/inscripcion.models';
import { InscripcionService } from '../../services/inscripcion.service';

@Component({
  selector: 'app-mi-inscripcion',
  standalone: true,
  imports: [
    CommonModule,
    FontAwesomeModule
  ],
  templateUrl: './mi-inscripcion.component.html',
  styleUrl: './mi-inscripcion.component.css'
})
export class MiInscripcionComponent implements OnInit {
  //--Inyecciones
  private readonly router = inject(Router);
  private readonly inscripcionService = inject(InscripcionService);

  //--Variables
  inscripcion: InscripcionResponse | null = null;
  companerosPorMateria: MateriaCompanerosResponse[] = [];
  expandedMateriaIds = new Set<number>();
  isLoading = false;

  readonly faBookOpen = faBookOpen;
  readonly faBookBookmark = faBookBookmark;
  readonly faChevronDown = faChevronDown;
  readonly faChevronUp = faChevronUp;
  readonly faUser = faUser;
  readonly faGraduationCap = faGraduationCap;

  //--Métodos
  // Carga la inscripción y sus compañeros al abrir la pantalla.
  ngOnInit(): void {
    this.cargarDatos();
  }

  // Consulta la inscripción activa y luego los compañeros por materia.
  cargarDatos(): void {
    this.isLoading = true;

    this.inscripcionService.getMiInscripcion(true)
      .pipe(
        catchError(error => this.manejarErrorMiInscripcion(error)),
        switchMap(inscripcion => {
          this.inscripcion = inscripcion;

          if (!inscripcion) {
            return of([]);
          }

          return this.inscripcionService.getCompanerosPorMiInscripcion(true).pipe(
            catchError(() => of([]))
          );
        })
      )
      .subscribe({
        next: companerosPorMateria => {
          this.companerosPorMateria = companerosPorMateria;
          this.isLoading = false;
        },
        error: () => {
          this.isLoading = false;
        }
      });
  }

  // Abre o cierra el panel de compañeros de una materia.
  alternarCompaneros(materiaId: number): void {
    if (this.expandedMateriaIds.has(materiaId)) {
      this.expandedMateriaIds.delete(materiaId);
      return;
    }

    this.expandedMateriaIds.add(materiaId);
  }

  // Indica si una materia tiene el panel de compañeros abierto.
  estaExpandida(materiaId: number): boolean {
    return this.expandedMateriaIds.has(materiaId);
  }

  // Obtiene los compañeros asociados a una materia.
  obtenerCompaneros(materiaId: number): CompaneroResponse[] {
    return this.companerosPorMateria.find(materia => materia.materiaId === materiaId)?.companeros ?? [];
  }

  // Cuenta cuántos compañeros comparte el estudiante en una materia.
  contarCompaneros(materiaId: number): number {
    return this.obtenerCompaneros(materiaId).length;
  }

  // Construye el texto singular o plural de compañeros.
  obtenerTextoCompaneros(materiaId: number): string {
    const count = this.contarCompaneros(materiaId);

    if (count === 0) {
      return '0 compañeros';
    }

    if (count === 1) {
      return '1 compañero';
    }

    return `${count} compañeros`;
  }

  // Obtiene iniciales para los chips de compañeros.
  obtenerIniciales(nombreCompleto: string): string {
    const palabras = nombreCompleto
      .split(' ')
      .map(palabra => palabra.trim())
      .filter(palabra => palabra.length > 0);

    if (palabras.length === 0) {
      return 'NA';
    }

    return palabras
      .slice(0, 2)
      .map(palabra => palabra[0].toUpperCase())
      .join('');
  }

  // Navega a la pantalla para inscribir materias.
  goToMaterias(): void {
    void this.router.navigate(['/materias']);
  }

  // Formatea la fecha de inscripción para mostrarla en español.
  formatearFecha(fecha: string): string {
    return new Intl.DateTimeFormat('es-CO', {
      day: '2-digit',
      month: 'long',
      year: 'numeric'
    }).format(new Date(fecha));
  }

  // Convierte el 404 de inscripción en estado vacío sin mostrar error global.
  private manejarErrorMiInscripcion(error: unknown) {
    if (error instanceof HttpErrorResponse && error.status === 404) {
      return of(null);
    }

    return throwError(() => error);
  }
}
