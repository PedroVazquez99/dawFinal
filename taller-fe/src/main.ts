import Vue from 'vue'
import './plugins/fontawesome'

import App from './App.vue'
import './registerServiceWorker'
import router from './router'
import store from './store'
import VueI18n from 'vue-i18n'
import VueSweetalert2 from 'vue-sweetalert2';
import 'sweetalert2/dist/sweetalert2.min.css';

Vue.use(VueI18n)
// eslint-disable-next-line @typescript-eslint/no-var-requires
Vue.use(require('vue-moment'))
Vue.use(VueSweetalert2);

import { messages, defaultLang } from "@/i18n";
const i18n = new VueI18n({
  messages,
  locale: 'es',
  fallbackLocale: defaultLang
});

Vue.config.productionTip = false

new Vue({
  router,
  store,
  i18n,
  render: h => h(App)
}).$mount('#app')
