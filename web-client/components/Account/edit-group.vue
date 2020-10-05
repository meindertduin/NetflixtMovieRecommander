<template>
  <v-card class="px-4">
    <v-card-title>
      Edit Group
    </v-card-title>
    <v-card-text>
      <div class="edit-watchgroup-container">
        <v-row>
          <v-col>
            <v-text-field label="Group Title" v-model="editedTitle"></v-text-field>
          </v-col>
        </v-row>
        <v-row>
          <v-col>
            <v-textarea label="Description" v-model="editedDescription"></v-textarea>
          </v-col>
        </v-row>
        <v-row class="watchgroup-names-row">
          <v-chip-group column v-for="(x, index) in editedAddedNames" :key="index">
            <v-chip close @click:close="removeAddedName(x)">{{x}}</v-chip>
          </v-chip-group>
        </v-row>
        <v-row>
          <v-text-field label="Add Name" v-model="AddNameFormValue"></v-text-field>
          <v-btn text color="green" @click="AddName">Add</v-btn>
        </v-row>
        <v-row class="my-4" justify="center">
          <v-btn class="mx-2" @click="save" color="green">Save Changes</v-btn>
          <v-btn class="mx-2" @click="initFields" >Cancel</v-btn>
        </v-row>
        <hr class="m-4">

        <v-row>
          <div class="display-1 font-weight-bold white--text mt-4">Add Friend</div>
        </v-row>
        <InviteAsFollower :group-id="currentWatchGroup.id"/>
        <hr>

        <v-row>
          <div class="display-1 font-weight-bold white--text mt-4">Add Watchlist</div>
        </v-row>
        <AddWatchlist class="w-100" :group-id="currentWatchGroup.id"/>

        <hr class="m-4">
        <v-row>
          <div class="display-1 white--text font-weight-bold my-4">Delete Group</div>
        </v-row>
        <v-row>
          <div class="p-4">
            <div class="h1 white--text">Delete Group</div>
            <div>Type <span class="red--text">Delete</span> and click the button to delete the watchgroup</div>
            <v-text-field v-model="deleteGuardText"></v-text-field>
            <v-btn @click="deleteWatchGroup">Delete</v-btn>
          </div>
        </v-row>
      </div>

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

    private deleteGuardText:string = "";

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

    private async deleteWatchGroup() {
      if (this.deleteGuardText.toLocaleLowerCase() !== "delete") return;
      this.$store.dispatch('watchgroup/deleteWatchGroup', this.currentWatchGroup.id)
        .then(() => {
        this.closeEditOverlay();
      }).catch(err => console.log(err));

    }

    created(){
      this.initFields();
    }
  }
</script>

<style scoped>
  .edit-watchgroup-container{
    max-height: 400px;
    max-width: 800px;
    padding: 40px;
    overflow: auto;
    overflow-x: hidden;
  }

  .watchgroup-names-row{
    max-width: 800px;
  }

  .edit-watchgroup-container::-webkit-scrollbar {
    width: 12px;
  }

  .edit-watchgroup-container::-webkit-scrollbar-track {
    background-color: black;
    border-radius: 10px;
  }

  .edit-watchgroup-container::-webkit-scrollbar-thumb {
    border-radius: 5px;
    height: 2px;
    background-color:#171a1d;
    -webkit-box-shadow: inset 0 0 6px rgba(90,90,90,0.7);
  }
</style>
