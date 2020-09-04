<template>
  <v-layout>
    <v-flex class="d-flex justify-center mt-20">
      <v-card>
        <v-card-title>
          Here will the video be uploaded
        </v-card-title>
        <v-card-actions>
          <v-btn @click="TOGGLE_OVERLAY">Close</v-btn>
        </v-card-actions>
      </v-card>
    </v-flex>
  </v-layout>
</template>

<script lang="ts">
  import { Vue, Component } from 'nuxt-property-decorator'
  import {mapMutations} from "vuex";

  @Component({
    methods: {
      ...mapMutations('watchlist', ['TOGGLE_OVERLAY']),
    }
  })
  export default class UploadOverlay extends Vue {

  }
</script>

