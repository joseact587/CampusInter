import { CommonModule } from '@angular/common';
import { HttpErrorResponse } from '@angular/common/http';
import { Component, OnInit, computed, inject, signal } from '@angular/core';
import { Router } from '@angular/router';
import { FontAwesomeModule } from '@fortawesome/angular-fontawesome';
import {
  faAward,
  faBookOpen,
  faCircleCheck,
  faCircleExclamation,
  faGraduationCap,
  faUser,
  faUsers
} from '@fortawesome/free-solid-svg-icons';
import { catchError, forkJoin, of, switchMap, throwError } from 'rxjs';
import { CurrentUserStore } from '../../../../core/auth/services/current-user.store';
import { EstudianteService } from '../../../estudiantes/services/estudiante.service';
import { InscripcionResponse, MateriaCompanerosResponse } from '../../../inscripciones/models/inscripcion.models';
import { InscripcionService } from '../../../inscripciones/services/inscripcion.service';

@Component({
  selector: 'app-home',
  standalone: true,
  imports: [
    CommonModule,
    FontAwesomeModule
  ],
  templateUrl: './home.component.html',
  styleUrl: './home.component.css'
})
export class HomeComponent implements OnInit {
  //--Inyecciones
  private readonly currentUserStore = inject(CurrentUserStore);
  private readonly router = inject(Router);
  private readonly inscripcionService = inject(InscripcionService);
  private readonly estudianteService = inject(EstudianteService);

  //--Variables
  readonly currentUser = this.currentUserStore.currentUser;

  readonly inscripcion = signal<InscripcionResponse | null>(null);
  readonly companerosPorMateria = signal<MateriaCompanerosResponse[]>([]);
  readonly estudiantesActivos = signal(0);
  readonly cargando = signal(true);

  readonly faBookOpen = faBookOpen;
  readonly faAward = faAward;
  readonly faUsers = faUsers;
  readonly faGraduationCap = faGraduationCap;
  readonly faCircleCheck = faCircleCheck;
  readonly faCircleExclamation = faCircleExclamation;
  readonly faUser = faUser;

  readonly primerNombre = computed(() => {
    const displayName = this.currentUser()?.displayName?.trim();

    return displayName
      ? displayName.split(' ')[0]
      : 'Estudiante';
  });

  readonly tieneInscripcionActiva = computed(() => this.inscripcion() !== null);
  readonly materiasInscritas = computed(() => this.inscripcion()?.materias.length ?? 0);
  readonly creditosTotales = computed(() => this.inscripcion()?.totalCreditos ?? 0);
  readonly companerosTotal = computed(() => {
    const nombresUnicos = new Set<string>();

    this.companerosPorMateria().forEach(materia => {
      materia.companeros.forEach(companero => nombresUnicos.add(companero.nombreCompleto));
    });

    return nombresUnicos.size;
  });

  //--Métodos
  ngOnInit(): void {
    this.cargarPanelPrincipal();
  }

  // Consulta inscripción, compañeros y estudiantes activos para armar el resumen.
  private cargarPanelPrincipal(): void {
    const miInscripcion$ = this.inscripcionService.getMiInscripcion(true).pipe(
      catchError(error => this.manejarErrorMiInscripcion(error))
    );

    forkJoin({
      inscripcion: miInscripcion$,
      estudiantes: this.estudianteService.getEstudiantes()
    })
      .pipe(
        switchMap(({ inscripcion, estudiantes }) => {
          this.inscripcion.set(inscripcion);
          this.estudiantesActivos.set(estudiantes.length);

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
          this.companerosPorMateria.set(companerosPorMateria);
          this.cargando.set(false);
        },
        error: () => {
          this.cargando.set(false);
        }
      });
  }
  // Navega a la pantalla de inscripción de materias.
  goToMaterias(): void {
    void this.router.navigate(['/materias']);
  }

  // Navega al detalle de la inscripción activa.
  goToMiInscripcion(): void {
    void this.router.navigate(['/mi-inscripcion']);
  }

  // Navega al listado de estudiantes.
  goToEstudiantes(): void {
    void this.router.navigate(['/estudiantes']);
  }

  // Navega al perfil del estudiante autenticado.
  goToMiPerfil(): void {
    void this.router.navigate(['/mi-perfil']);
  }

  // Formatea la fecha de inscripción para mostrarla en español.
  formatFecha(fecha: string): string {
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
