<template>
  <v-card min-width="500">
    <v-card-title>
      Group creation
    </v-card-title>
    <v-stepper v-model="step">
      <v-stepper-header>
        <v-stepper-step :complete="step > 1" step="1">Group details</v-stepper-step>

        <v-divider></v-divider>

        <v-stepper-step :complete="step > 2" step="2">Add users</v-stepper-step>

        <v-divider></v-divider>

        <v-stepper-step step="3">Save Group</v-stepper-step>
      </v-stepper-header>

      <v-stepper-items>
        <v-stepper-content step="1">
          <v-text-field class="mt-4" label="Group Name" v-model="groupTile"></v-text-field>
          <v-textarea label="Group Description" :counter="descriptionCount" v-model="description"></v-textarea>

          <v-btn
            color="primary"
            @click="step = 2"
            :disabled="groupTile.length === 0"
          >
            Continue
          </v-btn>

          <v-btn @click="toggleCreationOverlay">Cancel</v-btn>
        </v-stepper-content>

        <v-stepper-content step="2">
          <div v-if="displayNameList.length > 0">
            Added
            <div v-for="x in displayNameList" :key="x">{{x}}</div>
          </div>


          <div>
            Add Existing User
          </div>
          <v-text-field label="User Name"></v-text-field>

          <hr class="my-4">

          Add Group Member Name
          <v-text-field label="name" v-model="addedUserFormName"></v-text-field>
          <v-row class="ma-5">
            <v-btn color="green" @click="addUser" >Add User</v-btn>
          </v-row>

          <hr class="my-4">

          <v-btn
            class="ml-5"
            color="primary"
            @click="save"
          >
            Create
          </v-btn>

          <v-btn @click="toggleCreationOverlay">Cancel</v-btn>
        </v-stepper-content>

        <v-stepper-content step="3">

          <p>
            Add a Netflix Watchlist to get movies you haven't watched To see a guide on how to
            add a watch list click <a>Here</a>. You can also add a watchlist later.
          </p>


          <div v-if="watchListsCount > 0">{{watchListsCount}} Netflix watchlists added</div>
          <AddWatchlist :group-id="this.createdWatchGroupId"/>

          <v-btn @click="toggleCreationOverlay">Coninue</v-btn>

        </v-stepper-content>
      </v-stepper-items>
    </v-stepper>
  </v-card>
</template>

<script lang="ts">
  import {Component, Vue, Watch} from "nuxt-property-decorator";
  import AddWatchlist from "~/components/Account/add-watchlist.vue";


    interface existingUser{
      id: string,
      userName: string,
    }

    interface groupCreationForm{
      title: string,
      description: string | null,
      existingUsers: Array<existingUser> | null,
      addedUsers: Array<string> | null,
    }

    @Component({
      components: {
        AddWatchlist,
      }
    })
    export default class CreateGroup extends Vue{
      private step:number = 1;
      private descriptionCount:number = 200;

      private groupTile:string = "";
      private description:string = "";

      private watchList:any = {};
      private watchListsCount:number = 0;

      private existingUserSearchValue:string = "";
      private existingUsers: Array<existingUser> = [];

      private addedUserFormName:string = "";

      private addedUsers: Array<string> = [];
      private displayNameList: Array<string> = [];

      private createdWatchGroupId = "";


      private addUser(){
        if(this.addedUserFormName.length > 0){
          this.addedUsers.push(this.addedUserFormName);
          this.displayNameList.push(this.addedUserFormName);
          this.addedUserFormName = "";
        }
      }

      private async save(): Promise<void> {
        let groupForm: groupCreationForm = {
          title: this.groupTile,
          description: this.description,
          existingUsers: this.existingUsers,
          addedUsers: this.addedUsers,
        }

        await this.$store.dispatch('watchgroup/createGroup', {groupForm})
          .then(({data}) => {
            this.createdWatchGroupId = data;
            this.step++;
          })
        .catch(() => {
          // notify user
        })
      }

      private toggleCreationOverlay():void{
        this.$store.commit('watchgroup/TOGGLE_CREATION_OVERLAY');
      }

    }
</script>

<style scoped>

</style>
