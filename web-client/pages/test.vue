<template>
    <div>
      <v-btn @click="login">Login</v-btn>
    </div>
</template>

<script lang="ts">
  import {Component, Vue} from "nuxt-property-decorator";

    @Component({})
    export default class Test extends Vue{
        private login(){
          return this.$auth.signinRedirect()
        }
    }
</script>

<style scoped>

</style>
