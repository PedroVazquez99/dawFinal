import { Idiomas } from "./langs";

import es from "./es.json";
import en from "./en.json";

export const messages = {
    [Idiomas.ES]: es,
    [Idiomas.EN]: en
};
export const defaultLang = Idiomas.EN;