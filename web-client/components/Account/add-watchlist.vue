<template>
    <div>
      <v-row justify="start">
        <div class="orange--text mx-3">{{uploadMessage}}</div>
      </v-row>
      <v-row justify="start">
        <div v-if="uploadCount > 0" class="White--text mx-3">{{uploadCount}}: files succesfully uploaded</div>
      </v-row>
      <v-file-input accept=".csv" label="User Netflix Watch History (optional)" v-model="watchList" prepend-icon="mdi-paperclip"></v-file-input>
      <v-row class="ma-5">
        <v-btn color="green" @click="addWatchlist" >Add Watchlist</v-btn>
      </v-row>
    </div>
</template>

<script lang="ts">
  import {Component, Prop, Vue} from "nuxt-property-decorator";

    @Component({})
    export default class AddWatchlist extends Vue{

      @Prop({type: String, required: true}) readonly groupId!:string

      private watchList: any = {};
      private uploadMessage:string = "";
      private uploadCount: number = 0;

      private async addWatchlist() {
        if (Object.keys(this.watchList).length === 0 && this.watchList.constructor === Object || this.groupId.length === 0) return;

        const form = new FormData;

        form.append('WatchList', this.watchList);
        form.append('WatchGroupId', this.groupId);
        this.watchList = {};

        await this.$axios({
          method: 'post',
          url: 'api/watchgroup/watchlist-upload',
          data: form,
          headers: {'Content-Type': `multipart/form-data`}
        }).then((response) => {
          this.uploadMessage = "Upload Succeeded"
          this.uploadCount += 1;
        }).catch(err => {
          console.log(err);
          this.uploadMessage = "Something went wrong uploading your watchlist."
        })
      }
    }
</script>

<style scoped>

</style>
