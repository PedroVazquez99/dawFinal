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
  private calendar!: Calendar;

  mounted() {
    if (!this.$refs.calendar) return;

    this.calendar = new Calendar(this.$refs.calendar as HTMLElement, {
      plugins: [dayGridPlugin, timeGridPlugin, interactionPlugin],
      initialView: "dayGridMonth",
      selectable: true,
      events: [
        { title: "Reserva 1", start: "2025-03-16T10:00:00" },
        { title: "Reserva 2", start: "2025-03-17T12:00:00" },
      ],
      dateClick: this.handleDateClick,
    });

    this.calendar.render();
  }

  beforeDestroy() {
    if (this.calendar) {
      this.calendar.destroy();
    }
  }

  private handleDateClick(info: any): void {
    const eventTitle = prompt("Introduce el nombre de la reserva:");
    const eventTime = prompt("Introduce la hora en formato HH:mm (por ejemplo, 14:30):");

    if (eventTitle && eventTime) {
      const startDateTime = `${info.dateStr}T${eventTime}:00`;

      this.calendar.addEvent({
        title: eventTitle,
        start: startDateTime,
      });

      // Renderizar para asegurarnos de que el evento se muestre
      this.calendar.render();
    } else {
      alert("Nombre y hora son requeridos para crear la reserva.");
    }
  }
}
</script>

<style scoped>
/* Añade estilos si es necesario */
</style>
