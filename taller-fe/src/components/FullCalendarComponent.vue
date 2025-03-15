<template>
  <div>
    <div ref="calendar"></div>
  </div>
</template>

<script lang="ts">
import { Component, Vue } from "vue-property-decorator";
import { Calendar } from "@fullcalendar/core";
import dayGridPlugin from "@fullcalendar/daygrid";
import timeGridPlugin from "@fullcalendar/timegrid";
import interactionPlugin from "@fullcalendar/interaction";

@Component
export default class FullCalendarComponent extends Vue {
  private calendar!: Calendar; // Instancia de FullCalendar

  mounted() {
    if (!this.$refs.calendar) return;

    this.calendar = new Calendar(this.$refs.calendar as HTMLElement, {
      plugins: [dayGridPlugin, timeGridPlugin, interactionPlugin], // Plugins de FullCalendar
      locale: "es", // Idioma
      initialView: "dayGridMonth", // Vista inicial
      editable: true, // Hace que los eventos sean editables (drag and drop)
      selectable: true, // Permite seleccionar eventos
      firstDay: 1, // Lunes como primer día de la semana
      events: [
        { title: "Reserva 1", start: "2025-03-16T10:00:00" },
        { title: "Reserva 2", start: "2025-03-17T12:00:00" },
      ], // Eventos iniciales, se cargarán desde la API, es decir, desde la base de datos
      dateClick: this.handleDateClick, // Maneja cuando se hace clic en una fecha
      eventDrop: this.handleEventDrop, // Maneja cuando un evento es arrastrado
    });

    this.calendar.render(); // Renderiza el calendario, debe llamarse al final
  }

  // Para liberar memoria
  beforeDestroy() {
    if (this.calendar) {
      this.calendar.destroy();
    }
  }

  // Cuando hago click en el día.
  private handleDateClick(info: any): void {
    const eventoTitulo = prompt("Introduce el nombre de la reserva:"); // Cambiar con Swal
    const eventoFecha = prompt("Introduce la hora en formato HH:mm (por ejemplo, 14:30):"); // Cambiar con Swal

    if (eventoTitulo && eventoFecha) {
      const fechaCompleta = `${info.dateStr}T${eventoFecha}:00`;

      this.calendar.addEvent({
        title: eventoTitulo,
        start: fechaCompleta,
      });

      // Renderizar para asegurarnos de que el evento se muestre
      this.calendar.render();
    } else {
      alert("Nombre y hora son requeridos para crear la reserva."); // Cambiar con Swal
    }
  }

  // Cuando se arrastra un evento
  private handleEventDrop(info: any): void {
    // Actualiza las fechas después de que se arrastre el evento
    const { event } = info;
    alert(`Evento "${event.title}" movido a ${event.start?.toISOString()}`);
  }
}
</script>

<style scoped>
/* Añade estilos si es necesario */
</style>
