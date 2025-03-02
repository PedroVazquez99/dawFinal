<template>
  <div class="about">
    <h1>{{ titulo }}</h1>
    <p v-html="texto"></p>
    <div class="alert alert-danger" v-if="errorMsg != ''">
      Error al obtener los datos del CMS, causa: {{ errorMsg }}
    </div>
  </div>
</template>

<script lang="ts">
import { Component, Vue } from 'vue-property-decorator';
import serviceAPI from '@/services/service';
import { APIStatus } from '@/http-config';
@Component
export default class About extends Vue {
  titulo = this.$i18n.t('app.acerca_de');
  texto = "";
  errorMsg = "";
  mounted() {
    serviceAPI.get("CMS/about").then(r => {
      if (r.status == APIStatus.OK) {
        if (r != null && r.respuesta != null) {
          this.titulo = r.respuesta["titulo"];
          this.texto = r.respuesta["texto"];
        } else {
          this.errorMsg = "No se han recibido datos";
        }
      } else {
        this.errorMsg = r.error;
      }
    }).catch(e => {
      this.errorMsg = e;
    })

  }
}
</script>