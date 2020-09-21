<template>
  <div>
    <v-container>
      <v-row justify="center">
        <OptionsBar></OptionsBar>
      </v-row>
      <v-row>
        <v-btn width="80" @click="previousRecommendations">Previous</v-btn>
        <v-divider></v-divider>
        <v-btn width="80" @click="nextRecommendations">Next</v-btn>
      </v-row>
      <v-row justify="center">
        <v-col v-for="x in currentRecommendationsDisplay" :key="x.id">
          <RecommendedDisplay :title="x.title" :plot="x.plot" :type="x.type" :genres="x.genres" :poster="x.poster"  />
        </v-col>
      </v-row>
    </v-container>
    <v-overlay v-if="overlayActive">
      <UploadGuide v-if="uploadGuideActive" />
      <WatchlistUpload v-else />
    </v-overlay>
  </div>

</template>

<script lang="ts">
  import {Component, Vue} from "nuxt-property-decorator";
  import OptionsBar from "~/components/options-bar.vue";
  import {watchlist} from "~/store/watchlist";
  import WatchlistUpload from "~/components/watchlist-upload.vue";
  import {recommendation} from "~/store/recommendation";
  import RecommendedDisplay from "~/components/recommended-display.vue";
  import UploadGuide from "~/components/upload-guide.vue";

    @Component({
      name: "recommended",
      components: {
        UploadGuide,
        OptionsBar,
        RecommendedDisplay,
        WatchlistUpload,
      },
    })
    export default class Recommended extends Vue{
      get overlayActive(){
        return (this.$store.state.watchlist as watchlist).overlayActive
      }

      get uploadGuideActive(){
        //return true;
        return (this.$store.state.watchlist as watchlist).showGuide;
      }

      get currentRecommendationsDisplay(){
        return (this.$store.state.recommendation as recommendation).currentLoadedRecommendations;
      }

      get currRecIndex(){
        return (this.$store.state.recommendation as recommendation).currentRecIndex;
      }
      get recommendations(){
        return (this.$store.state.recommendation as recommendation).recommendations;
      }

      private nextRecommendations(){
        console.log(this.currRecIndex)
        if(this.currRecIndex >= this.recommendations.length - 5){
          this.addRecommendationsToWatched();
          this.getNewRecommendations();
        }
        this.$store.commit('recommendation/INC_CURR_REC_INDEX', 5);
        this.$store.commit('recommendation/SET_CURR_RECOMMENDATIONS', {from:  this.currRecIndex, to: this.currRecIndex + 5})
      }

      private previousRecommendations(){
        console.log(this.currRecIndex)
        if(this.currRecIndex < 5){
          return;
        }

        this.$store.commit('recommendation/SET_CURR_RECOMMENDATIONS', {from:  this.currRecIndex -5, to: this.currRecIndex})
        this.$store.commit('recommendation/INC_CURR_REC_INDEX', -5);
      }

      private addRecommendationsToWatched(){
        this.$store.commit('watchlist/ADD_TO_WATCHED_ITEMS', this.$store.state.recommendation.recommendations);
      }

      private async getNewRecommendations(){
        const watchedItems = this.$store.getters['watchlist/getWatchedItems'];
        const genres = this.$store.getters['recommendation/getSelectedGenres'];
        const type = this.$store.getters['recommendation/getSelectedType'];
        await this.$store.dispatch('recommendation/GetRecommendations', {watchedItems, genres: genres, type: type })
      }
    }
</script>

