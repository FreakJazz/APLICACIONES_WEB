import { CommonModule } from '@angular/common';
import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { AutenticacionServicio } from '../../servicios/autenticacion.servicio';

@Component({
  selector: 'app-login',
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule],
  templateUrl: './login.component.html',
  styleUrl: './login.component.css'
})
export class LoginComponent implements OnInit {
  
  // Formulario reactivo
  formularioLogin: FormGroup;
  
  // Estado de carga
  cargando = false;
  
  // Mensaje de error
  mensajeError = '';

  constructor(
    private formBuilder: FormBuilder,
    private autenticacionServicio: AutenticacionServicio,
    private router: Router
  ) {
    // Crear el formulario con validaciones
    this.formularioLogin = this.formBuilder.group({
      usuario: [
        '',
        [
          Validators.required,
          Validators.minLength(3),
          Validators.maxLength(20)
        ]
      ],
      contrasena: [
        '',
        [
          Validators.required,
          Validators.minLength(6),
          Validators.maxLength(50)
        ]
      ]
    });
  }

  ngOnInit(): void {
    // Limpieza inicial
    this.mensajeError = '';
  }

  /**
   * Obtiene el control del campo usuario
   */
  get usuario() {
    return this.formularioLogin.get('usuario');
  }

  /**
   * Obtiene el control del campo contraseña
   */
  get contrasena() {
    return this.formularioLogin.get('contrasena');
  }

  /**
   * Verifica si un campo tiene error de validación
   */
  tieneError(nombreCampo: string, tipoError: string): boolean {
    const campo = this.formularioLogin.get(nombreCampo);
    return !!(campo && campo.hasError(tipoError) && campo.touched);
  }

  /**
   * Realiza el envío del formulario de login
   */
  enviarLogin(): void {
    
    // Limpiar mensaje de error previo
    this.mensajeError = '';
    
    // Validar que el formulario sea válido
    if (this.formularioLogin.invalid) {
      this.mensajeError = 'Por favor completa todos los campos correctamente';
      return;
    }

    // Extraer valores del formulario
    const { usuario, contrasena } = this.formularioLogin.value;
    
    // Activar estado de carga
    this.cargando = true;

    // Llamar al servicio de autenticación
    this.autenticacionServicio.login(usuario, contrasena).subscribe({
      next: (respuesta) => {
        // Login exitoso
        this.cargando = false;
        // Redirigir a la página de inicio
        this.router.navigate(['/inicio']);
      },
      error: (error) => {
        // Error en el login
        this.cargando = false;
        
        // Mostrar mensaje de error apropiado
        if (error.status === 401) {
          this.mensajeError = 'Usuario o contraseña incorrectos';
        } else if (error.status === 0) {
          this.mensajeError = 'No se pudo conectar con el servidor. Verifica que esté en línea.';
        } else {
          this.mensajeError = error.error?.mensaje || 'Error al intentar iniciar sesión';
        }
      }
    });
  }
}
