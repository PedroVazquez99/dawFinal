<template>
    <footer class="border-top footer text-muted">
        <div class="container">
            &copy; 2022 -
            <router-link to="/about">
                {{$t('app.acerca_de')}} {{$t('app.titulo')}}
            </router-link>
            <small>(Ver. {{ env.ver }})</small>
        </div>
    </footer>
</template>
<script lang="ts">
    import { Component, Vue } from 'vue-property-decorator';
    import env from "@/env.json"
    
    @Component
    export default class Foot extends Vue {
        env: any = env;
    }
</script>