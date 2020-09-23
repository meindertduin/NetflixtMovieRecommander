<template>
  <v-app dark>
    <v-app-bar app dense>
      <v-toolbar-title>Netflix recommander</v-toolbar-title>
      <v-divider></v-divider>
      <v-skeleton-loader :loading="loadingState" transition="fade-transition" type="button">
        <v-btn v-if="authenticated">
          <v-icon>mdi-account-circle</v-icon>Profile
        </v-btn>
        <v-btn v-else @click="$auth.signinRedirect()">
          <v-icon>mdi-account-circle</v-icon>Sign in
        </v-btn>
      </v-skeleton-loader>
      <v-btn v-if="authenticated" @click="$auth.signoutRedirect()">
        <v-icon>mdi-account-circle</v-icon>Sign Out
      </v-btn>
    </v-app-bar>
    <v-main>
      <nuxt />
    </v-main>
  </v-app>
</template>

<script lang="ts">
  import {Component, Vue} from "nuxt-property-decorator";
  import {auth} from "~/store/auth";

@Component({})
export default class DefaultLayout extends Vue
{
  get loadingState(){
    return (this.$store.state.auth as auth).loading;
  }

  get authenticated(){
    return this.$store.getters['auth/authenticated'];
  }
  
  get getModState(){
    return this.$store.getters['auth/moderator'];
  }
}
</script>
