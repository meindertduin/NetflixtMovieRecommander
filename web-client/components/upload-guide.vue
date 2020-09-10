<template>
  <v-card max-width="800">
    <v-card-title class="d-flex justify-center">
      Watch History Upload Guide
    </v-card-title>
    <v-img class="white--text align-end" aspect-ratio="3"
           src="/netflixactivity.jpg"
    >

    </v-img>
    <v-card-text>
      Finding your viewing history is fairly simple. Go to Netflix.com, ensure that your logged in and then hover over your profile name.
      Select <b>Your Account</b> from the menu. Then scroll down to the bottom and select <b>Viewing history</b>. Alternitively you can also
      click <a href="https://www.netflix.com/viewingactivity" target="_blank">here</a> While your
      there scroll down to the bottom and select <b>Download everything</b>. Download this file and gather the watch histories of other
      accounts and upload them to get movie/series recommendations that both haven't watched!
    </v-card-text>
    <v-card-actions class="justify-center">
      <v-btn @click="toggleUploadGuide">Lets Get Some Recommendations!</v-btn>
    </v-card-actions>
  </v-card>
</template>

<script lang="ts">
  import {Component, Vue} from "nuxt-property-decorator";

    @Component({})
    export default class UploadGuide extends Vue{
      private toggleUploadGuide(){
        this.$store.commit('watchlist/TOGGLE_GUIDE');
      }
    }
</script>

