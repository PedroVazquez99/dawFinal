import moment from 'moment';

export default class ItemTask {
    id=0;
    texto="";
    fecha:moment.Moment=moment();
    caduca:moment.Moment=moment();
    terminada=false;
    visible=true
}