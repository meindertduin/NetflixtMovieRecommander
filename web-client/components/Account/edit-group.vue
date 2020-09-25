<template>
  <v-card class="px-4">
    <v-card-title>
      Edit Group
    </v-card-title>
    <v-card-text>
      <v-row>
        <v-col cols="12" sm="6">
          <v-text-field label="Group Title" v-model="editedTitle"></v-text-field>
          <v-text-field label="Description" v-model="editedDescription"></v-text-field>
          <v-row>
            <v-chip-group column v-for="(x, index) in editedAddedNames" :key="index">
              <v-chip close @click:close="removeAddedName(x)">{{x}}</v-chip>
            </v-chip-group>
          </v-row>
          <v-row justify="center" align="center">
            <v-text-field label="Add Name" v-model="AddNameFormValue"></v-text-field>
            <v-btn text color="green" @click="AddName">Add</v-btn>
          </v-row>
        </v-col>
        <v-col cols="12" sm="6">
          <v-row justify="center" align="center">
            <InviteAsFollower :group-id="currentWatchGroup.id"/>
          </v-row>
          <h3 class="mb-3">Add Watchlist: </h3>
          <v-row justify="center">
            <AddWatchlist  :group-id="currentWatchGroup.id"/>
          </v-row>
        </v-col>
      </v-row>
      <v-row justify="center">
        <v-col cols="12">
          <v-btn @click="save" color="green">Save Changes</v-btn>
          <v-btn @click="initFields" >Cancel</v-btn>
        </v-col>
      </v-row>

      <!-- Todo Add functionality for inviting people -->

    </v-card-text>
    <v-card-actions>
      <v-row justify="center">
        <v-btn @click="closeEditOverlay" width="90%">
          Close
        </v-btn>
      </v-row>
    </v-card-actions>
  </v-card>
</template>

<script lang="ts">
  import {Component, Vue} from "nuxt-property-decorator";
  import {UpdateWatchGroupModel, WatchGroupModel} from "~/assets/interface-models";
  import {watchgroup} from "~/store/watchgroup";
  import AddWatchlist from "~/components/Account/add-watchlist.vue";
  import InviteAsFollower from "~/components/Account/invite-as-follower.vue";

  @Component({
    components: {
      AddWatchlist,
      InviteAsFollower,
    }
  })
  export default class EditGroup extends Vue{

    private editedTitle:string = "";
    private editedDescription:string = "";

    private editedAddedNames:Array<string> = [];
    private AddNameFormValue:string = "";

    get currentWatchGroup():WatchGroupModel{
      return (this.$store.state.watchgroup as watchgroup).currentSelectedWatchGroup;
    }

    private AddName():void{
      if(this.AddNameFormValue.length === 0) return;
      this.editedAddedNames.push(this.AddNameFormValue);
      this.AddNameFormValue = "";
    }

    private removeAddedName(name:string):void{
      this.editedAddedNames =  this.editedAddedNames.filter(function(value, index, arr){ return value !== name})
    }

    private save():void{
      let payload: UpdateWatchGroupModel = {
        id: this.currentWatchGroup.id,
        title: this.editedTitle,
        description: this.editedDescription,
        addedNames: this.editedAddedNames,
      }

      console.log(payload);

      this.$store.dispatch('watchgroup/editGroup', payload)
    }

    private initFields():void{
      this.editedAddedNames = this.currentWatchGroup.addedNames.slice();
      this.editedTitle = this.currentWatchGroup.title;
      this.editedDescription = this.currentWatchGroup.description;
    }

    private closeEditOverlay(){
      this.$store.commit('watchgroup/CLOSE_EDIT_OVERLAY');
    }

    created(){
      this.initFields();
    }
  }
</script>

<style scoped>

</style>
