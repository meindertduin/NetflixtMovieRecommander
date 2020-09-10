<template>
  <v-container>
    <v-card max-width="300" height="700">
      <v-card-title class="justify-center">{{title}}</v-card-title>
      <v-img :src="poster"></v-img>
      <v-card-subtitle class="pb-0"><h4>{{type}}</h4></v-card-subtitle>
      <v-card-subtitle class="pb-0">
        <h4>{{genres}}</h4>
      </v-card-subtitle>
      <v-card-text>{{plot}}</v-card-text>
    </v-card>
  </v-container>
</template>

<script lang="ts">
  import {Component, Vue} from "nuxt-property-decorator";

    @Component({
      props: ['poster', 'plot', 'title', 'genres', 'type']
    })
    export default class RecommendedDisplay extends Vue {

    }
</script>


