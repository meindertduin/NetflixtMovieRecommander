<template>
  <v-row justify-md="end" justify="center">
    <v-card width="90%" color="dark">
      <v-card-title>
        <v-img class="white--text"
               src="/netflix-logo.png"/>
      </v-card-title>
      <v-card-text>
        <v-col cols="12">
          <v-row>
            <h1 class="mb-4">Meindert</h1>
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
          <v-btn text @click="toggleEditProfilePanel">Edit Profile</v-btn>
          <v-btn text>
            <v-icon>mdi-account-multiple</v-icon>{{followersCount}} Followers
          </v-btn>
        </v-row>
      </v-card-actions>
    </v-card>
    <v-card v-if="editProfilePanel" width="90%" class="mt-3">
      <v-card-title>
        <v-row justify="center">Edit Profile</v-row>
      </v-card-title>
      <v-card-text>
        <v-row justify="center">
          <v-col cols="8">
            <h3>Profile Picture: </h3>
            <v-file-input label="profile picture"></v-file-input>
          </v-col>
        </v-row>
      </v-card-text>
      <v-card-actions>
        <v-row justify="center">
          <v-btn text color="green">
            Save
          </v-btn>
          <v-btn text @click="toggleEditProfilePanel">
            Cancel
          </v-btn>
        </v-row>
      </v-card-actions>
    </v-card>
  </v-row>
</template>

<script lang="ts">
  import {Component, Vue} from "nuxt-property-decorator";

  @Component({})
  export default class ProfileDisplay extends Vue{
    get followersCount(){
      return 12;
    }

    private editProfilePanel:boolean = false

    private toggleEditProfilePanel():void{
      this.editProfilePanel = !this.editProfilePanel;
    }
  }
</script>

<style scoped>

</style>
