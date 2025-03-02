import axios from "axios";
import env from "@/env.json";
export enum APIStatus {
    OK = '0',
    ERR = '1',
}
export class APIResponse {
    public respuesta: any;
    public status!: APIStatus;
    public error!: string;
}
export default axios.create({
    baseURL: env.url_api,
    headers: {
        "Content-type": "application/json"
    }
});