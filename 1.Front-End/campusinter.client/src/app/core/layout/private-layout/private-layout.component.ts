import { CommonModule } from '@angular/common';
import { Component, computed, inject } from '@angular/core';
import { Router, RouterLink, RouterLinkActive, RouterOutlet } from '@angular/router';
import { FontAwesomeModule } from '@fortawesome/angular-fontawesome';
import {
  faBookBookmark,
  faBookOpen,
  faGraduationCap,
  faHouse,
  faRightFromBracket,
  faUser,
  faUsers
} from '@fortawesome/free-solid-svg-icons';
import { AuthService } from '../../auth/services/auth.service';
import { CurrentUserStore } from '../../auth/services/current-user.store';

@Component({
  selector: 'app-private-layout',
  standalone: true,
  imports: [
    CommonModule,
    RouterOutlet,
    RouterLink,
    RouterLinkActive,
    FontAwesomeModule
  ],
  templateUrl: './private-layout.component.html',
  styleUrl: './private-layout.component.css'
})
export class PrivateLayoutComponent {
  //--Inyecciones
  private readonly authService = inject(AuthService);
  private readonly currentUserStore = inject(CurrentUserStore);
  private readonly router = inject(Router);

  //--Variables
  readonly currentUser = this.currentUserStore.currentUser;

  readonly estudianteActivo = computed(() => {
    const estado = this.currentUser()?.estado ?? '';

    return estado.toLowerCase() === 'activo';
  });

  readonly initials = computed(() => {
    const displayName = this.currentUser()?.displayName ?? '';
    const words = displayName
      .split(' ')
      .map(word => word.trim())
      .filter(word => word.length > 0);

    if (words.length === 0) {
      return 'CI';
    }

    return words
      .slice(0, 2)
      .map(word => word[0].toUpperCase())
      .join('');
  });

  readonly faGraduationCap = faGraduationCap;
  readonly faHouse = faHouse;
  readonly faBookOpen = faBookOpen;
  readonly faBookBookmark = faBookBookmark;
  readonly faUsers = faUsers;
  readonly faUser = faUser;
  readonly faRightFromBracket = faRightFromBracket;

  //--Métodos
  // Cierra la sesión actual y redirige al login.
  logout(): void {
    this.authService.cerrarSesion();
    void this.router.navigate(['/login']);
  }
}
