<template>
    
    <div class="container">
        
    </div>
    
</template>
<script lang="ts">
    import { Component, Vue } from 'vue-property-decorator';
    import { Otros } from '@/mocks/otros.mock';
    @Component
    export default class OtrosComponent extends Vue {
        let objMock: {titulo: string, url: string}[] = Otros;
        
    }
</script>