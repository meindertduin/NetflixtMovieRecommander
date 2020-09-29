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
  import {Component, Vue, Watch} from "nuxt-property-decorator";
  import {genresOptions, typesOptions} from "~/assets/shared-variables";

  @Component({})
  export default class OptionsBar extends Vue{
    private genres:Array<string> = genresOptions;

    private types:Array<string> = typesOptions;

    private index:number = 0;
    @Watch("index")
    onIndexChanged(value: number, oldValue: number) {
      this.$store.commit('recommendation/SET_INDEX', value);
    }

    private selectedGenres:Array<string> = [];
    @Watch("selectedGenres")
    onSelectedGenresChange(value: Array<string>, oldValue: Array<string>) {
      this.$store.commit('recommendation/SET_GENRES', value);
    }

    private selectedType:string = "both";
    @Watch("selectedType")
    onSelectedTypeChange(value: string, oldValue: string) {
      this.$store.commit('recommendation/SET_TYPE', value);
    }

    private getRecommendations() {
      this.index = 0;

      this.$store.commit('recommendation/RESET_ALREADY_LOADED');
      this.$store.commit('recommendation/SET_INDEX', 0);
      const watchedItems = this.$store.getters['watchlist/getWatchedItems']

      this.$store.dispatch('recommendation/GetRecommendations', {watchedItems, reset: true})
        .then(() => {
          this.index++;
        })
        .catch(() => {
          // todo implement way to notify if failed
        })
    }

  }
</script>
