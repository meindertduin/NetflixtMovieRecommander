<template>
  <v-layout>
    <v-flex class="d-flex justify-center mt-20">
      <v-card width="1200" class="mx-auto mt-12"  shaped>
        <v-img class="white--text align-end" height="400px"
          src="/netflix-logo.png"
        >
          <v-card-title>
            Always arguing about what movie to watch?
          </v-card-title>
        </v-img>
        <v-card-text>
          Lorem ipsum dolor sit amet, consectetur adipisicing elit. Accusamus alias asperiores consectetur cupiditate delectus deleniti enim eum ex fugit hic laboriosam libero maiores nostrum perspiciatis quam recusandae, repudiandae unde velit veniam voluptate? Adipisci alias aliquam aspernatur enim et id incidunt ipsa minus modi necessitatibus reiciendis saepe sequi tempora, veritatis voluptas!
        </v-card-text>
        <v-card-actions>
          <v-btn @click="TOGGLE_OVERLAY">Get your movies now!</v-btn>
        </v-card-actions>
      </v-card>
      <v-overlay :absolute="true" :value="overlayActive" >
        <UploadOverlay></UploadOverlay>
      </v-overlay>
    </v-flex>
  </v-layout>
</template>

<script>
import UploadOverlay from "../.nuxt/components";
import {mapState, mapMutations} from 'vuex';

export default {
  components: {
    UploadOverlay,
  },
  computed: {
    ...mapState('watchlist', ['overlayActive']),

  },
  methods: {
    ...mapMutations('watchlist', ['TOGGLE_OVERLAY']),
  }
}
</script>
