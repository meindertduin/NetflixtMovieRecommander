<template>
  <v-app dark>
    <v-app-bar app dense>
      <v-toolbar-title>Netflix recommander</v-toolbar-title>
      <v-divider></v-divider>
      <div v-if="true">
        Want to save your shared watch history? <a>Click Here</a> to register or to login
      </div>
    </v-app-bar>
    <v-main>
      <nuxt />
    </v-main>
  </v-app>
</template>

<script>
export default {

}
</script>
