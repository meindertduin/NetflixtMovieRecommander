<template>
  <v-card
    dark
  >
    <v-card-title class="headline">
      Search Username
    </v-card-title>
    <v-card-text>
      <v-autocomplete
        v-model="selected"
        :items="resultEntries"
        :loading="isLoading"
        :search-input.sync="searchTerm"
        color="white"
        hide-no-data
        hide-selected
        item-text="Description"
        item-value="API"
        label="Search For User"
        placeholder="Start typing to Search"
        prepend-icon="mdi-account"
        return-object
      ></v-autocomplete>
    </v-card-text>
    <v-divider></v-divider>
    <v-expand-transition>
      <v-list>
        <v-list-item v-if="selectedProfile != null">
          <v-list-item-content>
            <v-avatar>
              <v-img :src="selectedProfile.avatarUrl != null? selectedProfile.avatarUrl: '/default_profile.jpg'" max-height="30" max-width="30" aspect-ratio="1"></v-img>
              {{" " + selectedProfile.userName}}
              <v-btn text @click="invite(selectedProfile.id)">Invite</v-btn>
            </v-avatar>
          </v-list-item-content>
        </v-list-item>
        <v-list-item v-if="selectedProfile == null">
          <v-list-item-content>
            <v-list-item-title  class="orange--text">
              <v-row justify="center">
                {{inviteMessage}}
              </v-row>
            </v-list-item-title>
          </v-list-item-content>
        </v-list-item>
      </v-list>
    </v-expand-transition>
    <v-card-actions>
      <v-spacer></v-spacer>
      <v-btn
        :disabled="!selected"
        color="grey darken-3"
        @click="selected = null"
      >
        Clear
        <v-icon right>mdi-close-circle</v-icon>
      </v-btn>
    </v-card-actions>
  </v-card>
</template>

<script lang="ts">
  import {Component, Prop, Vue, Watch} from "nuxt-property-decorator";
  import {Profile, WatchGroupInvite} from "~/assets/interface-models";

    @Component({})
    export default class InviteAsFollower extends Vue{
      private searchResults:Array<Profile> = [];
      private selected:string | null =  null;
      private isLoading:boolean = false;

      private inviteMessage:string = "";

      @Prop({type: String, required: true}) readonly groupId !:string;

      get resultEntries():Array<string>{
        return this.searchResults.map(x => x.userName);
      }

      get selectedProfile(){
        if(!this.selected) return null;
        return this.searchResults.find(x => x.userName === this.selected);
      }

      private searchTerm:string = "";
      @Watch("searchTerm")
      OnSearchTermChange(value:string, oldValue:string){
        if(value == null) return;
        if(value.length === 0) return;
        if(this.isLoading) return;

        console.log(value)
        this.isLoading = true;
        this.$axios.get(`api/profile/find?searchTerm=${value}`)
          .then(({data}) => {
            this.searchResults = data;
          })
        .catch(err => {
          console.log(err);
        }).finally(() => this.isLoading = false);
      }

      private invite(userId:string):void{
        // test purpose function
        if(this.groupId == null || userId == null) return;
        const groupInvite :WatchGroupInvite = {
          subjectId: userId,
          groupId: this.groupId,
        }

        this.$axios.post('api/watchgroup/invite', groupInvite)
          .then(() => {
            this.inviteMessage = "Invite has been sent";
          })
          .catch((err) => {
            this.inviteMessage = "Something went wrong sending your invite";
            console.log(err);
          })
          .finally(() => {
            this.selected = null;
          })
      }
    }
</script>

<style scoped>

</style>
