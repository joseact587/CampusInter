import { CommonModule } from '@angular/common';
import { Component, OnInit, inject } from '@angular/core';
import { FormControl, ReactiveFormsModule } from '@angular/forms';
import { FontAwesomeModule } from '@fortawesome/angular-fontawesome';
import { faEnvelope, faIdCard, faSearch, faUsers } from '@fortawesome/free-solid-svg-icons';
import { EstudianteResumenResponse } from '../../models/estudiante.models';
import { EstudianteService } from '../../services/estudiante.service';

@Component({
  selector: 'app-estudiante-list',
  standalone: true,
  imports: [
    CommonModule,
    ReactiveFormsModule,
    FontAwesomeModule
  ],
  templateUrl: './estudiante-list.component.html',
  styleUrl: './estudiante-list.component.css'
})
export class EstudianteListComponent implements OnInit {
  //--Inyecciones
  private readonly estudianteService = inject(EstudianteService);

  //--Variables
  estudiantes: EstudianteResumenResponse[] = [];
  isLoading = false;

  readonly faSearch = faSearch;
  readonly faEnvelope = faEnvelope;
  readonly faIdCard = faIdCard;
  readonly faUsers = faUsers;

  //--Formularios
  // Control del buscador de estudiantes.
  searchControl = new FormControl('', { nonNullable: true });

  //--Métodos

  // Carga el listado al entrar en la pantalla.
  ngOnInit(): void {
    this.loadEstudiantes();
  }

  // Consulta los estudiantes registrados desde la API.
  loadEstudiantes(): void {
    this.isLoading = true;

    this.estudianteService.getEstudiantes().subscribe({
      next: estudiantes => {
        this.estudiantes = estudiantes;
        this.isLoading = false;
      },
      error: () => {
        this.isLoading = false;
      }
    });
  }

  // Filtra estudiantes por nombre, correo o documento.
  get filteredEstudiantes(): EstudianteResumenResponse[] {
    const criterio = this.searchControl.value.trim().toLowerCase();

    if (!criterio) {
      return this.estudiantes;
    }

    return this.estudiantes.filter(estudiante => {
      const nombreCompleto = this.getNombreCompleto(estudiante).toLowerCase();
      const correo = estudiante.correo?.toLowerCase() ?? '';
      const documento = estudiante.documento?.toLowerCase() ?? '';

      return nombreCompleto.includes(criterio) ||
        correo.includes(criterio) ||
        documento.includes(criterio);
    });
  }


  // Construye el nombre completo del estudiante.
  getNombreCompleto(estudiante: EstudianteResumenResponse): string {
    return [
      estudiante.primerNombre,
      estudiante.segundoNombre,
      estudiante.primerApellido,
      estudiante.segundoApellido
    ]
      .filter(value => !!value?.trim())
      .join(' ');
  }

  // Obtiene iniciales para el avatar del estudiante.
  getInitials(estudiante: EstudianteResumenResponse): string {
    const nombreCompleto = this.getNombreCompleto(estudiante);
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

  // Indica si el estudiante está activo.
  isActive(estudiante: EstudianteResumenResponse): boolean {
    return estudiante.estado.toLowerCase() === 'activo';
  }
}
