import { Component, OnInit } from '@angular/core';
import { PlantasService } from '../../services/plantas';
import { Planta } from '../../models/planta.model';
import jsPDF from 'jspdf';
import autoTable from 'jspdf-autotable';

@Component({
  selector: 'app-plantas',
  standalone: false,
  templateUrl: './plantas.html',
  styleUrl: './plantas.css',
})
export class Plantas implements OnInit {
  plantas: Planta[] = [];
  filtro: string = '';
  tipoSeleccionado: string = '';

  constructor(private plantasService: PlantasService) {}

  ngOnInit() {
    this.plantas = this.plantasService.getTodas();
  }

  get plantasFiltradas(): Planta[] {
    return this.plantas.filter(p => {
      const coincideNombre = p.nombre.toLowerCase().includes(this.filtro.toLowerCase());
      const coincideTipo = this.tipoSeleccionado ? p.tipo === this.tipoSeleccionado : true;
      return coincideNombre && coincideTipo;
    });
  }

  exportarPDF() {
    const doc = new jsPDF();

    doc.setFontSize(18);
    doc.setTextColor(45, 90, 39);
    doc.text('Jardineria Verde - Catalogo de Plantas', 14, 20);

    doc.setFontSize(11);
    doc.setTextColor(100, 100, 100);
    doc.text(`Fecha: ${new Date().toLocaleDateString('es-ES')}`, 14, 28);
    doc.text(`Total de plantas: ${this.plantasFiltradas.length}`, 14, 34);

    autoTable(doc, {
      startY: 42,
      head: [['#', 'Nombre', 'Tipo', 'Precio ($)', 'Stock', 'Descripcion']],
      body: this.plantasFiltradas.map((p, i) => [
        i + 1,
        p.nombre,
        p.tipo.charAt(0).toUpperCase() + p.tipo.slice(1),
        p.precio.toFixed(2),
        p.stock,
        p.descripcion,
      ]),
      headStyles: { fillColor: [45, 90, 39], textColor: 255 },
      alternateRowStyles: { fillColor: [240, 247, 240] },
      styles: { fontSize: 9 },
    });

    doc.save('catalogo-plantas.pdf');
  }
}
