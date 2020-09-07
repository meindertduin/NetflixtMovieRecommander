<template>
  <v-layout>
    <v-flex class="d-flex justify-center mt-20">
      <v-card>
        <v-card-title>
          Here will the video be uploaded
        </v-card-title>

        <v-stepper v-model="step" v-if="uploadShowHelp">
          <v-stepper-header>
            <v-stepper-step :complete="step > 1" step="1">Helper</v-stepper-step>

            <v-divider></v-divider>

            <v-stepper-step :complete="step > 2" step="2">Upload</v-stepper-step>
          </v-stepper-header>

          <v-stepper-items>

            <v-stepper-content step="1">
              <v-card>
                <v-card-title>
                  Find your file here
                </v-card-title>
                <v-card-actions>
                  <v-btn @click="step++">Continue</v-btn>
                </v-card-actions>
              </v-card>
            </v-stepper-content>

            <v-stepper-content step="2">
              <v-card>
                <v-card-title>
                  Upload here your watchlist.csv
                </v-card-title>
                <v-card-text>
                  Fill in here your watchlist.csv file.
                </v-card-text>
                <v-card-actions>
                  <v-file-input accept=".csv" label="File input" multiple v-model="watchLists" prepend-icon="mdi-paperclip">
                  </v-file-input>
                </v-card-actions>
              </v-card>
            </v-stepper-content>

          </v-stepper-items>
        </v-stepper>


        <v-stepper v-model="step" v-else>
          <v-stepper-header>
            <v-stepper-step :complete="step > 1" step="1">Upload csv</v-stepper-step>

            <v-divider></v-divider>

            <v-stepper-step step="2">Completion</v-stepper-step>
          </v-stepper-header>


          <v-stepper-items>
            <v-stepper-content step="1">
              <v-card>
                <v-card-title>
                  Upload here your watchlist.csv
                </v-card-title>
                <v-card-text>
                  Fill in here your watchlist.csv file. <a @click="toggleUploadShowHelp">Click here</a> to see how to obtain.
                </v-card-text>
                <v-card-actions>
                  <v-file-input accept=".csv" label="File input" multiple v-model="watchLists" prepend-icon="mdi-paperclip"></v-file-input>
                  <v-btn @click="handleFileUpload">Upload</v-btn>
                </v-card-actions>
              </v-card>
            </v-stepper-content>

            <v-stepper-content step="2">
              <v-card>
                <v-card-title>
                  Your uploading is complete
                </v-card-title>
              </v-card>
            </v-stepper-content>


          </v-stepper-items>
        </v-stepper>

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
    private uploadShowHelp = false;
    private watchLists = {};
    private step = 1;

    private toggleUploadShowHelp(){
      this.uploadShowHelp = ! this.uploadShowHelp;
    }

    private async handleFileUpload(){

      if(!this.watchLists) return;

      const form = new FormData();

      form.append("watchlist", this.watchLists[0]);
      await this.$store.dispatch('watchlist/uploadWatchLists', {form});
      this.step++;
    }
  }
</script>

