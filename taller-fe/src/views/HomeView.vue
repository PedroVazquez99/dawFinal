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

    <!-- Cards Section -->
    <section class="cards-section py-5 bg-light">
      <div class="container">
        <div class="row">
          <div class="col-md-4">
            <div class="card shadow-sm p-3">
              <h3 class="card-title">Confianza</h3>
              <p class="card-text">Construimos relaciones basadas en la transparencia y el respeto.</p>
            </div>
          </div>
          <div class="col-md-4">
            <div class="card shadow-sm p-3">
              <h3 class="card-title">Profesionalismo</h3>
              <p class="card-text">Realizamos tareas con dedicación y cuidado, asegurando resultados de calidad.</p>
            </div>
          </div>
          <div class="col-md-4">
            <div class="card shadow-sm p-3">
              <h3 class="card-title">Satisfacción</h3>
              <p class="card-text">Nos enfocamos en superar las expectativas de nuestros clientes.</p>
            </div>
          </div>
        </div>
      </div>
    </section>
  </div>
</template>

<script lang="ts">
import { Component, Vue } from 'vue-property-decorator';

@Component
export default class HomeView extends Vue {
  
  get isAuthenticated() {
    return this.$store.getters.isAuthenticated;
  } 

  async handleReserva() {
    try {

    await this.$store.dispatch("fetchCurrentUser");

      if (this.isAuthenticated) {
        this.$router.push('/mi-reserva');
      } else {
        this.$router.push('/login');
      }
    } catch (error) {
        console.error("Error al verificar el usuario:", error);
        this.$router.push('/login');
    }
  }
}
</script>

<style scoped>
.hero {
  background-image: url('../assets/fondo.jpg'); 
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
  font-family: 'Georgia', serif; 
  font-size: 4rem; 
  color: rgb(49, 49, 47);
}

.hero-subtitle {
  font-size: 1rem; 
  color: rgb(0, 0, 0); 
}
</style>
