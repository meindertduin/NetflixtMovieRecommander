<template>
  <div>
    <v-card max-width="300" height="700">
      <v-card-title class="justify-center">{{title}}</v-card-title>
      <v-img :src="url""></v-img>
      <v-card-subtitle class="pb-0"><h4>{{type}}</h4></v-card-subtitle>
      <v-card-subtitle class="pb-0">
        <h4>{{genres}}</h4>
      </v-card-subtitle>
      <v-card-text>{{plot}}</v-card-text>
    </v-card>
  </div>
</template>

<script lang="ts">
  import {Prop, Vue, Component} from "nuxt-property-decorator";

  @Component({
    props: ['url', 'plot', 'title', 'genres', 'type']
  })
  export default class RecommendationDisplay extends Vue{


  }
</script>


