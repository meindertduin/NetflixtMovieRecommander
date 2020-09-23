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
          <v-btn text>Edit Profile</v-btn>
          <v-btn text>
            <v-icon>mdi-account-multiple</v-icon>{{followersCount}} Followers
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
  }
</script>

<style scoped>

</style>
