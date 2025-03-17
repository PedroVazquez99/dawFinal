<template>
    <div></div> <!-- No necesitamos contenido en el template porque solo usamos SweetAlert2 -->
  </template>
  
  <script lang="ts">
  import { Component, Prop, Vue } from "vue-property-decorator";
  import Swal from "sweetalert2";
  
  @Component
  export default class OkModal extends Vue {
    @Prop({ required: false }) position!: string; // Posición del modal (por ejemplo, 'top-end')
    @Prop({ required: true }) title!: string; // Título del mensaje
    @Prop({ default: false }) showConfirmButton!: boolean; // Mostrar o no botón de confirmación
    @Prop({ default: 1200 }) timer!: number; // Tiempo antes de cerrarse automáticamente
  
    mounted() {
      Swal.fire({
        position: 'center',
        icon: 'success',
        title: this.title,
        timer: this.timer,
      });
    }
  }
  </script>
  
  <style scoped>
  /* No estilos necesarios aquí */
  </style>
  