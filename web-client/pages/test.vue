<template>
    <div>
      <v-btn @click="test">Test</v-btn>
      <v-btn @click="testMod">Mod</v-btn>
      <v-btn @click="login">Login</v-btn>
      <v-btn @click="logout">Logout</v-btn>
    </div>

</template>

<script lang="ts">
  import {Component, Vue} from "nuxt-property-decorator";
  import {auth} from "~/store/auth";
  import {UserManager, WebStorageStateStore} from "oidc-client";

    @Component({})
    export default class Test extends Vue{
        private login(){
          return this.$auth.signinRedirect();
        };
        private async test(){
          const res = await this.$axios.get('api/users/test');
          console.log(res.data);
        }
        private async testMod(){
          const res = await this.$axios.get('api/users/mod');
          console.log(res.data);
        }
        private async logout(){
          return this.$auth.signoutRedirect();
        }
    }
</script>

<style scoped>

</style>
