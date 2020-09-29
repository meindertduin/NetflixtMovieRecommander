<template>
  <v-container>
    <v-card min-height="400">
      <v-card-title class="justify-center">{{recommendation.title}}</v-card-title>
      <v-row justify="center">
        <v-img class="justify-center" max-width="500" :src="recommendation.poster"></v-img>
      </v-row>
      <v-card-subtitle class="pb-0"><h4>{{recommendation.type}}</h4></v-card-subtitle>
      <v-card-subtitle class="pb-0">
        <h4>{{recommendation.genresString}}</h4>
      </v-card-subtitle>
      <v-card-text>{{recommendation.plot}}</v-card-text>
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

    private async addAlreadyWatched(): Promise<void> {
      const groupId = this.$route.params.id;
      await this.$store.dispatch('watchgroup/addAlreadyWatchedItem', {
        groupId: groupId,
        title: this.recommendation.title,
      })
    }
  }
</script>

