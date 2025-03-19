<template>
  <div class="login-container">
    <h2>Login</h2>
    <form @submit.prevent="handleLogin">
      <div class="form-group">
        <label for="username">Mail</label>
        <input type="text" id="username" v-model="email" required />
      </div>
      <div class="form-group">
        <label for="password">Password</label>
        <input type="password" id="password" v-model="password" required />
      </div>
      <button type="submit">Login</button>
    </form>
    <p v-if="error">{{ error }}</p>
  </div>
</template>

<script lang="ts">
import { Component, Vue } from "vue-property-decorator";

@Component
export default class LoginComponent extends Vue {
  private email = "";
  private password = "";
  private error = "";

  // Método para manejar el login
  private handleLogin() {
    this.$store
      .dispatch("login", { email: this.email, password: this.password }) // Llama a la acción 'login' del store
      .then(() => {
        // Redirige al usuario a la página principal después de un login exitoso
        this.$router.push({ name: "Home" });
      })
      .catch((err: Error) => {
        // Muestra el error si ocurre un problema durante el login
        this.error = err.message;
      });
  }
}
</script>

<style scoped>
.login-container {
  max-width: 400px;
  margin: 0 auto;
  padding: 20px;
  border: 1px solid #ccc;
  border-radius: 4px;
}

.form-group {
  margin-bottom: 15px;
}

.form-group label {
  display: block;
  margin-bottom: 5px;
}

.form-group input {
  width: 100%;
  padding: 8px;
  box-sizing: border-box;
}

button {
  width: 100%;
  padding: 10px;
  background-color: #007bff;
  color: white;
  border: none;
  border-radius: 4px;
  cursor: pointer;
}

button:hover {
  background-color: #0056b3;
}

p {
  color: red;
}
</style>