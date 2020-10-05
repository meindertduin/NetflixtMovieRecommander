<template>
  <v-container>
    <v-card class="display-card">
      <v-card-title class="justify-center display-card-title">{{recommendation.title}}</v-card-title>
      <v-row class="display-card-image" justify="center">
        <v-img class="justify-center" max-width="300" contain :src="recommendation.poster"></v-img>
      </v-row>
      <div class="display-card-body">
        <v-card-subtitle class="pb-0"><h4>{{recommendation.type}}</h4></v-card-subtitle>
        <v-card-subtitle class="pb-0">
          <h4>{{recommendation.genresString}}</h4>
        </v-card-subtitle>
        <v-card-text class="display-card-text">
          <div>
          {{recommendation.plot}}
          </div>
        </v-card-text>
      </div>
      <v-card-actions>
        <v-row justify="center">
          <v-btn color="orange" text @click="addAlreadyWatched">Already Watched</v-btn>
        </v-row>
      </v-card-actions>
    </v-card>
  </v-container>
</template>

<script lang="ts">
  import {Component, Prop, Vue} from "nuxt-property-decorator";
  import {Recommendation} from "~/assets/interface-models";

  @Component({})
  export default class WatchgroupRecommendedDisplay extends Vue {
    @Prop({type: Object, required: true}) readonly recommendation !: Recommendation;

    private genresString = "";

    created(){
      if(this.recommendation.genres.length > 0){
        const genresArray = this.recommendation.genres.split(',');
        genresArray.forEach((genre, index) => {
          if(index != genresArray.length -1){
            this.genresString += (genre + ', ')
          }
          else{
            this.genresString += (genre)
          }
        })
      }
    }

    private addAlreadyWatched(): void {
      const groupId = this.$route.params.id;
      this.$store.dispatch('watchgroup/addAlreadyWatchedItem', {
        groupId: groupId,
        title: this.recommendation.title,
      })
        .then(() => this.$store.commit('watchgroup/REMOVE_RECOMMENDATION_FROM_DISPLAY', this.recommendation))
        .catch(err => console.log(err));
    }
  }
</script>

<style scoped>
  .display-card{
    height: 800px;
  }
  .display-card-text{
    height: 100px;
    overflow: auto;
    padding: 10px;
  }

  .display-card-title{
    height: 60px;
  }

  .display-card-image{
    height: 500px;
  }

  .display-card-text::-webkit-scrollbar {
    width: 12px;
  }

  .display-card-text::-webkit-scrollbar-track {
    background-color: black;
    border-radius: 10px;
  }

  .display-card-text::-webkit-scrollbar-thumb {
    border-radius: 5px;
    height: 2px;
    background-color:#171a1d;
    -webkit-box-shadow: inset 0 0 6px rgba(90,90,90,0.7);
  }
</style>
