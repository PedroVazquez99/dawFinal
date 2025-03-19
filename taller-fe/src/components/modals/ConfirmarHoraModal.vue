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
        Swal.fire({
            title: 'Introduce los datos',
            html: `
                <input id="nombre" class="swal2-input" placeholder="Nombre">
                <input id="fecha" type="datetime-local" class="swal2-input">
            `,
            focusConfirm: false,
            showCancelButton: true,
            confirmButtonText: 'Guardar',
            cancelButtonText: 'Cancelar',
            preConfirm: () => {
                const nombre = (document.getElementById("nombre") as HTMLInputElement).value;
                const fecha = (document.getElementById("fecha") as HTMLInputElement).value;
                if (!nombre || !fecha) {
                    Swal.showValidationMessage("¡Todos los campos son obligatorios!");
                }
                return { nombre, fecha };
            }
        }).then((result) => {
            if (result.isConfirmed) {
                this.$emit('confirm', result.value);
            }
        });
    }
}
</script>

<style scoped>
/* Añade estilos si deseas incluir personalización extra */
</style>
