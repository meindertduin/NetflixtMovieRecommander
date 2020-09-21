<template>
    <div>
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

      private addWatchlist(){
        if(Object.keys(this.watchList).length === 0 && this.watchList.constructor === Object || this.groupId.length === 0) return;

        const form = new FormData;

        console.log(this.watchList)
        form.append('WatchList', this.watchList);
        form.append('WatchGroupId', this.groupId);
        this.watchList = {};

        const res = this.$axios({
          method: 'post',
          url: 'api/watchgroup/watchlist-upload',
          data: form,
          headers: {'Content-Type': `multipart/form-data` }
        })

        console.log(res);
      }
    }
</script>

<style scoped>

</style>
