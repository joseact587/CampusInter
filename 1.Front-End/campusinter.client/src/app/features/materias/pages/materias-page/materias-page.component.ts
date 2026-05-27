import { CommonModule } from '@angular/common';
import { HttpErrorResponse } from '@angular/common/http';
import { Component, OnInit, inject } from '@angular/core';
import { Router } from '@angular/router';
import { FontAwesomeModule } from '@fortawesome/angular-fontawesome';
import { faCircleCheck, faCircleExclamation, faXmark } from '@fortawesome/free-solid-svg-icons';
import { catchError, forkJoin, of, throwError } from 'rxjs';
import { CrearInscripcionRequest, InscripcionResponse } from '../../../inscripciones/models/inscripcion.models';
import { InscripcionService } from '../../../inscripciones/services/inscripcion.service';
import { MateriaResponse } from '../../models/materia.models';
import { MateriaService } from '../../services/materia.service';

@Component({
  selector: 'app-materias-page',
  standalone: true,
  imports: [
    CommonModule,
    FontAwesomeModule
  ],
  templateUrl: './materias-page.component.html',
  styleUrl: './materias-page.component.css'
})
export class MateriasPageComponent implements OnInit {
  //--Inyecciones
  private readonly materiaService = inject(MateriaService);
  private readonly inscripcionService = inject(InscripcionService);
  private readonly router = inject(Router);

  //--Variables
  materias: MateriaResponse[] = [];
  miInscripcion: InscripcionResponse | null = null;
  materiasSeleccionadas: MateriaResponse[] = [];
  tieneInscripcionActiva = false;
  isLoading = false;

  readonly faCircleCheck = faCircleCheck;
  readonly faCircleExclamation = faCircleExclamation;
  readonly faXmark = faXmark;

  get cantidadSeleccionada(): number {
    return this.materiasSeleccionadas.length;
  }

  get creditosSeleccionados(): number {
    return this.materiasSeleccionadas.reduce((total, materia) => total + materia.creditos, 0);
  }

  get porcentajeProgreso(): number {
    return (this.cantidadSeleccionada / 3) * 100;
  }

  get mensajeMateriasFaltantes(): string {
    const faltantes = 3 - this.cantidadSeleccionada;

    if (faltantes === 3) {
      return 'Selecciona 3 materias';
    }

    if (faltantes === 1) {
      return 'Selecciona 1 materia más';
    }

    return `Selecciona ${faltantes} materias más`;
  }

  //--Métodos
  // Carga materias e inscripción activa al abrir la pantalla.
  ngOnInit(): void {
    this.cargarMaterias();
  }

  // Consulta el catálogo de materias y la inscripción activa del estudiante.
  private cargarMaterias(): void {
    this.isLoading = true;

    forkJoin({
      materias: this.materiaService.getMaterias(),
      inscripcion: this.inscripcionService.getMiInscripcion(true).pipe(
        catchError(error => this.manejarErrorMiInscripcion(error))
      )
    }).subscribe({
      next: ({ materias, inscripcion }) => {
        this.materias = materias;
        this.miInscripcion = inscripcion;
        this.tieneInscripcionActiva = inscripcion !== null;
        this.isLoading = false;
      },
      error: () => {
        this.isLoading = false;
      }
    });
  }

  // Indica si la materia ya hace parte de la inscripción activa.
  materiaEstaInscrita(materia: MateriaResponse): boolean {
    return this.miInscripcion?.materias.some(inscrita => inscrita.materiaId === materia.materiaId) ?? false;
  }

  // Indica si la materia está seleccionada temporalmente.
  materiaEstaSeleccionada(materia: MateriaResponse): boolean {
    return this.materiasSeleccionadas.some(selected => selected.materiaId === materia.materiaId);
  }

  // Valida si ya fue elegido otro curso del mismo profesor.
  profesorYaSeleccionado(materia: MateriaResponse): boolean {
    return this.materiasSeleccionadas.some(selected =>
      selected.profesorId === materia.profesorId &&
      selected.materiaId !== materia.materiaId
    );
  }

  // Define si una materia puede agregarse a la selección actual.
  puedeSeleccionarMateria(materia: MateriaResponse): boolean {
    if (this.tieneInscripcionActiva) {
      return false;
    }

    if (this.materiaEstaSeleccionada(materia)) {
      return true;
    }

    if (this.cantidadSeleccionada >= 3) {
      return false;
    }

    return !this.profesorYaSeleccionado(materia);
  }

  // Agrega o quita una materia respetando las reglas de inscripción.
  alternarMateria(materia: MateriaResponse): void {
    if (this.tieneInscripcionActiva) {
      return;
    }

    if (this.materiaEstaSeleccionada(materia)) {
      this.quitarMateria(materia);
      return;
    }

    if (!this.puedeSeleccionarMateria(materia)) {
      return;
    }

    this.materiasSeleccionadas = [...this.materiasSeleccionadas, materia];
  }

  // Quita una materia de la selección temporal.
  quitarMateria(materia: MateriaResponse): void {
    this.materiasSeleccionadas = this.materiasSeleccionadas.filter(selected => selected.materiaId !== materia.materiaId);
  }

  // Crea la inscripción cuando hay exactamente tres materias válidas.
  confirmarInscripcion(): void {
    if (this.cantidadSeleccionada !== 3) {
      return;
    }

    const request: CrearInscripcionRequest = {
      materiasIds: this.materiasSeleccionadas.map(materia => materia.materiaId)
    };

    this.isLoading = true;

    this.inscripcionService.crearInscripcion(request).subscribe({
      next: inscripcion => {
        this.miInscripcion = inscripcion;
        this.tieneInscripcionActiva = true;
        this.materiasSeleccionadas = [];
        this.isLoading = false;
        void this.router.navigate(['/mi-inscripcion']);
      },
      error: () => {
        this.isLoading = false;
      }
    });
  }

  // Navega al detalle de la inscripción activa.
  goToMiInscripcion(): void {
    void this.router.navigate(['/mi-inscripcion']);
  }
  
  // Convierte el 404 de inscripción en estado vacío sin mostrar error global.
  private manejarErrorMiInscripcion(error: unknown) {
    if (error instanceof HttpErrorResponse && error.status === 404) {
      return of(null);
    }

    return throwError(() => error);
  }
}
