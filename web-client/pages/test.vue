<template>
    <div>
      <v-btn @click="login">Login</v-btn>
    </div>
</template>

<script lang="ts">
  import {Component, Vue} from "nuxt-property-decorator";
    import {UserManager, WebStorageStateStore} from "oidc-client";

    @Component({})
    export default class Test extends Vue{
        private login(){
          return this.$auth.signinRedirect()
        }

        created(){
          if(!process.server){
            const {code, scope, session_state, state } = this.$route.query;

            if(code && scope && session_state && state){
              this.$auth.signinRedirectCallback()
              .then(user =>{
                console.log(user);
                this.$router.push('/');
              })
            }
          }
        }
    }
</script>

<style scoped>

</style>
