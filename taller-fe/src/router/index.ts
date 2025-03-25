import Vue from 'vue'
import VueRouter, { RouteConfig } from 'vue-router'
import HomeView from '../views/HomeView.vue'

Vue.use(VueRouter)

const routes: Array<RouteConfig> = [
  {
    path: '/',
    name: 'home',
    component: HomeView
  },
  {
    path: '/reserva',
    name: 'reserva',
    // route level code-splitting
    // this generates a separate chunk (about.[hash].js) for this route
    // which is lazy-loaded when the route is visited.
    component: () => import(/* webpackChunkName: "about" */ '../views/ReservaView.vue')
  },
  {
    path: '/login',
    name: 'login',
    beforeEnter: (to, from, next) => {
      window.location.href = '/Account/Login'; // Cambia esta URL por la de tu vista ASP.NET
    }
  },
  {
    path: '/mi-reserva',
    name: 'mi-reserva',
    // route level code-splitting
    // this generates a separate chunk (about.[hash].js) for this route
    // which is lazy-loaded when the route is visited.
    component: () => import(/* webpackChunkName: "about" */ '../views/ReservaClienteView.vue')
  }
]

const router = new VueRouter({
  routes
})

export default router
