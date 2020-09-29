<template>
  <div>
    <v-container>
      <v-row justify="center">
        <OptionsBar></OptionsBar>
      </v-row>
      <v-row justify="center">
        <v-col v-for="(x, index) in currentRecommendationsDisplay" :key="index">
          <RecommendedDisplay :recommendation="x"/>
        </v-col>
      </v-row>
      <v-row justify="center">
        <v-btn width="80" @click="nextRecommendations" min-width="300">Load More</v-btn>
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
        return (this.$store.state.recommendation as recommendation).recommendations;
      }


      private nextRecommendations(){
        this.addRecommendationsToWatched();
        const watchedItems = this.$store.getters['watchlist/getWatchedItems']
        this.$store.dispatch('recommendation/GetRecommendations', {watchedItems, reset: false})
          .then(() => {
            this.$store.commit('recommendation/INC_INDEX')
            // notify user something went wrong
          })
        .catch(err => console.log(err))
      }

      private addRecommendationsToWatched(){
        this.$store.commit('watchlist/ADD_TO_WATCHED_ITEMS', this.$store.state.recommendation.recommendations);
      }
    }
</script>

