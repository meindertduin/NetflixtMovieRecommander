<template>

</template>

<script lang="ts">
  import {Component, Vue} from "nuxt-property-decorator";
  @Component({})
  export default class SilentSignInCallback extends Vue{
    created(){
      if(!process.server){

      }
    }
  }
</script>

<style scoped>

</style>
