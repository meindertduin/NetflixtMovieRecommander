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
  import {WatchGroupRecommendationForm} from "~/assets/interface-models";
  import WatchGroup from "~/components/Account/watch-group.vue";

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

      created(){
        const seed = this.uuidv4();
        this.$store.commit('watchgroup/SET_SEED', seed);
      }

      private async getRecommendations() {
        const promise = this.$store.dispatch('watchgroup/getRecomemendations', {route: this.$route.params.id})
          .then(() => {
            console.log("loading done")
          }).catch(() => {
            // todo implement way to notify if failed
          })
        this.index++;
      }

      private uuidv4() {
        return 'xxxxxxxx-xxxx-4xxx-yxxx-xxxxxxxxxxxx'.replace(/[xy]/g, function(c) {
          let r = Math.random() * 16 | 0, v = c == 'x' ? r : (r & 0x3 | 0x8);
          return v.toString(16);
        });
      }
    }
</script>

<style scoped>

</style>
