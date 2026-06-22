import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { BehaviorSubject, Observable } from 'rxjs';
import { tap } from 'rxjs/operators';

// Interfaz para los datos de login
interface DatosLogin {
  usuario: string;
  contrasena: string;
}

// Interfaz para la respuesta del servidor
interface RespuestaAutenticacion {
  mensaje: string;
  usuario: string;
  token?: string;
}

@Injectable({
  providedIn: 'root'
})
export class AutenticacionServicio {
  
  // URL del backend (cambiar según sea necesario)
  private urlApi = 'http://localhost:5000/api/autenticacion';
  
  // Estado observable del usuario autenticado
  private usuarioAutenticado = new BehaviorSubject<boolean>(this.verificarSesion());
  public usuarioAutenticado$ = this.usuarioAutenticado.asObservable();
  
  // Observable del nombre del usuario
  private nombreUsuario = new BehaviorSubject<string>(this.obtenerNombreUsuario());
  public nombreUsuario$ = this.nombreUsuario.asObservable();

  constructor(private http: HttpClient) {}

  /**
   * Realiza el login del usuario
   * @param usuario - Nombre de usuario
   * @param contrasena - Contraseña del usuario
   */
  login(usuario: string, contrasena: string): Observable<RespuestaAutenticacion> {
    const datosLogin: DatosLogin = { usuario, contrasena };
    
    return this.http.post<RespuestaAutenticacion>(
      `${this.urlApi}/login`,
      datosLogin,
      { withCredentials: true } // Enviar cookies con la petición
    ).pipe(
      tap((respuesta) => {
        // Almacenar nombre de usuario en localStorage
        localStorage.setItem('nombreUsuario', respuesta.usuario);
        
        // Actualizar estados observables
        this.usuarioAutenticado.next(true);
        this.nombreUsuario.next(respuesta.usuario);
      })
    );
  }

  /**
   * Realiza el logout del usuario
   */
  logout(): Observable<any> {
    return this.http.post(
      `${this.urlApi}/logout`,
      {},
      { withCredentials: true }
    ).pipe(
      tap(() => {
        // Limpiar localStorage
        localStorage.removeItem('nombreUsuario');
        
        // Actualizar estados
        this.usuarioAutenticado.next(false);
        this.nombreUsuario.next('');
      })
    );
  }

  /**
   * Verifica si la sesión está activa con el backend
   */
  verificarSesionEnBackend(): Observable<any> {
    return this.http.get(
      `${this.urlApi}/verificar-sesion`,
      { withCredentials: true }
    );
  }

  /**
   * Verifica si hay una sesión activa localmente
   */
  private verificarSesion(): boolean {
    return !!localStorage.getItem('nombreUsuario');
  }

  /**
   * Obtiene el nombre del usuario almacenado
   */
  private obtenerNombreUsuario(): string {
    return localStorage.getItem('nombreUsuario') || '';
  }

  /**
   * Retorna si el usuario está autenticado
   */
  estaAutenticado(): boolean {
    return this.usuarioAutenticado.value;
  }

  /**
   * Obtiene el nombre del usuario actual
   */
  obtenerNombreUsuarioActual(): string {
    return this.nombreUsuario.value;
  }
}
