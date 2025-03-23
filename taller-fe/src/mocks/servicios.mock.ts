export interface IServicioPeluqueria {
    id: number;
    nombre: string;
    precio: number; // Precio en dólares o la moneda local
    duracion: number; // Duración en minutos
  }
  
  export const serviciosPeluqueriaMock: IServicioPeluqueria[] = [
    { id: 1, nombre: "Corte de cabello", precio: 15, duracion: 30 },
    { id: 2, nombre: "Tinte", precio: 40, duracion: 90 },
    { id: 3, nombre: "Peinado", precio: 25, duracion: 45 },
    { id: 4, nombre: "Alisado", precio: 60, duracion: 120 }
  ];