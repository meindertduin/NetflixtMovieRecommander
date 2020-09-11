<template>
  <div>
    Hell world
  </div>
</template>

<script lang="ts">
  import {Component, Vue} from "nuxt-property-decorator";
    import {User} from "oidc-client";

    @Component({})
    export default class SignInCallback extends Vue{
      async created() {
        const {code, scope, session_state, state} = this.$route.query;

        if (code && scope && session_state && state) {
          await this.$auth.signinRedirectCallback()
            .then((user: User) => {
              console.log(user);
              this.$store.commit('auth/SET_USER', user);
              this.$store.commit('auth/SET_PROFILE', user.profile);
              this.$router.push('/');
            })
        }
      }
    }
</script>

<style scoped>

</style>
