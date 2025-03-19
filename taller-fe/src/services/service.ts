import TaskList from "@/models/TaskList";
import http, { APIStatus, APIResponse } from "../http-config";
import router from '@/router';

class APIService {
    async get(ruta: string, id = "") {
        let dato = "";
        const resp = new APIResponse();
        if (id != "") {
            dato = "/" + id;
        }
        await http.get(
            ruta + dato
        ).then((respuesta) => {
            resp.status = respuesta.data.status;
            if (respuesta.data.status == APIStatus.OK) {
                resp.respuesta=respuesta.data.data;
            } else {
                if (respuesta.data.msg != null && respuesta.data.msg != ""){
                    resp.error= respuesta.data.msg;
                }
            }
        }).catch((error) => {
            resp.status = APIStatus.ERR;
            resp.error = error.toString();
        });
        return resp;
    }


    async getRaw(ruta:string, id="") {
        let dato="";
        const resp = new APIResponse();
        if (id!="") {
            dato = "/" + id;
        }
        await http.get(
            ruta + dato
        ).then((respuesta) => {
            resp.status=APIStatus.OK;
            resp.respuesta=respuesta.data;
        }).catch((error) => {
            resp.status=APIStatus.ERR;
            resp.error = error.toString();
        });
        return resp;
    }

    async post(ruta: string, dato: any) {
        const resp = new APIResponse();
    
        await http.post(
            ruta, dato
        ).then((respuesta) => {
            resp.status = APIStatus.OK;
            resp.respuesta = respuesta.data;
        }).catch((error) => {
            resp.status = APIStatus.ERR;
            resp.error = error.toString();
        });
    
        return resp;
    }
    

    async delete(ruta: string, id: any) {
        const resp = new APIResponse();
    
        await http.delete(
            ruta+'/'+id
        ).then((respuesta) => {
            resp.status = APIStatus.OK;
            resp.respuesta = respuesta.data;
        }).catch((error) => {
            resp.status = APIStatus.ERR;
            resp.error = error.toString();
        });
    
        return resp;
    }

    async putVisible(ruta: string, list: any) {
        const resp = new APIResponse();
        const nuevaLista = {...list};
        nuevaLista.visible = list.visible == false ? "S" : "N"; // Cambio el list.visible al revés
        await http.put(
            ruta+'/'+ list.id,
            nuevaLista
        ).then((respuesta) => {
            resp.status = APIStatus.OK;
            resp.respuesta = respuesta.data;
        }).catch((error) => {
            resp.status = APIStatus.ERR;
            resp.error = error.toString();
        });
    
        return resp;
    }


    async putLista(ruta: string, list: any) {
        const resp = new APIResponse();
        const nuevaLista = {...list};
        nuevaLista.visible = list.visible == true ? "S" : "N"; // Cambio el list.visible al revés
        await http.put(
            ruta+'/'+ list.id,
            nuevaLista
        ).then((respuesta) => {
            resp.status = APIStatus.OK;
            resp.respuesta = respuesta.data;
        }).catch((error) => {
            resp.status = APIStatus.ERR;
            resp.error = error.toString();
        });
    
        return resp;
    }

    // Valida el usuario
    async login(ruta: string, credentials: { email: string; password: string }) {
        const resp = new APIResponse();
        await http.post(
            ruta, credentials
        ).then((respuesta) => {
            const data = respuesta.data;
            resp.status = APIStatus.OK;
            resp.respuesta = data; // Aquí puedes manejar la respuesta del servidor
            console.log(data);
            if (data.usuario.rol === 'admin') {
                console.log("Es admin");
                window.location.href = '/Usuarios'; // Redirige a Razor Pages
            } else if (data.usuario.rol === 'user') {
                console.log("Es user");
                router.push('/reserva'); // Navega dentro de la SPA de Vue
            }
        }).catch((error) => {
            resp.status = APIStatus.ERR;
            resp.error = error.toString();
        });
        return resp;
    }
    


}
export default new APIService();