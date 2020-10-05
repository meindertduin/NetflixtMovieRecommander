<template>
  <div>
    <v-row justify="center">
      <v-col cols="10">
        <v-card class="mx-auto mt-12 banner-card"  shaped>
          <div class="banner-container">
            <v-img class="banner-img" src="/cinema-bg.jpg">
            </v-img>
            <div class="banner-title display-3 font-weight-bold">Need some help finding a movie or series?</div>
            <div class="banner-button">
              <v-btn @click="redirectToRecommendations" color="orange" rounded>Seek no further</v-btn>
            </div>
          </div>
        </v-card>
      </v-col>
    </v-row>
    <v-row justify="center">
      <v-col cols="10">
        <v-row>
          <v-col cols="12" sm="6">
            <v-card class="mx-auto mt-12 banner-card" shaped>
              <div class="banner-container">
                <v-img class="banner-img" src="/movie-camera.jpg">
                </v-img>
                <div class="watchtuner-info-title banner-title display-3 font-weight-bold">What is watchTuner?</div>
              </div>
              <div class="banner-card-text">
                Watchtuner is a free website where you can can upload multiple netflix watch history lists to get the freshest recommendations provided by us.
                It also provides features like greating your own watch group with your friends and tag movies or series that have already been watched.
                This makes getting getting your fresh/movie or series for your next film night a breeze.
              </div>
              <v-card-actions>
                <v-row justify="center">
                  <v-btn class="mx-2 mb-4" color="orange">Get Quick recommendations</v-btn>
                </v-row>
              </v-card-actions>
            </v-card>
          </v-col>
          <v-col cols="12" sm="6">
            <v-card class="mx-auto mt-12 banner-card">
              <div class="banner-container">
                <v-img class="banner-img popcorn-banner" src="/popcorn-bg.jpg">
                </v-img>
                <div class="watchroup-banner-title banner-title display-2 font-weight-bold">Get the most out of your film nights with watch groups!</div>
                <div class="banner-button watchgroup-banner-button">
                  <v-btn class="mx-2 mb-4" color="green" @click="$auth.signinRedirect()">Register for free</v-btn>
                </div>
              </div>
            </v-card>
          </v-col>
        </v-row>
      </v-col>
    </v-row>
  </div>
</template>

<script lang="ts">
import { Component, Vue} from "nuxt-property-decorator";


@Component({})
export default class index extends Vue{
  private redirectToRecommendations(){
    this.$router.push('/recommended')
  }


}
</script>

<style scoped>
  .banner-container{
    height: auto;
    display: block;
    position:relative;
    text-align: center;
    width: 100%;
    overflow: hidden;
  }

  .banner-img{
    width: 100%;
    height: auto;
  }

  .banner-title{
    position: absolute;
    display: block;
    top: 30%;
    left: 50%;
    -o-transform: translate(-50%, -50%);
    -ms-transform: translate(-50%, -50%);
    -moz-transform: translate(-50%, -50%);
    -webkit-transform: translate(-50%, -50%);
    transform: translate(-50%, -50%);
    text-align: center;
    color: white;
  }

  .banner-button{
    position: absolute;
    display: block;
    top: 40%;
    left: 50%;
    -o-transform: translate(-50%, -50%);
    -ms-transform: translate(-50%, -50%);
    -moz-transform: translate(-50%, -50%);
    -webkit-transform: translate(-50%, -50%);
    transform: translate(-50%, -50%);
    text-align: center;
    color: white;
  }

  .watchtuner-info-title{
    top: 40%;
    left: 50%;
  }

  .watchroup-banner-title{
    top: 15%;
    width: 100%;
    left: 50%;
  }

  .banner-card-text{
    padding: 20px 50px 20px 50px;
  }

  .popcorn-banner{
    height: 660px;
  }

  .watchgroup-banner-button{
    top: 50%;
  }
</style>
