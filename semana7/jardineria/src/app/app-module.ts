import { NgModule, provideBrowserGlobalErrorListeners } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { FormsModule } from '@angular/forms';

import { AppRoutingModule } from './app-routing-module';
import { App } from './app';
import { Navbar } from './components/navbar/navbar';
import { Footer } from './components/footer/footer';
import { Inicio } from './pages/inicio/inicio';
import { Plantas } from './pages/plantas/plantas';
import { CrudPlantas } from './pages/crud-plantas/crud-plantas';

@NgModule({
  declarations: [App, Navbar, Footer, Inicio, Plantas, CrudPlantas],
  imports: [BrowserModule, AppRoutingModule, FormsModule],
  providers: [provideBrowserGlobalErrorListeners()],
  bootstrap: [App],
})
export class AppModule {}
