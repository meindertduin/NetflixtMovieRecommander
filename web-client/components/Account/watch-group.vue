<template>
  <v-card
    color="#385F73"
    dark
  >
    <v-card-title class="headline">{{title}}</v-card-title>

    <v-card-subtitle>Een beschrijving</v-card-subtitle>
    <v-card-text>
      <p>Shared with</p>
      <v-list-group
        active-class="deep-purple accent-4 white--text"
        column
      >
        <v-list-item v-for="(x, index) in sharedWith" :key="index">
          <v-list-item-avatar color="blue">
            <img v-if="x.picture.length !== 0" :src="x.picture" alt="">
            <span v-else class="white--text headline">{{x.userName[0].toUpperCase()}}</span>
          </v-list-item-avatar>
          <v-list-item-title>
            {{x.userName}}
          </v-list-item-title>
        </v-list-item>

      </v-list-group>
    </v-card-text>
    <v-card-actions>

      <v-btn text>Edit</v-btn>
      <v-btn text>Watch Now!</v-btn>
    </v-card-actions>
  </v-card>

</template>

<script lang="ts">
  import {Component, Prop, Vue} from "nuxt-property-decorator";

  interface Person {
    userName: string,
    link: string | null,
    picture: string,
  }

    @Component({})
    export default class WatchGroup extends Vue{
      @Prop({type: String, required: true}) readonly title!: string
      @Prop({type: Array, required: true}) readonly sharedWith!: Array<Person>
    }
</script>

<style scoped>

</style>
