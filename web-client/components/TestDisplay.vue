﻿<template>
  <v-container>
  <v-card max-width="300" height="700">
    <v-card-title class="justify-center">{{title}}</v-card-title>
    <v-img :src="poster"></v-img>
    <v-card-subtitle class="pb-0"><h4>{{type}}</h4></v-card-subtitle>
    <v-card-subtitle class="pb-0">
      <h4>{{genresString}}</h4>
    </v-card-subtitle>
    <v-card-text>{{plot}}</v-card-text>
  </v-card>
</v-container>
</template>

<script lang="ts">
  import {Component, Vue} from "nuxt-property-decorator";

  @Component({})
  export default class TestDisplay extends Vue{
    private poster = "https://m.media-amazon.com/images/M/MV5BMGE4YzY4NGEtOWYyYS00ZDk2LWExMmMtZDIyODhiMmNlMGE0XkEyXkFqcGdeQXVyMTkxNjUyNQ@@._V1_SX300.jpg";
    private plot = "The trials and tribulations of criminal lawyer Jimmy McGill in the time before he established his strip-mall law office in Albuquerque, New Mexico.";
    private title = "Better Call Saul";
    private genres = ["Crime", "Drama"];
    private type = "series";

    private genresString = "";
    created(){
      for (let i = 0; i < this.genres.length; i++){
        this.genresString += (this.genres[i] + ', ')
      }
    }
  }
</script>


