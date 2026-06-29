import { Component, OnInit } from '@angular/core';
import { PlantasService } from '../../services/plantas';
import { Planta } from '../../models/planta.model';

@Component({
  selector: 'app-crud-plantas',
  standalone: false,
  templateUrl: './crud-plantas.html',
  styleUrl: './crud-plantas.css',
})
export class CrudPlantas implements OnInit {
  plantas: Planta[] = [];
  mostrarFormulario = false;
  modoEdicion = false;
  idEditando: number | null = null;
  mensaje = '';

  form = {
    nombre: '',
    tipo: 'flor',
    precio: 0,
    stock: 0,
    descripcion: '',
    imagen: '🌱',
  };

  tiposEmoji: Record<string, string> = {
    flor: '🌸',
    suculenta: '🌵',
    arbusto: '🌿',
    arbol: '🌳',
  };

  constructor(private readonly plantasService: PlantasService) {}

  ngOnInit() {
    this.cargarPlantas();
  }

  cargarPlantas() {
    this.plantas = this.plantasService.getTodas();
  }

  abrirFormularioNuevo() {
    this.modoEdicion = false;
    this.idEditando = null;
    this.form = { nombre: '', tipo: 'flor', precio: 0, stock: 0, descripcion: '', imagen: '🌸' };
    this.mostrarFormulario = true;
    this.mensaje = '';
  }

  editar(planta: Planta) {
    this.modoEdicion = true;
    this.idEditando = planta.id;
    this.form = {
      nombre: planta.nombre,
      tipo: planta.tipo,
      precio: planta.precio,
      stock: planta.stock,
      descripcion: planta.descripcion,
      imagen: planta.imagen,
    };
    this.mostrarFormulario = true;
    this.mensaje = '';
  }

  onTipoChange() {
    this.form.imagen = this.tiposEmoji[this.form.tipo] || '🌱';
  }

  guardar() {
    if (!this.form.nombre.trim()) {
      this.mensaje = 'El nombre es obligatorio.';
      return;
    }
    if (this.modoEdicion && this.idEditando !== null) {
      this.plantasService.actualizar(this.idEditando, this.form);
      this.mensaje = '✅ Planta actualizada correctamente.';
    } else {
      this.plantasService.agregar(this.form);
      this.mensaje = '✅ Planta agregada correctamente.';
    }
    this.cargarPlantas();
    this.mostrarFormulario = false;
  }

  eliminar(id: number, nombre: string) {
    if (confirm(`¿Seguro que deseas eliminar "${nombre}"?`)) {
      this.plantasService.eliminar(id);
      this.cargarPlantas();
      this.mensaje = `🗑️ "${nombre}" eliminada.`;
    }
  }

  cancelar() {
    this.mostrarFormulario = false;
    this.mensaje = '';
  }
}
