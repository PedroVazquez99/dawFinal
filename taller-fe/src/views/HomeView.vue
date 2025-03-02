<template>
  <div class="home">
    <img alt="Vue logo" src="../assets/logo.png">
    <Mislistas v-bind:msg="$t('app.titulo')"/>
  </div>
</template>

<script lang="ts">
import { Component, Vue } from 'vue-property-decorator';
import Mislistas from '@/components/MisListas.vue';

@Component({
  components: {
    Mislistas
  },
})
export default class HomeView extends Vue {}
</script>
