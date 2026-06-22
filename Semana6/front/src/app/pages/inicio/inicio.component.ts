import { CommonModule } from '@angular/common';
import { Component, OnDestroy, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { Subject } from 'rxjs';
import { takeUntil } from 'rxjs/operators';
import { AutenticacionServicio } from '../../servicios/autenticacion.servicio';

@Component({
  selector: 'app-inicio',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './inicio.component.html',
  styleUrl: './inicio.component.css'
})
export class InicioComponent implements OnInit, OnDestroy {
  
  // Nombre del usuario autenticado
  nombreUsuario = '';
  
  // Estado de carga
  cargandoLogout = false;
  
  // Para desuscribirse
  private destruido = new Subject<void>();

  constructor(
    private autenticacionServicio: AutenticacionServicio,
    private router: Router
  ) {}

  ngOnInit(): void {
    // Obtener el nombre del usuario actual
    this.nombreUsuario = this.autenticacionServicio.obtenerNombreUsuarioActual();
    
    // Suscribirse a cambios del nombre del usuario
    this.autenticacionServicio.nombreUsuario$
      .pipe(takeUntil(this.destruido))
      .subscribe(nombre => {
        this.nombreUsuario = nombre;
      });
  }

  ngOnDestroy(): void {
    // Limpiar suscripciones
    this.destruido.next();
    this.destruido.complete();
  }

  /**
   * Realiza el logout del usuario
   */
  cerrarSesion(): void {
    this.cargandoLogout = true;

    this.autenticacionServicio.logout().subscribe({
      next: () => {
        // Logout exitoso
        this.cargandoLogout = false;
        // Redirigir al login
        this.router.navigate(['/login']);
      },
      error: () => {
        // Si hay error, igual limpiar y redirigir
        this.cargandoLogout = false;
        localStorage.removeItem('nombreUsuario');
        this.router.navigate(['/login']);
      }
    });
  }
}
