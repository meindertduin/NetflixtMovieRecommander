<template>
  <v-card>
    <v-card-title>
      Watch History Upload
    </v-card-title>
    <v-card-text>
      Fill in here your watchlist.csv file. <a @click="toggleUploadGuide">Click here</a> to see how to obtain it.
      <v-row justify="start">
        <div class="orange--text mx-3">{{statusMessage}}</div>
      </v-row>
      <v-row justify="start">
        <div v-if="uploadCount > 0" class="White--text mx-3">{{uploadCount}}: files succesfully uploaded</div>
      </v-row>
    </v-card-text>
    <v-card-actions>
      <v-file-input accept=".csv" label="File input" multiple v-model="watchLists" prepend-icon="mdi-paperclip"></v-file-input>
      <v-btn @click="handleFileUpload" color="green" text>Upload</v-btn>
      <v-btn @click="toggleOverlay" color="white" text>Close</v-btn>
    </v-card-actions>
  </v-card>
</template>

<script lang="ts">
  import {Component, Vue} from "nuxt-property-decorator";
  import {watchlist} from "~/store/watchlist";

    @Component({})
    export default class WatchlistUpload extends Vue{
      private watchLists:any = {};

      created(){
        this.$store.commit('watchlist/SET_UPLOAD_COUNT', 0);
      }

      get uploadCount():number{
        return (this.$store.state.watchlist as watchlist).uploadCount;
      }

      private toggleOverlay():void{
        this.$store.commit('watchlist/TOGGLE_OVERLAY');
      }

      private toggleUploadGuide():void{
        this.$store.commit('watchlist/TOGGLE_GUIDE');
      }

      private statusMessage:string = "";


      private async handleFileUpload():Promise<void>{
        this.statusMessage = "";
        if(Object.keys(this.watchLists).length === 0 && this.watchLists.constructor === Object) {
          this.statusMessage = "Please select a file"
          return;
        }

        let form:FormData = new FormData();
        let count:number = 0;
        for (let files in this.watchLists){
          form.append('watchlists', this.watchLists[count]);
          count++;
        }
        this.$store.dispatch('watchlist/uploadWatchLists', {form, count})
        .then(() => {
          this.statusMessage = "Upload Succeeded";
        })
        .catch(err => {
          console.log(err);
          this.statusMessage = "something went wrong while processing your watchlist, did you upload the right file?";
          this.watchLists = {};
        });
      }



    }
</script>

