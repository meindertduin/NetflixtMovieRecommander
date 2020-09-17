<template>
  <v-container>
    <v-row  class="justify-center">
      <v-col :cols="5">
        <WatchGroup :title="title" :shared-with="sharedWith"/>
      </v-col>
    </v-row>
  </v-container>
</template>

<script lang="ts">
  import {Component, Vue} from "nuxt-property-decorator";
  import WatchGroup from "~/components/Account/watch-group.vue";

  interface Person {
    userName: string,
    link: string | null,
    picture: string,
  }


  @Component({
    name: "account",
    components: {
      WatchGroup
    },
  })
  export default class Account extends Vue{
    private title:string = "title";


    private person: Person = {
      userName: "john",
      link: "https://localhost:3000",
      picture: "https://m.media-amazon.com/images/M/MV5BMGE4YzY4NGEtOWYyYS00ZDk2LWExMmMtZDIyODhiMmNlMGE0XkEyXkFqcGdeQXVyMTkxNjUyNQ@@._V1_SX300.jpg",
    }

    private sharedWith: Array<Person> = [];

    created(){
      this.sharedWith = [this.person, this.person, this.person, this.person, this.person, this.person, this.person, this.person, this.person, this.person, this.person, this.person, this.person, this.person, this.person, this.person, this.person, this.person]
    }
  }
</script>

<style scoped>

</style>
