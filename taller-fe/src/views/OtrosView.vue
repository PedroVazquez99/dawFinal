<template>
    <div class="otros">
      <h1>{{ titulo }}</h1>
      <div class="alert alert-danger" v-if="errorMsg != ''">
        {{ errorMsg }}
      </div>
      <div class="alert alert-primary" v-for="(item, index) in listado" :key="index">
        <p>{{ item.nombre }}</p>
        <a class="btn btn-secondary">{{ item.url }}</a>
    </div>
    </div>
  </template>
  
  <script lang="ts">
  import { Component, Vue } from 'vue-property-decorator';
  import serviceAPI from '@/services/service';
  import { APIStatus } from '@/http-config';
  @Component
  export default class Otros extends Vue {
    titulo = this.$i18n.t('app.otros');
    errorMsg = "";
    listadoKeys: string[] = [];
    listado: {nombre: string, url: string}[] = [];

    // realizo la llamada
    mounted() {
     
      serviceAPI.get("Listas/otros").then(r => {
        if (r.status == APIStatus.OK) {
          if (r != null && r.respuesta != null && Object.keys(r.respuesta).length > 0) {
            this.listadoKeys = Object.keys(r.respuesta); // hago un listado de las keys: [Home, About, Others]
            // console.log(this.listadoKeys);
            Object.entries(r.respuesta).forEach(([key, value]) => { // me monto el objeto para representarlo en el BACK
              if(typeof value === "string"){
                this.listado.push({nombre: key, url: value})
              }
            });
            console.log(r);
          } else {
            this.errorMsg = "No se han recibido datos";
          }
        } else {
          this.errorMsg = r.error;
        }
      }).catch(e => {
        this.errorMsg = "Se produjo un error el listado de URL";
      })
  
    }
  }
  </script>