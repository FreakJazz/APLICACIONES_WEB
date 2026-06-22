import { Routes } from '@angular/router';
import { proteccionGuard } from './guards/proteccion.guard';
import { InicioComponent } from './pages/inicio/inicio.component';
import { LoginComponent } from './pages/login/login.component';

export const rutas: Routes = [
  {
    path: '',
    redirectTo: 'login',
    pathMatch: 'full'
  },
  {
    path: 'login',
    component: LoginComponent
  },
  {
    path: 'inicio',
    component: InicioComponent,
    canActivate: [proteccionGuard]
  },
  {
    path: '**',
    redirectTo: 'login'
  }
];
