import { HttpErrorResponse, HttpInterceptorFn } from '@angular/common/http';
import { inject } from '@angular/core';
import { Router } from '@angular/router';
import { throwError } from 'rxjs';
import { catchError } from 'rxjs/operators';

/**
 * Interceptor que maneja las peticiones HTTP
 * - Agrega credenciales para cookies
 * - Maneja errores de autenticación (401, 403)
 * - Redirige al login si la sesión expiró
 */
export const autenticacionInterceptor: HttpInterceptorFn = (
  peticion,
  siguiente
) => {
  
  const router = inject(Router);
  
  // Agregar credenciales a todas las peticiones
  const peticionConCredenciales = peticion.clone({
    withCredentials: true
  });

  return siguiente(peticionConCredenciales).pipe(
    catchError((error: HttpErrorResponse) => {
      
      // Manejar errores de autenticación
      if (error.status === 401 || error.status === 403) {
        // Sesión expirada o no autorizado
        localStorage.removeItem('nombreUsuario');
        router.navigate(['/login']);
      }
      
      // Propagar el error
      return throwError(() => error);
    })
  );
};
