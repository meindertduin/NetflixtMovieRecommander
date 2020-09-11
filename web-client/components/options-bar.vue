﻿<template>
  <div>
    <v-row justify="start" al>
      <v-col cols="7">
        <v-select height="30" :items="genres" v-model="selectedGenres" label="Select Genres" multiple deletable-chips chips></v-select>
      </v-col>
      <v-col cols="3">
        <v-select height="30" :items="types" v-model="selectedType" label="Select Type"></v-select>
      </v-col>
      <v-col cols="2">
        <v-btn @click="getRecommendations">Confirm</v-btn>
      </v-col>
    </v-row>
  </div>
</template>

<script lang="ts">
  import {Component, Vue} from "nuxt-property-decorator";

  @Component({})
  export default class OptionsBar extends Vue{
    private genres = [
      "Animation", "Action", "Documentary", "Drama", "War", "Crime",  "Mystery", "Sci-Fi", "Thriller",
    ];
    private selectedGenres = [];

    private types:Array<string> = ["series", "movie", "both"];
    private selectedType:string = "both";

    private async getRecommendations(){
      this.$store.commit('recommendation/RESET');

      const watchedItems = this.$store.getters['watchlist/getWatchedItems'];
      console.log(watchedItems);
      await this.$store.dispatch('recommendation/GetRecommendations', {watchedItems, genres: this.selectedGenres, type: this.selectedType })

      this.$store.commit('recommendation/SET_INITIAL_CURRENT_LOADED');
      this.$store.commit('recommendation/SET_TYPE', this.selectedType);
      this.$store.commit('recommendation/SET_GENRES', this.selectedGenres);
      this.$store.commit('recommendation/SET_CURR_RECOMMENDATIONS', {from: 0, to: 5});

      console.log(this.$store.state.recommendation.currentLoadedRecommendations);
      console.log(this.$store.state.recommendation.recommendations);
    }

  }
</script>
