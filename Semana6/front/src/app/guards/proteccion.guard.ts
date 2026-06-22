import { CanActivateFn, Router } from '@angular/router';
import { AutenticacionServicio } from '../servicios/autenticacion.servicio';

/**
 * Guard que protege las rutas requireniendo autenticación
 * Si el usuario no está autenticado, redirige al login
 */
export const proteccionGuard: CanActivateFn = (route, state) => {
  
  // Inyectar servicios manualmente
  const router = new Router();
  const autenticacionServicio = new AutenticacionServicio(null as any);
  
  // Verificar si el usuario está autenticado
  if (autenticacionServicio.estaAutenticado()) {
    return true;
  } else {
    // Redirigir al login si no está autenticado
    router.navigate(['/login']);
    return false;
  }
};
