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
import Swal from "sweetalert2";
import moment, { Moment } from "moment";
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
      editable: true, // Hace que las citas sean editables (drag and drop)
      selectable: true, // Permite seleccionar citas
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
      selectAllow: (selectInfo) => {
        // Solo permitir selecciones de un día
        return selectInfo.startStr === selectInfo.endStr.split("T")[0];
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

  private async handleDateClick(info: any): Promise<void> {
  const { value } = await Swal.fire({
    title: "Crear cita",
    html: `
      <div style="display: flex; align-items: center; gap: 10px;">
        <i class="fas fa-user"></i>
        <input id="nombre" class="swal2-input" placeholder="Nombre del cliente" style="flex: 1;">
      </div>
      <div style="display: flex; align-items: center; gap: 10px;">
        <i class="fas fa-clock"></i>
        <input id="hora" type="time" class="swal2-input" style="flex: 1;">
      </div>
    `,
    preConfirm: () => ({
      nombre: (document.getElementById("nombre") as HTMLInputElement).value,
      hora: (document.getElementById("hora") as HTMLInputElement).value,
    }),
    showCancelButton: true,
  });
  console.log('Paso por aqui'); // Verificar si se ejecuta el código
  if (value?.nombre && value?.hora) {
    const nuevaFecha = `${info.dateStr}T${value.hora}:00`;

    // Verificar colisión
    const duracion = 30; // Duración en minutos del evento
    if (this.isColision(nuevaFecha, duracion)) {
      await Swal.fire({
        icon: "error",
        title: "Conflicto de horarios",
        text: "Ya existe un evento en esta franja horaria.",
      });
      console.log('Paso por aqui 2'); // Verificar si se ejecuta el código
      return; // No añadir el evento debido a la colisión
    }

    // Si no hay colisión, añadir el evento
    const newTask = new TaskList();
    newTask.nombre = value.nombre;
    newTask.fecha = moment(nuevaFecha);
    this.addList(newTask);

    this.calendar.addEvent({
      title: value.nombre,
      start: nuevaFecha,
    });
  }
}

  // Cuando se arrastra una cita
  private handleEventDrop(info: any): void {
    const { event } = info;

    // Validar si la nueva fecha es posterior al día actual
    const hoy = new Date();
    if (event.start && event.start < hoy) {
      alert("No puedes mover una cita a una fecha pasada.");
      info.revert(); // Revertir el cambio
      return;
    }

    alert(`Evento "${event.title}" movido a ${event.start?.toISOString()}`);
  }

  private convertirAEventSource = (datos: TaskList[]): EventSourceInput => {
    return {
      events: datos.map((item) => ({
        title: item.nombre, // Mapeamos 'nombre' a 'title'
        start: item.fecha.toString(),  // Convertir 'fecha' a 'Date'
      })),
    }
  }

  addList(item: TaskList): void {
    if (item.nombre != "") {
      
      this.$store.dispatch("addList", item);
      
      this.errAdd = "";
      if (this.hayError()) {
        this.errAdd = this.getError();
      }
    } 
  }

  // Verificar si hay colisión con otra cita
    private isColision(fecha: string, duracionEnMinutos: number): boolean {
    const inicio = moment(fecha);
    const fin = moment(fecha).add(duracionEnMinutos, 'minutes');
    const eventos = this.calendar.getEvents();

    return eventos.some((evento) => this.eventoEnRango(evento, inicio, fin));
  }

  // Función auxiliar para verificar si una cita existente solapa con el rango propuesto
  private eventoEnRango(evento: any, inicio: Moment, fin: Moment): boolean {
    // Añadir un margen de 30 minutos antes y después del evento existente
    const eventoInicio = moment(evento.start)
    const eventoFin = moment(evento.end || evento.start)

    return (
      this.rangoSeSolapa(inicio, fin, eventoInicio, eventoFin)
    );
  }

  // Verificar si el rango de fechas se solapa
  private rangoSeSolapa(inicio: Moment, fin: Moment, eventoInicio: Moment, eventoFin: Moment): boolean {
    return inicio.isBefore(eventoFin) && fin.isAfter(eventoInicio);
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
  

  //   // Validar que no sea una hora posterior al momento actual en la fecha actual
  //   if (fechaSeleccionada < hoy) {
  //     alert("No puedes seleccionar fechas u horas anteriores a ahora."); // Validación
  //     return;
  //   }

  //   const eventoTitulo = prompt("Introduce el nombre de la reserva:"); // Cambiar con Swal
  //   const eventoFecha = prompt("Introduce la hora en formato HH:mm (por ejemplo, 14:30):"); // Cambiar con Swal

  //   if (eventoTitulo && eventoFecha) {
  //     const fechaCompleta = `${info.dateStr}T${eventoFecha}:00`;

  //     this.calendar.addEvent({
  //       title: eventoTitulo,
  //       start: fechaCompleta,
  //     });

  //     // Renderizar para asegurarnos de que la cita se muestre
  //     this.calendar.render();
  //   } else {
  //     alert("Nombre y hora son requeridos para crear la reserva."); // Validación
  //   }
  // }

}
</script>

<style scoped>
/* Añade estilos si es necesario */
</style>
