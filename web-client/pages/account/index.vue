﻿﻿﻿﻿﻿﻿﻿<template>
  <v-container>
    <v-row justify="center">
      <v-col cols="12" md="4">
        <v-row justify-md="end" justify="center">
          <v-card width="90%" color="dark">
            <v-card-title>
              <v-img class="white--text"
                     src="/netflix-logo.png"/>
            </v-card-title>
            <v-card-text>
              <v-col cols="12">
                <v-row>
                  <h1>Meindert</h1>
                </v-row>
                <v-row>
                  <div>Lorem ipsum dolor sit amet, consectetur adipisicing elit. Accusantium amet culpa delectus fugiat in maxime nihil, officiis provident rerum velit.</div>
                </v-row>
                <v-row>
                  <v-icon>fas fa-address-book</v-icon>
                </v-row>
              </v-col>
            </v-card-text>
            <v-card-actions>
              <v-row justify="center" class="my-3">
                <v-btn>Edit Profile</v-btn>
              </v-row>
            </v-card-actions>
          </v-card>
        </v-row>
      </v-col>
      <v-col cols="12" md="8">
        <v-row  justify-md="end" justify="center">
          <v-card width="90%">
            <v-app-bar
              color="dark"
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
        </v-row>
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

    async mounted() {
      await this.$store.getters['getInitPromise'];
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
