﻿﻿<template>
  <v-container>
  <v-card>
    <v-card-title class="justify-center">{{title}}</v-card-title>
    <v-row justify="center">
      <v-img class="justify-center" max-width="500" :src="poster"></v-img>
    </v-row>
    <v-card-subtitle class="pb-0"><h4>{{type}}</h4></v-card-subtitle>
    <v-card-subtitle class="pb-0">
      <h4>{{genresString}}</h4>
    </v-card-subtitle>
    <v-card-text>{{plot}}</v-card-text>
    <v-card-actions>
      <v-row justify="center">
        <v-btn color="orange" text>Already Watched</v-btn>
      </v-row>
    </v-card-actions>
  </v-card>
</v-container>
</template>

<script lang="ts">
  import {Component, Vue} from "nuxt-property-decorator";

  @Component({})
  export default class TestDisplay extends Vue{
    private poster:string | null = "https://m.media-amazon.com/images/M/MV5BMGE4YzY4NGEtOWYyYS00ZDk2LWExMmMtZDIyODhiMmNlMGE0XkEyXkFqcGdeQXVyMTkxNjUyNQ@@._V1_SX300.jpg";
    private plot:string | null= "The trials and tribulations of criminal lawyer Jimmy McGill in the time before he established his strip-mall law office in Albuquerque, New Mexico.";
    private title:string | null = "Better Call Saul";
    private genres:Array<string> | null = ["Crime", "Drama"];
    private type:string = "series";

    private genresString = "";
    created(){
      if(this.genres != null){
        for (let i = 0; i < this.genres.length; i++){
          this.genresString += (this.genres[i] + ', ')
        }
      }
    }
  }
</script>


