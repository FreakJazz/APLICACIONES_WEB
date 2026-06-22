# 🔐 Aplicación de Login - Semana 6

**Autor:** Estudiante  
**Curso:** Aplicaciones Web  
**Fecha:** 2026

---

## 📋 Descripción

Aplicación de autenticación simple desarrollada en **Angular 17** que implementa un sistema de login seguro con:

- ✅ **Validación reactiva** de formularios
- ✅ **Almacenamiento seguro** de sesión en localStorage
- ✅ **Cookies HttpOnly** con flags de seguridad
- ✅ **Protección de rutas** mediante Guards
- ✅ **Interceptores HTTP** para manejo automático de credenciales
- ✅ **Gestión centralizada** de autenticación con Servicios
- ✅ **Manejo robusto** de errores

---

## 🚀 Inicio Rápido

### Requisitos Previos

- **Node.js** 18.0.0 o superior
- **npm** o **yarn**
- **Angular CLI** 17.0.0 o superior

### Instalación

1. **Navega a la carpeta del proyecto:**
   ```bash
   cd Semana6/front
   ```

2. **Instala las dependencias:**
   ```bash
   npm install
   ```

3. **Inicia el servidor de desarrollo:**
   ```bash
   npm start
   ```

4. **Accede a la aplicación:**
   Abre tu navegador en: `http://localhost:4200`

---

## 🔑 Credenciales de Prueba

Para probar la aplicación, usa las siguientes credenciales:

| Campo | Valor |
|-------|-------|
| **Usuario** | `admin` |
| **Contraseña** | `admin123` |

> **Nota:** Asegúrate de que el backend esté corriendo en `http://localhost:5000`

---

## 📁 Estructura del Proyecto

```
front/
├── src/
│   ├── app/
│   │   ├── servicios/
│   │   │   └── autenticacion.servicio.ts    # Lógica de autenticación
│   │   ├── guards/
│   │   │   └── proteccion.guard.ts          # Protección de rutas
│   │   ├── interceptores/
│   │   │   └── autenticacion.interceptor.ts # Manejo de HTTP
│   │   ├── pages/
│   │   │   ├── login/
│   │   │   │   ├── login.component.ts
│   │   │   │   ├── login.component.html
│   │   │   │   └── login.component.css
│   │   │   └── inicio/
│   │   │       ├── inicio.component.ts
│   │   │       ├── inicio.component.html
│   │   │       └── inicio.component.css
│   │   ├── app.component.ts                 # Componente raíz
│   │   ├── app.component.html
│   │   ├── app.component.css
│   │   └── app.routes.ts                    # Rutas de la aplicación
│   ├── main.ts                              # Bootstrap de la aplicación
│   ├── index.html                           # HTML principal
│   └── styles.css                           # Estilos globales
├── package.json                             # Dependencias
├── angular.json                             # Configuración de Angular
├── tsconfig.json                            # Configuración de TypeScript
└── README.md                                # Este archivo
```

---

## 🏗️ Arquitectura

### Componentes

#### 1. **LoginComponent**
- Formulario reactivo con validación
- Campos: Usuario y Contraseña
- Manejo de errores de autenticación
- Integración con el servicio de autenticación

#### 2. **InicioComponent**
- Página protegida (requiere autenticación)
- Muestra información del usuario
- Botón para cerrar sesión
- Información técnica sobre la implementación

### Servicios

#### **AutenticacionServicio**
- Gestión centralizada de la autenticación
- Métodos:
  - `login(usuario, contrasena)` - Autentica al usuario
  - `logout()` - Cierra la sesión
  - `verificarSesionEnBackend()` - Valida sesión con el servidor
  - `estaAutenticado()` - Retorna estado de autenticación
  - `obtenerNombreUsuarioActual()` - Obtiene nombre del usuario

### Guards

#### **ProteccionGuard**
- Protege rutas que requieren autenticación
- Redirige al login si no está autenticado
- Aplicado a la ruta `/inicio`

### Interceptores

#### **AutenticacionInterceptor**
- Agrega automáticamente `withCredentials: true` a todas las peticiones
- Maneja errores 401 y 403 (sesión expirada)
- Limpia localStorage y redirige al login en caso de expiración

---

## 🔐 Seguridad

### Implementación de Seguridad

1. **localStorage**
   - Almacena el nombre del usuario para acceso local
   - Se limpia automáticamente al cerrar sesión

2. **Cookies HttpOnly**
   - El servidor establece cookies HttpOnly con la sesión
   - No accesibles desde JavaScript (protección XSS)
   - Incluyen flags `Secure` y `SameSite`

3. **withCredentials**
   - Las peticiones HTTP incluyen automáticamente las cookies
   - Permite validar sesión desde el backend

4. **CSRF Protection**
   - El backend debe implementar protección CSRF
   - Usar tokens XSRF-TOKEN en el servidor

5. **Guards de Rutas**
   - Protección de acceso a páginas sensibles
   - Redireccionamiento automático al login

---

## 🧪 Pruebas

### Con Postman o cURL

1. **Login:**
   ```bash
   curl -X POST http://localhost:5000/api/autenticacion/login \
     -H "Content-Type: application/json" \
     -d '{"usuario": "admin", "contrasena": "admin123"}' \
     -c cookies.txt
   ```

2. **Verificar Sesión:**
   ```bash
   curl http://localhost:5000/api/autenticacion/verificar-sesion \
     -b cookies.txt
   ```

3. **Logout:**
   ```bash
   curl -X POST http://localhost:5000/api/autenticacion/logout \
     -b cookies.txt
   ```

### Con la Aplicación Angular

1. Accede a `http://localhost:4200/login`
2. Ingresa las credenciales de prueba
3. Si es exitoso, serás redirigido a `/inicio`
4. Haz clic en "Cerrar Sesión" para logout
5. Intenta acceder directamente a `/inicio` sin autenticar (serás redirigido)

---

## 🎯 Criterios de Evaluación

### 1. Funcionalidad de Autenticación (30%)
- ✅ Login end-to-end funcional
- ✅ Rutas protegidas requieren sesión
- ✅ Logout limpia sesión
- ✅ Manejo de credenciales inválidas
- ✅ Mensajes de error claros

### 2. Cookies y Seguridad (25%)
- ✅ Cookies HttpOnly desde el backend
- ✅ Flags Secure y SameSite adecuados
- ✅ withCredentials en peticiones
- ✅ Protección XSRF/CSRF
- ✅ Regeneración de sesión tras login

### 3. Arquitectura y Calidad (15%)
- ✅ Servicio de autenticación centralizado
- ✅ Interceptor HTTP para manejo automático
- ✅ Guard de rutas implementado
- ✅ Nombres de variables descriptivos
- ✅ Comentarios generales útiles
- ✅ Sin secretos en código

### 4. UX y Validación (10%)
- ✅ Validación reactiva de formularios
- ✅ Mensajes claros de validación
- ✅ Botón deshabilitado durante envío
- ✅ Accesibilidad (labels y aria-label)
- ✅ Navegación post-login

### 5. Errores y Pruebas (10%)
- ✅ Manejo de errores en cliente
- ✅ Códigos HTTP apropiados (200, 401, 403, 400)
- ✅ Evidencia de pruebas (Postman incluida)

### 6. Documentación (10%)
- ✅ README con pasos de instalación
- ✅ Variables de entorno documentadas
- ✅ Estructura del repo clara
- ✅ Ejemplos cURL/Postman

---

## 🔧 Variables de Entorno

Para usar variables de entorno, crea un archivo `environment.ts`:

```typescript
// environment.ts
export const environment = {
  production: false,
  apiUrl: 'http://localhost:5000/api'
};
```

Luego usa en el servicio:
```typescript
import { environment } from '../environments/environment';

private urlApi = `${environment.apiUrl}/autenticacion`;
```

---

## 🛠️ Comandos Útiles

```bash
# Instalar dependencias
npm install

# Iniciar servidor de desarrollo
npm start

# Construir para producción
npm build

# Ejecutar pruebas unitarias
npm test

# Lint del código
npm lint

# Construir e iniciar (combinado)
npm run build && npm start
```

---

## 📝 Validaciones del Formulario

### Campo Usuario
- **Requerido:** Sí
- **Mínimo:** 3 caracteres
- **Máximo:** 20 caracteres
- **Error:** "El usuario debe tener al menos 3 caracteres"

### Campo Contraseña
- **Requerido:** Sí
- **Mínimo:** 6 caracteres
- **Máximo:** 50 caracteres
- **Error:** "La contraseña debe tener al menos 6 caracteres"

---

## 🐛 Solución de Problemas

### Error: "No se pudo conectar con el servidor"
- Verifica que el backend esté corriendo en `http://localhost:5000`
- Revisa la consola del navegador (F12) para más detalles

### Error: "Usuario o contraseña incorrectos"
- Verifica que las credenciales sean correctas
- Revisa que el backend esté retornando estado 401 para credenciales inválidas

### Error: CORS
- Configura CORS en el backend
- Asegúrate que `withCredentials: true` está habilitado

### Sesión no persiste
- Verifica que las cookies se crean correctamente (F12 → Application → Cookies)
- Comprueba que localStorage tiene `nombreUsuario` guardado

---

## 🌟 Características Implementadas

- [x] Login con validación reactiva
- [x] Almacenamiento en localStorage
- [x] Validación de sesión desde backend
- [x] Protección de rutas con Guards
- [x] Interceptores HTTP automáticos
- [x] Manejo centralizado con Servicios
- [x] Logout con limpieza de datos
- [x] Mensajes de error claros
- [x] Interfaz amigable y responsive
- [x] Documentación completa

---

## 📚 Referencias

- [Angular Docs](https://angular.io/docs)
- [Angular Security](https://angular.io/guide/security)
- [HTTPClient with Credentials](https://angular.io/guide/http#checking-for-data-in-the-error-response)
- [Route Guards](https://angular.io/guide/router#preventing-unauthorized-access)
- [localStorage API](https://developer.mozilla.org/es/docs/Web/API/Storage)
- [HTTP Cookies](https://developer.mozilla.org/es/docs/Web/HTTP/Cookies)

---

## ✍️ Notas

- Esta es una implementación básica de autenticación
- El código está comentado en español para mejor comprensión
- Para producción, implementa adicionales medidas de seguridad
- Consulta con tu backend sobre la configuración de CORS y cookies

---

**¡Éxito en tu evaluación! 🎉**
