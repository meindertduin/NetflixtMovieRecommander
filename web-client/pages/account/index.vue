﻿﻿﻿﻿﻿﻿﻿<template>
  <v-container>
    <v-row  class="justify-center">
      <v-col :cols="8">
        <v-card>
          <v-app-bar
            color="pink"
          >
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
      </v-col>
    </v-row>
    <v-overlay v-if="creationOverlayActive">
      <CreateGroup />
    </v-overlay>
    <v-overlay v-if="editOverlayActive">
      <EditGroup />
    </v-overlay>
  </v-container>
</template>

<script lang="ts">
  import {Component, Vue, Watch} from "nuxt-property-decorator";
  import WatchGroup from "~/components/Account/watch-group.vue";
  import CreateGroup from "~/components/Account/create-group.vue";
  import {watchgroup} from "~/store/watchgroup";
  import EditGroup from "~/components/Account/edit-group.vue";

  @Component({
    name: "account",
    components: {
      WatchGroup,
      CreateGroup,
      EditGroup,
    },
  })
  export default class Account extends Vue{
    private userWatchGroups:Array<WatchGroup> = [];
    private displayedWatchGroups: Array<WatchGroup> = [];
    private searchTerm:string = "";

    @Watch("searchTerm")
    onPropertyChanged(value: string, oldValue: string) {
      this.displayedWatchGroups = this.loadedUserWatchGroups.filter((value:WatchGroup) => {
        return value.title.includes(this.searchTerm);
      })
    }

    get loadedUserWatchGroups():Array<WatchGroup>{
      return (this.$store.state.watchgroup as watchgroup).watchGroups;
    }

    @Watch("loadedUserWatchGroups")
    onUserWatchGroupsChange(value: Array<watchgroup>, oldValue:Array<watchgroup>){
      this.displayedWatchGroups = value;
    }


    get creationOverlayActive():boolean{
      return (this.$store.state.watchgroup as watchgroup).creationOverlayActive;
    }

    get editOverlayActive():boolean{
      return (this.$store.state.watchgroup as watchgroup).editOverlayActive;
    }

    async created() {
      await this.$store.dispatch('watchgroup/getUserWatchGroups');
      this.displayedWatchGroups= this.loadedUserWatchGroups;
    }
    
    private toggleCreationOverlay():void{
      this.$store.commit('watchgroup/TOGGLE_CREATION_OVERLAY');
    }
  }
</script>

<style scoped>

</style>
