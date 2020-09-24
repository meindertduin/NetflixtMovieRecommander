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
          <v-btn @click="redirectToRecommendations">Get your movies now!</v-btn>
        </v-card-actions>
      </v-card>
    </v-flex>
  </v-layout>
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
