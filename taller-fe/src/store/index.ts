import Vue from "vue";
import Vuex from "vuex";
import TaskList from "@/models/TaskList";
import serviceAPI from "@/services/service";
import { APIStatus } from "@/http-config";
import ItemTask from "@/models/ItemTask";
import moment from 'moment';
Vue.use(Vuex);

export default new Vuex.Store({
  state: {
    listas: Array<TaskList>(),
    error: "",
  },
  getters: {
    getAll(state) {
      return state.listas;
    },
    getCount(state) {
      return state.listas.length;
    },
    getNextId(state) {
      return state.listas.length > 0
        ? state.listas[state.listas.length - 1].id + 1
        : 1;
    },
    getVisible: (state) => (index: number) => {
      return state.listas[index].visible;
    },
    getError(state) {
      return state.error;
    },
  },
  mutations: { // CRUD del listado, cambia el estado en local
    add(state, lista) {
      lista.visible = lista.visible == "S" ? true : false;
      state.listas.push(lista);
    },
    del(state, index: number) {
      state.listas.splice(index, 1);
    },
    setVisible(state, index: number) {
      state.listas[index].visible = !state.listas[index].visible;
    },
    setLists(state, lists) {
      lists.forEach((e: any) => {
        e.visible = e.visible == "S" ? true : false;
      });
      console.log(lists);
      state.listas = lists;
    },
    setLista(state, [lista, index]){
      // lista.visible = lista.visible == "S" ? true : false;
      if(lista && index >= 0)
      state.listas[index] = lista;
    }
 
  },
  actions: {
    // Obtiene el listado de la BBDD
    getLists({ commit }) {
      serviceAPI
        .getRaw("APIListas")
        .then((r) => {
          console.log(r);
          if (r.status == APIStatus.OK) {
            console.log(r.respuesta);
            commit("setLists", r.respuesta);
          } else {
            this.state.error = r.error;
          }
        })
        .catch((e) => {
          this.state.error = e;
        });
    },
    // Agrega una lista contra la BBDD
    addList({ commit }, lista) {
      lista.fecha = lista.fecha.format("YYYY-MM-DDTHH:mm:ss");
      console.log(lista.visible);
      lista.visible = lista.visible ? "S" : "N";
      console.log(lista.visible);
      serviceAPI
        .post("APIListas", lista)
        .then((r) => {
          if (r.status == APIStatus.OK) {
            commit("add", r.respuesta);
          } else {
            this.state.error = r.error;
          }
        })
        .catch((e) => {
          this.state.error = e;
        });
    },
    // Elimina una lista contra la BBDD
    deleteList({ commit }, {list, index}) {
    
      serviceAPI
        .delete("APIListas", list.id)
        .then((r) => {
          if (r.status == APIStatus.OK) {
            console.log(list);
            commit("del", index);
          } else {
            this.state.error = r.error;
          }
        })
        .catch((e) => {
          this.state.error = e;
        });
    },
    // Cambia el estado de visible contra la BBDD
    setVisibleList({ commit }, {list, index}) {
      
      serviceAPI
        .putVisible("APIListas", list)
        .then((r) => {
          if (r.status == APIStatus.OK) {
            commit("setVisible", index);
          } else {
            this.state.error = r.error;
          }
        })
        .catch((e) => {
          this.state.error = e;
        });
    },
    // EDITA la LISTA contra la BBDD
    setLista({ commit }, [list, index]) {
      console.log(list, index);
      console.log('Info de arriba');
      serviceAPI
        .putLista("APIListas", list)
        .then((r) => {
          if (r.status == APIStatus.OK) {
            commit("setLista", list, index);
          } else {
            this.state.error = r.error;
          }
        })
        .catch((e) => {
          this.state.error = e;
        });
    }

  },
  
  modules: {},
});
