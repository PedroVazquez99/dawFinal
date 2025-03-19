<template>
  <div class="login-view">
    <LoginComponent />
  </div>
</template>

<script lang="ts">
import { Component, Vue } from "vue-property-decorator";
import LoginComponent from "@/components/Login.vue";

@Component({
  components: {
    LoginComponent,
  },
})
export default class LoginView extends Vue {}
</script>

<style scoped>
.login-view {
  display: flex;
  justify-content: center;
  align-items: center;
  height: 100vh;
  background-color: #f5f5f5;
}
</style>