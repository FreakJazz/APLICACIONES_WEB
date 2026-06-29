export interface Planta {
  id: number;
  nombre: string;
  tipo: string; // 'flor' | 'arbusto' | 'arbol' | 'suculenta'
  precio: number;
  stock: number;
  descripcion: string;
  imagen: string;
}
