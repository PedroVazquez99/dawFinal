<template>
    <div class="card" v-bind:key="list.id" v-bind:style="{ 'background-color': list.color }"
        v-if="getVisibility(index)">
        <div class="card-body">{{ list.nombre }}</div>

        <div class="tituloLista">

            <!-- BOTONES -->
            <div class="btn-group">
                <!-- Eliminar -->
                <button v-on:click="deleteList(list, index)" class="btn btn-sm btn-danger"
                    v-bind:title="$t('delete.ttpBtn')">
                    <font-awesome-icon icon="trash"></font-awesome-icon>
                </button>
                <!-- Visible -->
                <button v-if="list.visible == true" v-on:click="visibleList(list, index)" class="btn btn-sm btn-info"
                    v-bind:title="$t('list.ttpNoVisible')">
                    <font-awesome-icon icon="eye-slash"></font-awesome-icon>
                </button>
                <!-- No visible -->
                <button v-if="list.visible == false" v-on:click="visibleList(list, index)" class="btn btn-sm btn-info"
                    v-bind:title="$t('list.ttpVisible')">
                    <font-awesome-icon icon="eye"></font-awesome-icon>
                </button>
                <!-- Editar -->
                <button type="button" v-on:click="editarLista(list, index)" class="btn btn-sm btn-primary"
                    v-bind:title="$t('list.ttpEditar')">
                    <font-awesome-icon icon="edit"></font-awesome-icon>
                </button>
            </div>
        </div>

        <!-- <h5>{{ list.nombre }}</h5> -->
        <small>{{ list.fecha | formatDate }}</small>
    </div>
</template>

<script lang="ts">
import { Component, Prop, Vue } from 'vue-property-decorator';
import TaskList from '@/models/TaskList';
import moment from 'moment';

@Component({
    filters: {
        formatDate: function (value: any) {
            //TODO, verificar que hay algo en value!
            return moment(value).format("DD/MM/YYYY hh:mm:ss");
        }
    }
})
export default class Item extends Vue {
    $refs!: {
        newList_nombre: HTMLFormElement
    }

    // Variables
    @Prop() private list!: TaskList;
    @Prop() private index!: number;
    @Prop() private slctVisible!: string;


    // Metodos
    visibleList(list: TaskList, index: number) {
        if (list) {

            this.$store.dispatch('setVisibleList', { list, index });

        }
    }
    deleteList(list: TaskList, index: number) {
        if (list) {
            this.$store.dispatch('deleteList', { list, index });
        }
    }
    getVisibility(index: number) {
        let res = true;
        switch (this.slctVisible) {
            case "V":
                // res = this.lists[index].visible;
                res = this.$store.getters.getVisible(index);
                break;
            case "N":
                res = !this.$store.getters.getVisible(index);
                break;
            default:
                res = true;
                break;
        }
        return res;
    }
    editarLista(list: TaskList, index: number) {
        
        this.$swal.fire({
            title: "Editar lista",
            html: `
        <label for="inpNombre" class="w-25">Nombre:</label>
        <input type="text" class="swal2-input w-50" placeholder="Ingresa tu nombre" id="inpNombre">
        <label for="inpColor" class="w-25">Color</label>
        <input type="color" class="swal2-input w-50" id="inpColor">
        <label for="inpFecha" class="w-25">Fecha:</label>
        <input type="date" class="swal2-input w-50" id="inpFecha">
      `,
            inputAttributes: {
                autocapitalize: "off"
            },
            showCancelButton: true,
            cancelButtonText: "Cancelar",
            confirmButtonText: "Aceptar",
            showLoaderOnConfirm: true,
            didOpen: async () => {
                try {

                    const inputElement: any = document.getElementById("inpNombre");
                    const inputElement2: any = document.getElementById("inpColor");
                    const inputElement3: any = document.getElementById("inpFecha");

                    const listaActual = this.$store.getters.getAll[index];
        
                    inputElement.value = listaActual.nombre;
                    inputElement2.value = listaActual.color;
                    inputElement3.value = listaActual.fecha.split('T')[0];

                } catch (error) {
                    this.$swal.showValidationMessage(`
            Request failed: ${error}
          `);
                }
            },
            allowOutsideClick: () => !this.$swal.isLoading(),
            preConfirm: async () => {
                const inputElement: any = document.getElementById("inpNombre");
                const inpDate: any = document.getElementById("inpFecha");
                
                const [year, month, day] = inpDate.value.split("-");
                
                if (inputElement.value == '') { // Verifica si el valor está vacío
                    this.$swal.showValidationMessage("El campo nombre no puede ser vacío");
                    return false; // Previene el cierre del modal
                }
                if (!year || !month || !day) { // Verifica si el valor está vacío
                    this.$swal.showValidationMessage("El campo fecha no puede ser vacío");
                    return false; // Previene el cierre del modal
                }
                    return;
            }
        }).then((result) => {
            if (result.isConfirmed) {
                const inputElement: any = document.getElementById("inpNombre");
                const inputElement2: any = document.getElementById("inpColor");
                const inputElement3: any = document.getElementById("inpFecha");

                this.list.nombre = inputElement.value;
                this.list.color = inputElement2.value;
                this.list.fecha = inputElement3.value;

                this.$store.dispatch('setLista', [this.list, index]);
            }
        });
    }
    mounted() {
        console.log("Se monta el componente");
    }

    hayError() {
        return this.$store.getters.getError != "";
    }

    getError() {
        return this.$store.getters.getError;
    }
    

}
</script>

<style scoped>
.titulolista {
    font-size: 2em;
}
</style>