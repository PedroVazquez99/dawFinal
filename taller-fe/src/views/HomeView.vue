<template>
  <div class="home">
    <!-- Hero Section -->
    <section class="hero d-flex align-items-center justify-content-center">
      <div class="hero-content text-center w-100">
        <div class="blurred-band px-4 py-3">
          <h1 class="display-3 title">Peluquería</h1>
          <h2 class="hero-subtitle fs-6">Cuidamos tu estilo con profesionalismo y dedicación</h2>
        </div>
        <button class="btn btn-primary btn-lg mt-4" @click="handleReserva">Reserva Ahora</button>
      </div>
    </section>

    <!-- Servicios Section -->
    <section class="services-section py-5 bg-light">
      <div class="container">
        <h2 class="services-title text-center mb-4">Nuestros Servicios</h2>
        <div class="services-carousel">
          <div
            v-for="(servicio, index) in servicios"
            :key="index"
            class="service-card shadow-sm p-3"
          >
          
            <p class="card-text">{{ servicio.descripcion }}</p>
            <p class="card-price">{{ servicio.precio }} €</p>
          </div>
        </div>
      </div>
    </section>
  </div>
</template>

<script lang="ts">
import { Component, Vue } from "vue-property-decorator";

@Component
export default class HomeView extends Vue {
  servicios: Array<{ titulo: string; descripcion: string; precio: number }> = [];

  async mounted() {
    try {
      // Llama a la acción para obtener los servicios desde el store
      await this.$store.dispatch("fetchServicios");
      this.servicios = this.$store.state.servicios;
    } catch (error) {
      console.error("Error al cargar los servicios:", error);
    }
  }

  get isAuthenticated() {
    return this.$store.getters.isAuthenticated;
  }

  async handleReserva() {
    try {
      await this.$store.dispatch("fetchCurrentUser");

      if (this.isAuthenticated) {
        this.$router.push("/mi-reserva");
      } else {
        this.$router.push("/login");
      }
    } catch (error) {
      console.error("Error al verificar el usuario:", error);
      this.$router.push("/login");
    }
  }
}
</script>

<style scoped>
.hero {
  background-image: url("../assets/fondo.jpg");
  background-size: cover;
  background-position: center;
  height: 100vh;
  width: 100%;
  display: flex;
  align-items: center;
  justify-content: center;
}

.hero-content {
  text-align: center;
  color: black;
  width: 100%;
}

.blurred-band {
  background: rgba(255, 255, 255, 0.4);
  backdrop-filter: blur(3px);
  border-radius: 0;
  width: 100%;
  text-align: center;
}

.title {
  font-family: "Georgia", serif;
  font-size: 4rem;
  color: rgb(49, 49, 47);
}

.hero-subtitle {
  font-size: 1rem;
  color: rgb(0, 0, 0);
}

.services-title {
  font-family: "Georgia", serif; /* Igual que el título "Peluquería" */
  font-size: 2.5rem;
  color: rgb(49, 49, 47);
}

.services-carousel {
  display: flex;
  gap: 1rem;
  padding: 1rem 0;
  overflow: hidden; /* Oculta el slider horizontal */
  position: relative;
}

.services-carousel::before,
.services-carousel::after {
  content: "";
  position: absolute;
  top: 0;
  bottom: 0;
  width: 50px;
  background: linear-gradient(to right, #f8f9fa, rgba(248, 249, 250, 0));
  z-index: 1;
}

.services-carousel::before {
  left: 0;
}

.services-carousel::after {
  right: 0;
  transform: rotate(180deg);
}

.service-card {
  flex: 0 0 auto;
  width: 300px;
  background: white;
  border-radius: 8px;
  text-align: center;
  transition: transform 0.3s ease;
  box-shadow: 0 4px 6px rgba(0, 0, 0, 0.1);
  animation: float 10s linear infinite; /* Animación flotante */
}

.service-card:hover {
  transform: scale(1.05);
}

.card-title {
  font-size: 1.5rem;
  font-weight: bold;
}

.card-text {
  font-size: 1rem;
  margin: 0.5rem 0;
}

.card-price {
  font-size: 1.2rem;
  font-weight: bold;
  color: #000000;
}

/* Animación flotante */
@keyframes float {
  0% {
    transform: translateX(0);
  }
  100% {
    transform: translateX(-100%);
  }
}
</style>
