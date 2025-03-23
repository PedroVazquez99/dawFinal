<template>
    <div class="home">
      <!-- <img alt="Vue logo" src="../assets/logo.png"> -->
      <h1>Pagina de reservas</h1>
      <FullCalendarUser></FullCalendarUser>
    </div>
  </template>
  
  <script lang="ts">
  import { Component, Vue } from 'vue-property-decorator';
  import FullCalendarUser from '@/components/FullCalendarUser.vue';
  
  @Component({
    components: {
      FullCalendarUser
    },
  })
  export default class HomeView extends Vue {}
  </script>
  