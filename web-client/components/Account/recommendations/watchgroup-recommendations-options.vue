<template>
  <div>
    <v-row justify="start">
      <v-col>
        <v-select height="30" :items="genres" v-model="selectedGenres" label="Select Genres" multiple deletable-chips chips></v-select>
      </v-col>
      <v-col>
        <v-select height="30" :items="types" v-model="selectedType" label="Select Type"></v-select>
      </v-col>
      <v-col>
        <v-btn @click="getRecommendations">Confirm</v-btn>
      </v-col>
    </v-row>
  </div>
</template>

<script lang="ts">
  import {Component, Vue, Watch} from "nuxt-property-decorator";
  import {genresOptions, typesOptions} from "~/assets/shared-variables";

    @Component({})
    export default class WatchGroupRecommendationsOptions extends  Vue{
      private index:number = 0;
      @Watch("index")
      onIndexChanged(value: number, oldValue: number) {
        this.$store.commit('watchgroup/SET_RECOMMENDATIONS_INDEX', value);
      }

      private genres:Array<string> = genresOptions;

      private types:Array<string> = typesOptions;


      private selectedGenres:Array<string> = [];
      @Watch("selectedGenres")
      onSelectedGenresChange(value: Array<string>, oldValue: Array<string>) {
        this.$store.commit('watchgroup/SET_SELECTED_GENRES', value);
      }

      private selectedType:string = "both";
      @Watch("selectedType")
      onSelectedTypeChange(value: string, oldValue: string) {
        this.$store.commit('watchgroup/SET_SELECTED_TYPE', value);
      }


      private getRecommendations() {
        this.index = 0;

        this.$store.commit('watchgroup/RESET_ALREADY_LOADED');
        this.$store.commit('watchgroup/SET_RECOMMENDATIONS_INDEX', 0);
        this.$store.dispatch('watchgroup/getRecomemendations', {route: this.$route.params.id, reset: true})
          .then(() => {
            this.index++;
          })
          .catch(() => {
            // todo implement way to notify if failed
          })
      }
    }
</script>

<style scoped>

</style>
