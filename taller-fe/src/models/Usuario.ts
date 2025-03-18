export interface Usuario {
    id: number;
    nombre: string;
    email: string;
    passwordHash: string;
    rol: 'admin' | 'user';
    fechaRegistro: Date;
}