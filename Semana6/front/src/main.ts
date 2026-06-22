import { bootstrapApplication } from '@angular/platform-browser';
import { provideHttpClient, withInterceptors } from '@angular/platform-browser/http';
import { provideRouter } from '@angular/router';
import { AppComponent } from './app/app.component';
import { rutas } from './app/app.routes';
import { autenticacionInterceptor } from './app/interceptores/autenticacion.interceptor';

bootstrapApplication(AppComponent, {
  providers: [
    provideRouter(rutas),
    provideHttpClient(
      withInterceptors([autenticacionInterceptor])
    )
  ]
}).catch(err => console.error(err));
