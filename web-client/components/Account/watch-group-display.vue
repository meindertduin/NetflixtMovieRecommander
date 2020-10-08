﻿<template>
  <v-card>
    <v-app-bar color="dark">
      <v-toolbar-title>Watch Groups</v-toolbar-title>
      <v-spacer></v-spacer>
      <v-toolbar-items>
        <v-text-field class="mb-5 mt-1" label="search" v-model="searchTerm" clearable outlined rounded color="white"></v-text-field>
      </v-toolbar-items>
      <v-btn color="red" class="mx-2" @click="toggleCreationOverlay">
        Create new
      </v-btn>
    </v-app-bar>
    <v-row class="m1">
      <v-col :cols="12" v-for="(x, index) in displayedWatchGroups" :key="index">
        <WatchGroup  :title="x.title" :members="x.members" :added-names="x.addedNames" :owner="x.owner" :description="x.description" :watch-group="x"/>
      </v-col>
    </v-row>
  </v-card>
</template>

<script lang="ts">
  import {Component, Vue, Watch} from "nuxt-property-decorator";
  import WatchGroup from "~/components/Account/watch-group.vue";
  import {watchgroup} from "~/store/watchgroup";
  import {WatchGroupModel} from "~/assets/interface-models";

    @Component({
      components: {
        WatchGroup,
      },
    })
    export default class WatchGroupDisplay extends Vue{
      private displayedWatchGroups: Array<WatchGroupModel> = [];
      private searchTerm:string = "";

      @Watch("searchTerm")
      onPropertyChanged(value: string, oldValue: string) {
        this.displayedWatchGroups = this.loadedUserWatchGroups.filter((value:WatchGroupModel) => {
          return value.title.includes(this.searchTerm);
        })
      }

      get loadedUserWatchGroups():Array<WatchGroupModel>{
        return (this.$store.state.watchgroup as watchgroup).watchGroups;
      }

      @Watch("loadedUserWatchGroups")
      onUserWatchGroupsChange(value: Array<WatchGroupModel>, oldValue:Array<WatchGroupModel>){
        this.displayedWatchGroups = value;
      }

      created(){
        this.displayedWatchGroups = this.loadedUserWatchGroups;
      }

      private toggleCreationOverlay():void{
        this.$store.commit('watchgroup/TOGGLE_CREATION_OVERLAY');
      }
    }
</script>

<style scoped>

</style>
