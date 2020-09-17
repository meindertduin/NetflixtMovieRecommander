<template>
  <v-card>
    <v-card-title>
      Watch History Upload
    </v-card-title>
    <v-card-subtitle v-if="errorMessage.length > 0">
      {{errorMessage}}
    </v-card-subtitle>
    <v-card-text>
      Fill in here your watchlist.csv file. <a @click="toggleUploadGuide">Click here</a> to see how to obtain it.
    </v-card-text>
    <v-card-actions>
      <v-file-input accept=".csv" label="File input" multiple v-model="watchLists" prepend-icon="mdi-paperclip"></v-file-input>
      <v-btn @click="handleFileUpload">Upload</v-btn>
    </v-card-actions>
  </v-card>
</template>

<script lang="ts">
  import {Component, Vue} from "nuxt-property-decorator";

    @Component({})
    export default class WatchlistUpload extends Vue{

      private watchLists:any = {};

      private toggleOverlay():void{
        this.$store.commit('watchlist/TOGGLE_OVERLAY');
      }

      private toggleUploadGuide():void{
        this.$store.commit('watchlist/TOGGLE_GUIDE');
      }

      private errorMessage:string = "";


      private async handleFileUpload():Promise<void>{
        if(Object.keys(this.watchLists).length === 0 && this.watchLists.constructor === Object) return;
        console.log(this.watchLists)
        let form:FormData = new FormData();

        let count:number = 0;
        for (let files in this.watchLists){
          form.append('watchlists', this.watchLists[count]);
          count++;
        }

        const res:number = await this.$store.dispatch('watchlist/uploadWatchLists', {form});

        if(res != 200 && res < 500){
          this.errorMessage = "something went wrong while processing your watchlist, did you upload the right file?";
          this.watchLists = {};
          return;
        }
        this.toggleOverlay();
      }

    }
</script>

