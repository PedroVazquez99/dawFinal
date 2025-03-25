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

@Component
  export default class FullCalendarUser extends Vue {
    @Prop() private titulo!: string;
    private calendar!: Calendar;
    private errGet = "";
    private errAdd = "";
    private usuarioID: string | null = null; // Cogerlo desde el store, ver como hacerlo

    mounted() {
      if (!this.$refs.calendar) return;
      Promise.all([
        this.$store.dispatch("fetchCurrentUser"),
        this.$store.dispatch("fetchServicios"),
        this.$store.dispatch("getLists"),
      ])
        .then(() => {
          this.errGet = this.getErrorIfExists();
          const interval = setInterval(() => {
            const events = this.getAll();
            console.log("Esperando datos de getAll:", events);
            if (events.length > 0) {
              clearInterval(interval);
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
                left: "dayGridMonth,timeGridWeek,timeGridDay",
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
        }, 100);
      })
      
      this.errGet = this.getErrorIfExists(); 
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
        this.addEvent(value.nombre, nuevaFecha, Number(value.servicioId));
      }
    }
// holaaaaa  
    private async handleEventClick(info: any): Promise<void> {
      const evento = info.event;
      const { value, isDenied } = await this.showSwal(
        "Editar cita",
        evento.title,
        moment.utc(evento.start).format("HH:mm"), // Display time in UTC
        evento.extendedProps.servicioId, // Pass the service ID to pre-select it
        true
      );
      if (isDenied) {
        this.deleteEvent(evento);
        return;
      }
      if (value?.nombre && value?.hora && value?.servicioId) {
        const nuevaFecha = this.combineDateAndTime(moment.utc(evento.start).format("YYYY-MM-DD"), value.hora);
        if (this.isColision(nuevaFecha, 30, evento.id)) { // Pass the event ID to exclude it from collision checks
          errorModal("Conflicto de horarios", "Ya existe un evento en esta franja horaria.");
          return;
        }
        this.updateEvent(evento, value.nombre, nuevaFecha, Number(value.servicioId));
      }
    }
  //hola2
    private async handleEventDrop(info: any): Promise<void> {
      const { event } = info;

      // Validar conflictos de horarios excluyendo el evento actual
      if (this.isColision(event.start?.toISOString() || "", 30, event.id)) {
        errorModal("Acción cancelada", "No se puede mover la cita a una fecha anterior o con conflicto de horarios.");
        info.revert();
        return;
      }

      try {
        // Actualizar la tarea en el store
        await this.updateTask(event.id, event.title, event.start?.toISOString() || "", Number(event.extendedProps.servicioId));

        // Sincronizar el estado del evento en el calendario
        event.setStart(event.start?.toISOString() || "");
        OkModal("Cita actualizada", "La cita se ha movido correctamente.");
      } catch (error) {
        console.error("Error al actualizar la cita:", error);
        errorModal("Error", "No se pudo actualizar la cita. Inténtalo de nuevo.");
        info.revert();
      }
    }
  
    private deleteEvent(evento: any): void {
      console.log(evento.extendedProps, this.usuarioID);

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
          usuarioId: Number(item.usuarioId),
          servicioId: Number(item.servicioId),
        })),
      };
    }
  
    private async addEvent(nombre: string, fecha: string, servicioId: number): Promise<void> {
      const newTask = new TaskList();
      newTask.nombre = nombre;
      newTask.fecha = moment.utc(fecha); // Almacenar en UTC
      newTask.usuarioId = this.usuarioID ? Number(this.usuarioID) : 0;
      newTask.servicioId = servicioId;

      try {
        // Llamar a addList y obtener el ID generado por el backend
        const newId = await this.$store.dispatch("addList", newTask);
        
        console.log("ID de la nueva cita:", newId);
        // Añadir el evento al calendario con el ID generado
        this.calendar.addEvent({
          id: newId, // Usar el ID generado
          title: nombre,
          start: fecha, // Usar la fecha en formato UTC
          extendedProps: { usuarioId: Number(this.usuarioID), servicioId },
        });

        console.log("Cita creada:", this.calendar.getEventById(newId));

        OkModal("Cita creada", "La cita ha sido creada correctamente.");
      } catch (error) {
        console.error("Error al crear la cita:", error);
        errorModal("Error", "No se pudo crear la cita. Inténtalo de nuevo.");
      }
  }
  
    private updateEvent(evento: any, nombre: string, fecha: string, servicioId: number): void {

      evento.setProp("title", nombre); // Update the title in the calendar
      evento.setStart(fecha); // Update the start date in the calendar
      evento.setExtendedProp("servicioId", servicioId); // Update the service ID in the calendar
      this.updateTask(evento.id, nombre, fecha, servicioId); // Update the event in the store
    }
  
    private async showSwal(title: string, nombre = "", hora = "", servicioId = "", showDeleteButton = false) {
      const opcionesServicios = this.serviciosDelStore
        .map((servicio: any) => `
          <option value="${servicio.id}" ${servicio.id === Number(servicioId) ? "selected" : ""}>
            ${servicio.nombre + " - " + servicio.precio + "€"}
          </option>
        `)
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
              <option value="" disabled>Selecciona un servicio</option>
              ${opcionesServicios}
            </select>
          </div>
        `,
        preConfirm: () => ({
          nombre: (document.getElementById("nombre") as HTMLInputElement).value,
          hora: (document.getElementById("hora") as HTMLInputElement).value,
          servicioId: (document.getElementById("servicio") as HTMLSelectElement).value,
        }),
        showCancelButton: true,
        showDenyButton: showDeleteButton,
        denyButtonText: "Eliminar",
      });
    }
  
    // mira las colisiones entre citas
    private isColision(fecha: string, duracionEnMinutos: number, excludeEventId?: string): boolean {
      const inicio = moment.utc(fecha); // Start time in UTC
      const fin = moment.utc(fecha).add(duracionEnMinutos, "minutes"); // End time in UTC
    
      return this.calendar.getEvents().some((evento) => {
        if (excludeEventId && evento.id === excludeEventId) {
          return false; // Exclude the currently edited or moved event
        }
    
        const eventoInicio = moment.utc(evento.start); // Event start time in UTC
        const eventoFin = moment.utc(evento.end || evento.start).add(duracionEnMinutos, "minutes"); // Event end time in UTC
    
        // Check if the given time range overlaps with the event's time range
        return (
          inicio.isBefore(eventoFin) && fin.isAfter(eventoInicio) || // Overlap from above
          fin.isAfter(eventoInicio) && inicio.isBefore(eventoFin)    // Overlap from below
        );
      });
    }
  
    // añade una cita
    private async addList(item: TaskList): Promise<void> {
      if (item.nombre) {
        await this.$store.dispatch("addList", item);
        this.errAdd = this.getErrorIfExists();
      }
    }
  
    // actializa los valores de una cita
    private async updateTask(id: string, nombre: string, fecha: string, servicioId: number): Promise<void> {
      const task = this.getAll().find((t) => t.id === Number(id));
      if (task) {
        task.nombre = nombre;
        task.fecha = moment.utc(fecha); // Store the updated date in UTC
        task.servicioId = servicioId; // Update the service ID
        await this.$store.dispatch("setLista", [task, task.id]); // Dispatch the update to the store
      } else {
        errorModal("No se pudo encontrar la cita para actualizar."); // Handle missing task
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

