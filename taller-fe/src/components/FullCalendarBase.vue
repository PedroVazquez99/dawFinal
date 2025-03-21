<template>
    <div>
      <div ref="calendar"></div>
    </div>
  </template>
  
  <script lang="ts">
  import { Component, Vue } from "vue-property-decorator";
  import { Calendar, EventSourceInput } from "@fullcalendar/core";
  import dayGridPlugin from "@fullcalendar/daygrid";
  import timeGridPlugin from "@fullcalendar/timegrid";
  import interactionPlugin from "@fullcalendar/interaction";
  
  @Component
  export default class FullCalendarBase extends Vue {
    protected calendar!: Calendar;
  
    mounted() {
      if (!this.$refs.calendar) return;
      this.calendar = new Calendar(this.$refs.calendar as HTMLElement, {
        plugins: [dayGridPlugin, interactionPlugin],
        locale: "es",
        initialView: "dayGridMonth",
        editable: false, // Base by default
        headerToolbar: {
          left: "prev,next today",
          center: "title",
          right: "dayGridMonth,timeGridWeek,timeGridDay",
        },
        events: this.getEvents(),
      });
      this.calendar.render();
    }
  
    beforeDestroy() {
      if (this.calendar) {
        this.calendar.destroy();
      }
    }
  
    protected getEvents(): EventSourceInput {
      return {
        events: [], // Placeholder for data
      };
    }
  }
  </script>
  