import { Injectable } from '@angular/core';
import { Planta } from '../models/planta.model';

@Injectable({
  providedIn: 'root'
})
export class PlantasService {

  private plantas: Planta[] = [
    { id: 1, nombre: 'Rosa Roja', tipo: 'flor', precio: 5.50, stock: 30, descripcion: 'Flor clásica de color rojo intenso, perfecta para regalos.', imagen: '🌹' },
    { id: 2, nombre: 'Tulipán Amarillo', tipo: 'flor', precio: 4.00, stock: 25, descripcion: 'Flor primaveral de color amarillo brillante.', imagen: '🌷' },
    { id: 3, nombre: 'Girasol', tipo: 'flor', precio: 3.50, stock: 40, descripcion: 'Planta alta con flor grande que sigue al sol.', imagen: '🌻' },
    { id: 4, nombre: 'Lirio Blanco', tipo: 'flor', precio: 6.00, stock: 20, descripcion: 'Elegante flor blanca con aroma suave.', imagen: '🌸' },
    { id: 5, nombre: 'Cactus Pequeño', tipo: 'suculenta', precio: 8.00, stock: 50, descripcion: 'Planta resistente, ideal para interiores.', imagen: '🌵' },
    { id: 6, nombre: 'Aloe Vera', tipo: 'suculenta', precio: 10.00, stock: 35, descripcion: 'Suculenta medicinal con propiedades curativas.', imagen: '🪴' },
    { id: 7, nombre: 'Rosal Trepador', tipo: 'arbusto', precio: 15.00, stock: 15, descripcion: 'Arbusto con rosas que trepa paredes y cercas.', imagen: '🌿' },
    { id: 8, nombre: 'Lavanda', tipo: 'arbusto', precio: 7.00, stock: 28, descripcion: 'Arbusto aromático de color púrpura.', imagen: '💜' },
    { id: 9, nombre: 'Palma Enana', tipo: 'arbol', precio: 25.00, stock: 10, descripcion: 'Palmera pequeña ideal para jardines y patios.', imagen: '🌴' },
    { id: 10, nombre: 'Naranjo Ornamental', tipo: 'arbol', precio: 30.00, stock: 8, descripcion: 'Árbol con naranjas decorativas y fragante.', imagen: '🍊' },
  ];

  private nextId = 11;

  getTodas(): Planta[] {
    return [...this.plantas];
  }

  getPorId(id: number): Planta | undefined {
    return this.plantas.find(p => p.id === id);
  }

  agregar(planta: Omit<Planta, 'id'>): Planta {
    const nueva: Planta = { ...planta, id: this.nextId++ };
    this.plantas.push(nueva);
    return nueva;
  }

  actualizar(id: number, datos: Omit<Planta, 'id'>): boolean {
    const index = this.plantas.findIndex(p => p.id === id);
    if (index === -1) return false;
    this.plantas[index] = { ...datos, id };
    return true;
  }

  eliminar(id: number): boolean {
    const index = this.plantas.findIndex(p => p.id === id);
    if (index === -1) return false;
    this.plantas.splice(index, 1);
    return true;
  }
}
