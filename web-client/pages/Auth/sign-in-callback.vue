<template>
  <div>
  </div>
</template>

<script lang="ts">
  import {Component, Vue} from "nuxt-property-decorator";
    import {User} from "oidc-client";

    @Component({})
    export default class SignInCallback extends Vue{
      created() {
        if(!process.server){
          const {code, scope, session_state, state} = this.$route.query;
          if (code && scope && session_state && state) {
            this.$auth.signinRedirectCallback()
              .then((user) => {
                console.log(user);
                this.$store.dispatch('auth/setBearer', user.access_token);
                this.$router.push('/')
              }).catch((err) => {
                console.log(err);
                // todo: add a page for handling authorization errors
              })
          }
        }
      }
    }
</script>

<style scoped>

</style>
