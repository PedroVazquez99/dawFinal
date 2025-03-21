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
import moment, { duration, Moment } from "moment";
import { OkModal, deleteModal, errorModal } from "./modals/ModalAdapter";
import { serviciosPeluqueriaMock } from "@/mocks/servicios.mock";

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
      timeZone: 'UTC',
      initialView: "dayGridMonth", // Vista inicial
      editable: true, // Hace que las citas sean editables (drag and drop)
      selectable: true, // Permite seleccionar citas
      firstDay: 1, // Lunes como primer día de la semana
      dayMaxEvents: 3, // Máximo de eventos por día
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
      eventClick: this.handleEventClick, // Maneja cuando se hace clic en un evento
      dateClick: this.handleDateClick, // Maneja cuando se hace clic en una fecha
      eventDrop: this.handleEventDrop, // Maneja cuando un evento es arrastrado
      moreLinkText: (num) => `Ver ${num} más`, // Cambia "+X more" por "Ver X más"
    });

    this.calendar.render(); // Renderiza el calendario, debe llamarse al final
  }

  // Para liberar memoria
  beforeDestroy() {
    if (this.calendar) {
      this.calendar.destroy();
    }
  }

  // ........................................................................................................
  // ----------------------------------------------- HANDLERS -----------------------------------------------
  // ........................................................................................................

  // Cuando hago click en el día.
  private async handleDateClick(info: any): Promise<void> {
    
    const opcionesServicios = serviciosPeluqueriaMock
      .map(servicio => `<option value="${servicio.nombre}">${servicio.nombre}</option>`)
      .join("");

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
        <div style="display: flex; align-items: center; gap: 10px;">
          <i class="fas fa-cut"></i>
          <select id="servicio" class="swal2-input" style="flex: 1;">
            <option value="" disabled selected>Selecciona un servicio</option>
            ${opcionesServicios}
          </select>
        </div>
      `,
      preConfirm: () => ({
        nombre: (document.getElementById("nombre") as HTMLInputElement).value,
        hora: (document.getElementById("hora") as HTMLInputElement).value,
      }),
      showCancelButton: true,
    });

    if (value?.nombre && value?.hora) {
      const nuevaFecha = `${info.dateStr}T${value.hora}:00`;

      // Verificar colisión
      const duracion = 30; // Duración en minutos del evento
      if (this.isColision(nuevaFecha, duracion)) {
        errorModal("Conflicto de horarios", "Ya existe un evento en esta franja horaria.");
        return;
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

  // Cuando hago click en un evento.
  private handleEventClick(info: any) {

    deleteModal('¿Desea borrar la cita?', 'Se borrará la cita para ' + info.event.title).then(() => {
      const id: string = info.event.id;
      console.log(id);
      this.$store.dispatch("deleteList", id);
      info.event.remove();
      this.calendar.render();
    })
  }

  // Cuando se arrastra una cita
  private handleEventDrop(info: any): void {
    const { event } = info;

    // Validar si la nueva fecha es posterior al día actual
    const hoy = new Date();
    if (event.start && (event.start < hoy || this.isColision(event.start.toISOString(), 30))) {
      errorModal("Acción cancelada", "No se puede mover la cita a una fecha anterior o con conflicto de horarios.");
      info.revert(); // Revertir el cambio
      return;
    }
    console.log(info);

    let t = new TaskList();
    t.id = parseInt(info.event.id);
    t.nombre = info.event.title;
    t.fecha = info.event.start;

    if(!this.isColision(t.fecha.toISOString(), 30)){
      
      this.$store.dispatch('setLista', [t, t.id]);
      OkModal("Cita actualizada", "La cita se ha movido correctamente.");
    }
  }

  // ........................................................................................................
  // ----------------------------------------------- MÉTODOS ------------------------------------------------
  // ........................................................................................................

  private convertirAEventSource = (datos: TaskList[]): EventSourceInput => {
    return {
      events: datos.map((item) => ({
        id: item.id.toString(), // Mapeamos 'id' a 'id'
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
    return eventos.some((evento) => this.eventoEnRango(evento, inicio, fin, duracionEnMinutos));
  }

  // Función auxiliar para verificar si una cita existente solapa con el rango propuesto
  private eventoEnRango(evento: any, inicio: Moment, fin: Moment, duracionEnMinutos: number): boolean {
    // Calcular el margen dinámico antes y después
    const margen = moment.duration(duracionEnMinutos, 'minutes');

    // Ajustar el rango del evento existente
    const eventoInicio = moment(evento.start).subtract(margen); // Reducir el inicio
    const eventoFin = moment(evento.end || evento.start).add(margen); // Ampliar el fin

    // Comprobar si los rangos se solapan
    return this.rangoSeSolapa(inicio, fin, eventoInicio, eventoFin);
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


}
</script>

<style scoped>
/* Añade estilos si es necesario */
</style>
