import ItemTask from "@/models/TaskList";
import moment from 'moment';
export default class TaskList {
  id = 0;
  nombre = "";
  color = "#FFFFFF";
  visible=true;
  fecha:moment.Moment=moment();
  UsuarioId = 0;
  ServicioId = 0;
}
