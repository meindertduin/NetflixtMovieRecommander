<template>
  <v-layout>
    <v-flex class="d-flex justify-center mt-20">
      <v-stepper v-model="step" v-if="uploadShowHelp">
        <v-stepper-header>
          <v-stepper-step :complete="step > 1" step="1">Helper</v-stepper-step>

          <v-divider></v-divider>

          <v-stepper-step :complete="step > 2" step="2">Upload</v-stepper-step>
        </v-stepper-header>

        <v-stepper-items>

          <v-stepper-content step="1">
            <v-card>
              <v-card-title>
                Find your file here
              </v-card-title>
              <v-card-actions>
                <v-btn @click="step++">Continue</v-btn>
              </v-card-actions>
            </v-card>
          </v-stepper-content>

          <v-stepper-content step="2">
            <v-card>
              <v-card-title>
                Upload here your watchlist.csv
              </v-card-title>
              <v-card-text>
                Fill in here your watchlist.csv file.
              </v-card-text>
              <v-card-actions>
                <v-file-input accept=".csv" label="File input" multiple v-model="watchLists" prepend-icon="mdi-paperclip">
                </v-file-input>
              </v-card-actions>
            </v-card>
          </v-stepper-content>

        </v-stepper-items>
      </v-stepper>


      <v-stepper v-model="step" v-else>
        <v-stepper-header>
          <v-stepper-step :complete="step > 1" step="1">Upload csv</v-stepper-step>

          <v-divider></v-divider>

          <v-stepper-step :complete="step > 2" step="2">Options</v-stepper-step>

          <v-divider></v-divider>

          <v-stepper-step step="3">Results</v-stepper-step>
        </v-stepper-header>


        <v-stepper-items>
          <v-stepper-content step="1">
            <div>
              <div>Fill in here your watchlist.csv file. <a @click="toggleUploadShowHelp">Click here</a> to see how to obtain.</div>


              <v-file-input accept=".csv" label="File input" multiple v-model="watchLists" prepend-icon="mdi-paperclip"></v-file-input>
              <v-btn @click="handleFileUpload">Upload</v-btn>
              <v-btn @click="step++">Dev Skip</v-btn>
            </div>
          </v-stepper-content>

          <v-stepper-content step="2">
            <div>
              <div>
                Fill in extra options
              </div>
              <v-select :items="genres" v-model="selectedGenres" label="Select Genres" multiple chips deletable-chips></v-select>
              <v-select :items="types" v-model="selectedType" label="Select type"></v-select>
              <div>
                <v-btn @click="getRecommendations">Confirm</v-btn>
                <v-btn @click="step++">Dev Skip</v-btn>
              </div>
            </div>
          </v-stepper-content>

          <v-stepper-content step="3">
            <v-row>
              <v-btn width="80" @click="previousRecommendations">Previous</v-btn>
              <v-divider></v-divider>
              <v-btn width="80" @click="nextRecommendations">Next</v-btn>
            </v-row>
            <v-row>
              <v-col v-for="x in currentRecommendationsDisplay">
                <RecommendationDisplay :key="x.id" :_title="x.title" :plot="x.plot" :url="x.poster" :genres="x.genres" :type="x.type" />
              </v-col>
            </v-row>
            <v-row justify="center">
              <div>Want to save your watchhistory? Be sure to make an account <a>Here</a></div>
            </v-row>
          </v-stepper-content>

        </v-stepper-items>
      </v-stepper>
      <v-row justify="center">
        <v-btn @click="TOGGLE_OVERLAY">Cancel</v-btn>
      </v-row>
    </v-flex>
  </v-layout>
</template>

<script lang="ts">
  import {Vue, Component, Watch} from 'nuxt-property-decorator'
  import {mapMutations} from "vuex";
  import {watchlist} from "~/store/watchlist";
  import {recommendation} from "~/store/recommendation";
  import RecommendationDisplay from "~/components/RecommendationDisplay.vue";

  @Component({
    methods: {
      ...mapMutations('watchlist', ['TOGGLE_OVERLAY']),
    },
    components: {
      RecommendationDisplay
    }
  })
  export default class UploadOverlay extends Vue {
    private uploadShowHelp = false;
    private watchLists = {};
    private step = 1;

    private genres = [
      "Animation", "Action", "Documentary", "Drama", "War", "Crime",  "Mystery", "Sci-Fi", "Thriller",
    ];
    private selectedGenres = [];

    private types = ["series", "movie", "both"];
    private selectedType = "both";

    private currentRecommendationsDisplay:Array<string> = [];

    get recommendations(){
      return (this.$store.state.recommendation as recommendation).recommendations;
    }

    private toggleUploadShowHelp(){
      this.uploadShowHelp = ! this.uploadShowHelp;
    }

    private async handleFileUpload(){

      if(Object.keys(this.watchLists).length === 0 && this.watchLists.constructor === Object) return;

      const form = new FormData();
      let count = 0;
      for (const file in this.watchLists){
        form.append('file' + count, file)
        count++
      }

      await this.$store.dispatch('watchlist/uploadWatchLists', {watchLists: form});
      this.step++;
    }

    private async getRecommendations(){
      const watchedItems = this.$store.getters['watchlist/getWatchedItems'];
      const recommendations = await this.$store.dispatch('recommendation/GetRecommendations', {watchedItems, genres: this.selectedGenres, type: this.selectedType })
      this.currentRecommendationsDisplay = recommendations.slice(0, 5);
      this.step++;
    }

    private currRecIndex = 0;

    private nextRecommendations(){
      if(this.currRecIndex < this.recommendations.length - 5){
        this.currRecIndex += 5;
        this.currentRecommendationsDisplay = this.recommendations.slice(this.currRecIndex, this.currRecIndex + 5)
      }
      else{
        this.addRecommendationsToWatched();
        this.getRecommendations();
      }
    }

    private previousRecommendations(){
      console.log(this.currRecIndex)
      if(this.currRecIndex >= 5){
        this.currentRecommendationsDisplay =  this.recommendations.slice(this.currRecIndex - 5, this.currRecIndex)
        this.currRecIndex -= 5;
      }
      else{
        return;
      }
    }

    private addRecommendationsToWatched(){
      this.$store.commit('watchlist/ADD_TO_WATCHED_ITEMS', this.recommendations);
    }
  }
</script>

