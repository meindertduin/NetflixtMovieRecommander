<template>
    <div></div>
</template>

<script lang="ts">
  import {Component, Vue} from "nuxt-property-decorator";
  import {WatchGroupRecommendationForm} from "~/assets/interface-models";

    @Component({})
    export default class WatchGroupId extends Vue{
      private index:number = 0;
      private genres:Array<string> = [];
      private type:string = "both";

      created(){

      }

      private getRecommendations(){
        const payload:WatchGroupRecommendationForm = {
          genres: this.genres,
          index: this.index,
          seed: this.uuidv4(),
          type: this.type,
        }
        this.$store.dispatch('watchgroup/getRecomemendations', {route: this.$route.params.id, payload})
      }

      private uuidv4() {
        return 'xxxxxxxx-xxxx-4xxx-yxxx-xxxxxxxxxxxx'.replace(/[xy]/g, function(c) {
          let r = Math.random() * 16 | 0, v = c == 'x' ? r : (r & 0x3 | 0x8);
          return v.toString(16);
        });
      }
    }
</script>

<style scoped>

</style>
