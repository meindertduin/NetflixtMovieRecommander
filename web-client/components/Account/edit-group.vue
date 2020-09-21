<template>
  <v-card>
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
      <hr>

      <!-- Todo Add functionality for inviting people -->
      <v-row class="justify-start align-center mx-2">
        <v-text-field label="Invite User"></v-text-field><v-btn color="green" text>Invite</v-btn>
      </v-row>

      <v-row>
        <AddWatchlist  :group-id="currentWatchGroup.id"/>
      </v-row>
    </v-card-text>
  </v-card>
</template>

<script lang="ts">
  import {Component, Vue} from "nuxt-property-decorator";
  import {WatchGroupModel} from "~/assets/interface-models";
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
      if(this.editedTitle !== this.currentWatchGroup.title){
        this.$store.dispatch('watchgroup/editGroupTitle', {title: this.editedTitle, id: this.currentWatchGroup.id})
      }
      if(this.editedDescription !== this.currentWatchGroup.description){
        this.$store.dispatch('watchgroup/editGroupDescription', {description: this.editedDescription, id: this.currentWatchGroup.id})
      }
      if(this.editedAddedNames.length !== this.currentWatchGroup.addedNames.length){
        let addedNamesUnchanged = true;
        for (let i = 0; i < this.editedAddedNames.length; i++){
          if(this.currentWatchGroup.addedNames.includes(this.editedAddedNames[i]) == false){
            addedNamesUnchanged = false;
            break;
          }
        }
        if(addedNamesUnchanged === false){
          this.$store.dispatch('watchgroup/editGroupAddedNames', {addedNames: this.editedAddedNames, id: this.currentWatchGroup.id});
        }
      }
    }

    private initFields():void{
      this.editedAddedNames = this.currentWatchGroup.addedNames.slice();
      this.editedTitle = this.currentWatchGroup.title;
      this.editedDescription = this.currentWatchGroup.description;
    }

    private handleUpload():void{

    }

    created(){
      this.initFields();
    }
  }
</script>

<style scoped>

</style>
