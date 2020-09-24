﻿<template>
  <v-row justify-md="end" justify="center">
    <v-card width="90%" color="dark">
      <v-card-title>
        <v-row justify="center" align-content="center">
          <v-img v-if="userProfile == null" class="white--text"
                 src="/default_profile.jpg"  aspect-ratio="1"/>
          <v-img v-else class="white--text" aspect-ratio="1" max-height="200" max-width="200"
                 :src="userProfile.avatarUrl != null? userProfile.avatarUrl: '/default_profile.jpg'"></v-img>
        </v-row>
      </v-card-title>
      <v-card-text>
        <v-col cols="12">
          <v-row>
            <h1 class="mb-4">{{userProfile != null? userProfile.userName: ""}}</h1>
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
            <div class="text-body-2 orange--text">
              {{uploadMessage}}
            </div>
            <v-file-input accept="image/*" prepend-icon="mdi-paperclip" label="profile picture" v-model="uploadFile"></v-file-input>
            <v-btn text color="green" @click="handleFileUpload">
              Upload
            </v-btn>
          </v-col>
        </v-row>
      </v-card-text>
      <v-card-actions>
        <v-row justify="center">
          <v-btn text @click="toggleEditProfilePanel">
            Close
          </v-btn>
        </v-row>
      </v-card-actions>
    </v-card>
  </v-row>
</template>

<script lang="ts">
  import {Component, Prop, Vue} from "nuxt-property-decorator";
  import {user} from "~/store/user";
  import {Profile} from "~/assets/interface-models";

  @Component({})
  export default class ProfileDisplay extends Vue{
    get followersCount(){
      return 12;
    }

    get userProfile():Profile | null{
      return (this.$store.state.user as user).userProfile;
    }

    private editProfilePanel:boolean = false
    private uploadFile:File | null = null;
    private uploadMessage:string = "";

    private toggleEditProfilePanel():void{
      this.editProfilePanel = !this.editProfilePanel;
    }

    private handleFileUpload(){
      if(this.uploadFile === null) return;
      if(this.uploadFile.size > 100_000){
        console.log("to big");
        this.uploadMessage = "The size of the file is to big"
        return;
      }

      const form:FormData = new FormData();
      form.append('picture', this.uploadFile);

      this.$axios.post('api/profile/picture', form)
        .then(() => {
          this.uploadMessage = "The upload succeeded"
        }).catch(() => {
          this.uploadMessage = "Something went wrong uploading your file"
      })
    }
  }
</script>

<style scoped>

</style>
