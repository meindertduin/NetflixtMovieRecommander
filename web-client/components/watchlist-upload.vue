<template>
  <v-card>
    <v-card-title>
      Watch History Upload
    </v-card-title>
    <v-card-text>
      Fill in here your watchlist.csv file. <a @click="">Click here</a> to see how to obtain it.
    </v-card-text>
    <v-card-actions>
      <v-file-input accept=".csv" label="File input" multiple v-model="watchLists" prepend-icon="mdi-paperclip"></v-file-input>
      <v-btn @click="handleFileUpload">Upload</v-btn>
    </v-card-actions>
  </v-card>
</template>

<script lang="ts">
  import {Component, Vue} from "nuxt-property-decorator";

    @Component
    export default class WatchlistUpload extends Vue{

      private toggleOverlay(){
        this.$store.commit('watchlist/TOGGLE_OVERLAY');
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
        this.toggleOverlay();
      }
      private watchLists = {};
    }
</script>

