<template>
  <v-container>
    <v-card min-height="400" class="display-card">
      <v-card-title class="justify-center display-card-title">{{recommendation.title}}</v-card-title>
      <v-row justify="center" class="display-card-image">
        <v-img class="justify-center" max-width="500" :src="recommendation.poster"></v-img>
      </v-row>
      <div class="display-card-body mt-2">
        <div class="subtitle-2 orange--text mx-2">{{recommendation.type}}</div>
        <div class="subtitle-2 orange--text mx-2">{{genresString}}</div>
        <v-card-text class="display-card-text">{{recommendation.plot}}</v-card-text>
      </div>
    </v-card>
  </v-container>
</template>

<script lang="ts">
  import {Component, Prop, Vue} from "nuxt-property-decorator";
  import {Recommendation} from "~/assets/interface-models";

    @Component({})
    export default class RecommendedDisplay extends Vue {
      @Prop({type: Object, required: true}) readonly recommendation !: Recommendation;

      private genresString = "";

      created(){
        console.log("happens");
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
        console.log(this.genresString)
      }
    }
</script>

<style scoped>
  .display-card{
    height: 800px;
  }
  .display-card-text{
    height: 150px;
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
