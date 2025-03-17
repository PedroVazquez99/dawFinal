<template>
  <div>
    <div ref="calendar"></div>
  </div>
</template>

<script lang="ts">
import { Component, Prop, Vue } from "vue-property-decorator";
import { Calendar, EventSourceInput } from "@fullcalendar/core";
import dayGridPlugin from "@fullcalendar/daygrid";
import timeGridPlugin from "@fullcalendar/timegrid";
import interactionPlugin from "@fullcalendar/interaction";
import TaskList from "@/models/TaskList";

@Component
export default class FullCalendarComponent extends Vue {
  
 

    // Variables
    @Prop() 
      private titulo!: string;
      private calendar!: Calendar; // Instancia de FullCalendar
      mode = "list";
      newList: TaskList = new TaskList();
      errList = { nombre: false };
      slctVisible = "T";
      orden = "N";
      showModal = false;
      errGet = "";
      errAdd = "";
      editList: TaskList = new TaskList();

  mounted() {
    if (!this.$refs.calendar) return; // Devuelve undefined si no hay referencia al calendario
    
    this.$store.dispatch("getLists");
    this.errGet = "";
    if (this.hayError()) {
      this.errGet = this.getError();
    }

    const today = new Date().toISOString().split("T")[0]; // Obtener la fecha actual en formato YYYY-MM-DD

    this.calendar = new Calendar(this.$refs.calendar as HTMLElement, {
      plugins: [dayGridPlugin, timeGridPlugin, interactionPlugin], // Plugins de FullCalendar
      locale: "es", // Idioma
      initialView: "dayGridMonth", // Vista inicial
      editable: true, // Hace que los eventos sean editables (drag and drop)
      selectable: true, // Permite seleccionar eventos
      firstDay: 1, // Lunes como primer día de la semana
      headerToolbar: {
        left: "dayGridMonth,timeGridWeek,timeGridDay",
        center: "title",
        right: "prev,next today",
      },
      buttonText: {
        today: "Hoy", // Cambia el texto del botón "Today"
        month: "Mes",
        week: "Semana",
        day: "Día",
      },
      validRange: {
        start: today, // Restringe los días anteriores a hoy
      },
      eventTimeFormat: { // Personalización del formato de la hora
        hour: '2-digit',
        minute: '2-digit',
        meridiem: false, // Omite AM/PM, usa formato 24 horas
      },
      events: this.convertirAEventSource(this.getAll()), // Eventos iniciales, se cargarán desde la API, es decir, desde la base de datos
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
    const hoy = new Date();
    const fechaSeleccionada = new Date(info.dateStr);

    // Validar que no sea una hora posterior al momento actual en la fecha actual
    if (fechaSeleccionada < hoy) {
      alert("No puedes seleccionar fechas u horas anteriores a ahora."); // Validación
      return;
    }

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
      alert("Nombre y hora son requeridos para crear la reserva."); // Validación
    }
  }

  // Cuando se arrastra un evento
  private handleEventDrop(info: any): void {
    const { event } = info;

    // Validar si la nueva fecha es posterior al día actual
    const hoy = new Date();
    if (event.start && event.start < hoy) {
      alert("No puedes mover un evento a una fecha pasada.");
      info.revert(); // Revertir el cambio
      return;
    }

    alert(`Evento "${event.title}" movido a ${event.start?.toISOString()}`);
  }

  hayError() {
    return this.$store.getters.getError != "";
  }

  getError() {
    return this.$store.getters.getError;
  }

  getAll(): TaskList[] {
    return this.$store.getters.getAll;
  }
  
  private convertirAEventSource = (datos: TaskList[]): EventSourceInput => {
    return {
      events: datos.map((item) => ({
        title: item.nombre, // Mapeamos 'nombre' a 'title'
        start: item.fecha.toString(),  // Convertir 'fecha' a 'Date'
      })),
    }
  }





}
</script>

<style scoped>
/* Añade estilos si es necesario */
</style>
