<template>
  <v-card min-width="400">
    <v-card-title>
      Edit Group
    </v-card-title>
    <v-card-text>
      <v-text-field label="Group Title" v-model="editedTitle"></v-text-field>
      <v-text-field label="Description" v-model="editedDescription"></v-text-field>
      <v-row>
        <v-chip-group column v-for="(x, index) in editedAddedNames" :key="index">
          <v-chip close @click:close="removeAddedName(x)">{{x}}</v-chip>
        </v-chip-group>
      </v-row>
      <v-row class="justify-start align-center mx-2">
        <v-text-field label="Add Name" v-model="AddNameFormValue"></v-text-field><v-btn color="green" @click="AddName">Add</v-btn>
      </v-row>
      <v-btn @click="save" color="green">Save Changes</v-btn>
      <v-btn @click="initFields" >Cancel</v-btn>

      <hr class="my-3">

      <!-- Todo Add functionality for inviting people -->
      <v-row class="justify-start align-center mx-2">
        <v-text-field label="Invite User"></v-text-field><v-btn color="green" text>Invite</v-btn>
      </v-row>


      <h3 class="mb-3">Add Watchlist: </h3>
      <v-row>
        <AddWatchlist  :group-id="currentWatchGroup.id"/>
      </v-row>
    </v-card-text>
    <v-card-actions>
      <v-row>
        <v-btn @click="closeEditOverlay" >
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

  @Component({
    components: {
      AddWatchlist
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

      this.$store.dispatch('watchgroup/editGroup', payload);
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
