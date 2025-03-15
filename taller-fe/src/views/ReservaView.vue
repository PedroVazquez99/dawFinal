<template>
    <div class="home">
      <img alt="Vue logo" src="../assets/logo.png">
      <h1>Paginna de reservas</h1>
      <FullCalendarComponent></FullCalendarComponent>
    </div>
  </template>
  
  <script lang="ts">
  import { Component, Vue } from 'vue-property-decorator';
  import FullCalendarComponent from '@/components/FullCalendarComponent.vue';
  
  @Component({
    components: {
      FullCalendarComponent
    },
  })
  export default class HomeView extends Vue {}
  </script>
  