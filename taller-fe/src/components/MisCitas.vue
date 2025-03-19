<!-- <template>
    <div>
      <ul>
        <li v-for="cita in citas" :key="cita.id" :style="{ color: cita.color || 'black' }">
          <strong>{{ cita.nombre }}</strong> - 
          <span>{{ formatFecha(cita.fecha) }}</span>
          <ul>
            <li v-for="(task, index) in cita.tasks" :key="index">{{ task }}</li>
          </ul>
        </li>
      </ul>
    </div>
  </template>
  
  <script lang="ts">
  import { Component, Vue, Prop } from "vue-property-decorator";
  import moment from "moment";
  import TaskList from "@/models/TaskList";
  
  @Component
  export default class MisCitas extends Vue {
    @Prop({ required: true }) citas!: TaskList[]; // Recibe las citas como prop desde el padre
  
    // Método para formatear las fechas con Moment.js
    formatFecha(fecha: moment.Moment): string {
      return fecha.format("DD/MM/YYYY");
    }
  }
  </script>
  
  <style scoped>
  ul {
    list-style: none;
    padding: 0;
  }
  
  li {
    margin-bottom: 10px;
  }
  </style>
   -->