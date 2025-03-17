<template>
    <!-- Este componente no requiere plantilla porque maneja todo con SweetAlert2 -->
    <div></div>
  </template>
  
  <script lang="ts">
  import { Component, Prop, Vue } from "vue-property-decorator";
  import Swal from "sweetalert2";
  
  @Component
  export default class ConfirmarHoraModal extends Vue {
    @Prop({ default: () => ({}) }) config!: object; // Configuración dinámica del modal
  
    mounted() {
      // Mostrar el modal cuando se monte el componente
      Swal.fire(this.config).then((result) => {
        if (result.isConfirmed) {
          this.$emit("confirm"); // Emitir evento si el usuario confirma
        } else if (result.dismiss === Swal.DismissReason.cancel) {
          this.$emit("cancel"); // Emitir evento si el usuario cancela
        } else {
          this.$emit("close"); // Emitir evento si simplemente cierra
        }
      });
    }
  }
  </script>
  
  <style scoped>
  /* Añade estilos si deseas incluir personalización extra */
  </style>
  