<template>
  <div>
    <v-container>
      <v-row justify="center">
        <WatchGroupRecommendationsOptions />
      </v-row>
      <v-row justify="center">
<!--        <v-col v-for="x in currentRecommendationsDisplay" :key="x.id">-->
<!--          <RecommendedDisplay :title="x.title" :plot="x.plot" :type="x.type" :genres="x.genres" :poster="x.poster"  />-->
<!--        </v-col>-->
        <v-col v-for="(x, index) in currentRecommendationsDisplay" :key="index">
          <RecommendedDisplay :recommendation="x"/>
        </v-col>
      </v-row>
      <v-row justify="center">
        <v-btn width="80" @click="nextRecommendations" min-width="300">Load More</v-btn>
      </v-row>
    </v-container>
  </div>
</template>

<script lang="ts">
  import {Component, Vue} from "nuxt-property-decorator";
  import WatchGroupRecommendationsOptions from "~/components/Account/recommendations/watchgroup-recommendations-options.vue";
  import {watchgroup} from "~/store/watchgroup";
  import RecommendedDisplay from "~/components/recommended-display.vue";
  import TestDisplay from "~/components/TestDisplay.vue";

    @Component({
      components: {
        TestDisplay,
        WatchGroupRecommendationsOptions,
        RecommendedDisplay
      }
    })
    export default class WatchGroupId extends Vue{

      get currentRecommendationsDisplay(){
        return (this.$store.state.watchgroup as watchgroup).watchGroupRecommendations;
      }

      nextRecommendations():void{
        this.$store.dispatch('watchgroup/getRecomemendations', {route: this.$route.params.id, reset: false})
        .catch(() => {
          this.$store.commit('watchgroup/INC_RECOMMENDATIONS_INDEX')
          // notify user something went wrong
        })
      }
    }
</script>

<style scoped>

</style>
