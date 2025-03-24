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
  import moment from "moment";
  import { OkModal, deleteModal, errorModal } from "./modals/ModalAdapter";
  import { serviciosPeluqueriaMock } from "@/mocks/servicios.mock";
  import { Citas } from "@/mocks/miscitas.mock";
  
  @Component
  export default class FullCalendarUser extends Vue {
    @Prop() private titulo!: string;
    private calendar!: Calendar;
    private errGet = "";
    private errAdd = "";
    private usuarioID: string | null = null; // Cogerlo desde el store, ver como hacerlo
    private servicioID = 1; // Cogerlo desde el store, ver como hacerlo
    mounted() {
      if (!this.$refs.calendar) return;
      console.log("Usuario cargado:", this.currentUser);
      //Recupera el usuario
      this.$store.dispatch("fetchCurrentUser")
      .then(() => {
        console.log("Usuario cargado:", this.currentUser);
        this.usuarioID = this.currentUserId;
      })
      .catch((error) => {
        console.error("Error al cargar el usuario:", error);
      });

      // Recupera los servicios
      this.$store.dispatch("fetchServicios")
      .then(() => {
        // console.log("Servicios cargados:", this.servicios);
      })
      .catch((error) => {
        console.error("Error al cargar los servicios:", error);
      });


      this.$store.dispatch("getLists");
      this.errGet = this.getErrorIfExists();
  
      this.calendar = new Calendar(this.$refs.calendar as HTMLElement, {
        plugins: [dayGridPlugin, timeGridPlugin, interactionPlugin],
        locale: "es",
        initialView: "dayGridMonth",
        editable: true,
        eventDurationEditable: false,
        selectable: true,
        firstDay: 1,
        dayMaxEvents: 3,
        timeZone: 'UTC',
        headerToolbar: {
          left: "",
          center: "title",
          right: "prev,next today",
        },
        buttonText: { today: "Hoy", month: "Mes", week: "Semana", day: "Día" },
        validRange: { start: this.getToday() },
        eventTimeFormat: { hour: "2-digit", minute: "2-digit", meridiem: false },
        selectAllow: (selectInfo) => selectInfo.startStr === selectInfo.endStr.split("T")[0],
        events: this.convertirAEventSource(this.getAll()),
        eventClick: this.handleEventClick,
        dateClick: this.handleDateClick,
        eventDrop: this.handleEventDrop,
        moreLinkText: (num) => `Ver ${num} más`,
      });
  
      this.calendar.render();
    }
  
    beforeDestroy() {
      this.calendar?.destroy();
    }

    //Metodo para obtener usuario de store
    get currentUser() {
      return this.$store.getters.getAuthenticatedUser;
    }
    //Metodo para obtener id de usuario de store
    get currentUserId() {
      return this.$store.getters.getAuthenticatedUser.userId || null;
    }
    // Acceso a los servicios
    private get serviciosDelStore() {
      return this.$store.state.servicios;
    }
  
    private async handleDateClick(info: any): Promise<void> {
      const { value } = await this.showSwal("Crear cita");
      if (value?.nombre && value?.hora) {
        const nuevaFecha = this.combineDateAndTime(info.dateStr, value.hora);
        if (this.isColision(nuevaFecha, 30)) {
          errorModal("Conflicto de horarios", "Ya existe un evento en esta franja horaria.");
          return;
        }
        this.addEvent(value.nombre, nuevaFecha);
      }
    }
  
    private async handleEventClick(info: any): Promise<void> {
      const evento = info.event;
      const { value, isDenied } = await this.showSwal(
        "Editar cita",
        evento.title,
        moment.utc(evento.start).format("HH:mm"), // Display time in UTC
        true
      );
      if (isDenied) {
        this.deleteEvent(evento);
        return;
      }
      if (value?.nombre && value?.hora) {
        const nuevaFecha = this.combineDateAndTime(moment.utc(evento.start).format("YYYY-MM-DD"), value.hora);
        if (this.isColision(nuevaFecha, 30, evento.id)) { // Pass the event ID to exclude it from collision checks
          errorModal("Conflicto de horarios", "Ya existe un evento en esta franja horaria.");
          return;
        }
        this.updateEvent(evento, value.nombre, nuevaFecha);
      }
    }
  
    private handleEventDrop(info: any): void {
      const { event } = info;
      if (event.start && (event.start < new Date() || this.isColision(event.start.toISOString(), 30, event.id))) {
        errorModal("Acción cancelada", "No se puede mover la cita a una fecha anterior o con conflicto de horarios.");
        info.revert();
        return;
      }
      this.updateTask(event.id, event.title, event.start.toISOString());
    }
  
    private deleteEvent(evento: any): void {
      deleteModal("¿Desea borrar la cita?", `Se borrará la cita para ${evento.title}`).then(() => {
        this.$store.dispatch("deleteList", evento.id);
        evento.remove();
        OkModal("Cita eliminada", "La cita ha sido eliminada correctamente.");
      });
    }
  
    private convertirAEventSource(datos: TaskList[]): EventSourceInput {
      return {
        events: datos.map((item) => ({
          id: item.id.toString(),
          title: item.nombre,
          start: item.fecha.toString(),
        })),
      };
    }
  
    private addEvent(nombre: string, fecha: string): void {
      const newTask = new TaskList();
      newTask.nombre = nombre;
      newTask.fecha = moment.utc(fecha); // Store in UTC
      newTask.usuarioId = this.usuarioID ? Number(this.usuarioID) : 0; // Cogerlo desde el store, ver como hacerlo
      newTask.servicioId = this.servicioID; // Cogerlo desde el store, ver como hacerlo
      this.addList(newTask);
      this.calendar.addEvent({
        title: nombre,
        start: fecha, // Use UTC ISO string
        extendedProps: { usuarioId: this.usuarioID, servicioId: this.servicioID },
      });
    }
  
    private updateEvent(evento: any, nombre: string, fecha: string): void {
      evento.setProp("title", nombre); // Update the title in the calendar
      evento.setStart(fecha); // Update the start date in the calendar
      this.updateTask(evento.id, nombre, fecha); // Update the event in the store
    }
  
    private async showSwal(title: string, nombre = "", hora = "", showDeleteButton = false) {
      const opcionesServicios = serviciosPeluqueriaMock
        .map((servicio) => `<option value="${servicio.nombre}">${servicio.nombre}</option>`)
        .join("");
      return Swal.fire({
        title,
        html: `
          <div style="display: flex; align-items: center; gap: 10px;">
            <i class="fas fa-user"></i>
            <input id="nombre" class="swal2-input" placeholder="Nombre del cliente" style="flex: 1;" value="${nombre}">
          </div>
          <div style="display: flex; align-items: center; gap: 10px;">
            <i class="fas fa-clock"></i>
            <input id="hora" type="time" class="swal2-input" style="flex: 1;" value="${hora}">
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
        showDenyButton: showDeleteButton,
        denyButtonText: "Eliminar",
      });
    }
  
    // mira las colisiones entre citas
    private isColision(fecha: string, duracionEnMinutos: number, excludeEventId?: string): boolean {
      const inicio = moment.utc(fecha); // Ensure the start time is in UTC
      const fin = moment.utc(fecha).add(duracionEnMinutos, "minutes"); // Ensure the end time is in UTC
      return this.calendar.getEvents().some((evento) => {
        if (excludeEventId && evento.id === excludeEventId) {
          return false; // Exclude the currently edited event
        }
        const eventoInicio = moment.utc(evento.start); // Ensure the event start time is in UTC
        const eventoFin = moment.utc(evento.end || evento.start).add(duracionEnMinutos, "minutes"); // Ensure the event end time is in UTC
        return inicio.isBefore(eventoFin) && fin.isAfter(eventoInicio);
      });
    }
  
    // añade una cita
    private addList(item: TaskList): void {
      if (item.nombre) {
        this.$store.dispatch("addList", item);
        this.errAdd = this.getErrorIfExists();
      }
    }
  
    // actializa los valores de una cita
    private updateTask(id: string, nombre: string, fecha: string): void {
      const task = this.getAll().find((t) => t.id === Number(id));
      if (task) {
        task.nombre = nombre;
        task.fecha = moment.utc(fecha); // Store the updated date in UTC
        this.$store.dispatch("setLista", [task, task.id]); // Dispatch the update to the store
        OkModal("Cita actualizada", "La cita se ha actualizado correctamente."); // Show success message
      } else {
        errorModal("Error", "No se pudo encontrar la cita para actualizar."); // Handle missing task
      }
    }
  
    // ----------------------------------------------------------------------- HELPERS -----------------------------------------------------------------------
  
    private getErrorIfExists(): string {
      return this.$store.getters.getError || "";
    }
  
    private getToday(): string {
      return new Date().toISOString().split("T")[0];
    }
  
    private combineDateAndTime(date: string, time: string): string {
      return moment.utc(`${date}T${time}:00`).toISOString(); // Combine and convert to UTC
    }
  
    private getAll(): TaskList[] {
      return this.$store.getters.getAll;
    }
  }
  </script>
  
  <style scoped>
  /* Añade estilos si es necesario */
  </style>
