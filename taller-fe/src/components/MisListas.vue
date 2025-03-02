<template>
  <div>
    <h1>{{ titulo }}</h1>
    <button v-if="mode != 'add'" v-on:click="addMode()" class="btn btn-primary">
      {{ $t("add.btnAdd") }}
    </button>
    <div class="container" v-if="errGet == '' && mode == 'list'">
      <fieldset>
        <legend>{{ $t("list.titulo") }}</legend>

        <select class="form-control col-md-3" v-model="slctVisible">
          <option value="T">{{ $t("list.optTodas") }}</option>
          <option value="V">{{ $t("list.optVisibles") }}</option>
          <option value="N">{{ $t("list.optNOVisibles") }}</option>
        </select>

        <select class="form-control col-md-3" v-model="orden">
          <option value="N">{{ $t("list.ordenNombre") }}</option>
          <option value="D">{{ $t("list.ordenFecha") }}</option>
        </select>

        <!--------------------------------------------------- ITEMS --------------------------------------------------->
        <template v-for="(list, index) in getAll()">
          <div :key="index">
            <Item :list="list" :index="index" :slctVisible="slctVisible"></Item>
          </div>
        </template>

        <div v-if="getCount() == 0" class="alert alert-warning">
          {{ $t("list.noLists") }}
        </div>
      </fieldset>
    </div>

    <!--------------------------------------------------- CREAR LISTA --------------------------------------------------->
    <div class="container" v-if="errGet == '' && mode == 'add'">
      <fieldset>
        <legend>{{ $t("add.titulo") }}</legend>
        <form>
          <!-- NOMBRE -->
          <div class="form-group">
            <label for="addListNombre">{{ $t("add.lblNombre") }}</label>
            <input
              ref="newList_nombre"
              v-model="newList.nombre"
              type="text"
              class="form-control"
              id="addListNombre"
              v-bind:placeholder="$t('add.ttpNombre')"
            />
            <!-- v-bind:placeholder="$t('add.ttpNombre')" -->
            <span v-if="errList.nombre" class="small text-danger">
              {{ $t("add.errNombre") }}
            </span>
          </div>

          <!-- COLOR -->
          <div class="form-group">
            <label for="addListColor"> {{ $t("add.lblColor") }}</label>
            <input
              v-model="newList.color"
              type="color"
              class="form-control"
              id="addListColor"
            />
          </div>
          <!-- FECHA -->
          <div class="form-group">
            <label for="addListFecha"> {{ $t("add.lblFecha") }}</label>
            <input
              v-model="newList.fecha"
              type="date"
              class="form-control"
              id="addListFecha"
            />
          </div>
        </form>

        <div class="btn-group">
          <button v-on:click="addList()" class="btn btn-primary">
            {{ $t("add.btnAddList") }}
          </button>
          <button v-on:click="listMode()" class="btn btn-danger">
            {{ $t("app.btnAtras") }}
          </button>
        </div>
        <div class="alert alert-danger" v-if="errAdd != ''">
          Error: {{ errAdd }}
        </div>
      </fieldset>
    </div>
  </div>
</template>

<script lang="ts">
import { Component, Prop, Vue } from "vue-property-decorator";
import TaskList from "@/models/TaskList";
import moment from "moment";
import Item from "@/components/Lista.vue";

@Component({
  filters: {
    formatDate: function (value: any) {
      //TODO, verificar que hay algo en value!
      return moment(value).format("DD/MM/YYYY hh:mm:ss");
    },
  },
  components: {
    Item,
  },
})
export default class Mislistas extends Vue {
  $refs!: {
    newList_nombre: HTMLFormElement;
  };

  // Variables
  @Prop() private titulo!: string;
  mode = "list";
  newList: TaskList = new TaskList();
  errList = { nombre: false };
  slctVisible = "T";
  orden = "N";
  showModal = false;
  errGet = "";
  errAdd = "";
  editList: TaskList = new TaskList();

  // Metodos
  addMode(): void {
    //VUEX: let nextId= this.lists.length>0?this.lists[this.lists.length-1].id+1:+1;
    // let nextId = this.$store.getters.getNextId;
    this.newList = new TaskList();
    // this.newList.id = nextId;
    this.mode = "add";
    this.errList.nombre = false;
  }
  listMode(): void {
    this.mode = "list";
  }
  addList(): void {
    if (this.newList.nombre != "") {
      this.newList.fecha = moment(this.newList.fecha);
      // this.lists.push(this.newList);
      this.$store.dispatch("addList", this.newList);
      this.errAdd = "";
      if (this.hayError()) {
        this.errAdd = this.getError();
      } else {
        this.mode = "list";
      }
    } else {
      this.errList.nombre = true;
      this.$refs.newList_nombre.focus();
    }
  }

  getAll() {
    return this.$store.getters.getAll;
  }
  getCount() {
    return this.$store.getters.getCount;
  }
  mounted() {
    this.$store.dispatch("getLists");
    this.errGet = "";
    if (this.hayError()) {
      this.errGet = this.getError();
    }
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
