<template>
  <v-container>
    <v-row  class="justify-center">
      <v-col :cols="8">
        <v-card>
          <v-app-bar
            color="pink"
          >
            <v-toolbar-title>Watch Groups</v-toolbar-title>
            <v-spacer></v-spacer>
            <v-toolbar-items>
              <v-text-field class="mb-5 mt-1" label="search" clearable outlined rounded color="white"></v-text-field>
            </v-toolbar-items>
            <v-btn color="red" class="mx-2" @click="toggleCreationOverlay">
              Create new
            </v-btn>
          </v-app-bar>
          <v-row class="m1">
            <v-col :cols="12" v-for="(x, index) in userWatchGroups" :key="index">
              <WatchGroup  :title="title" :shared-with="sharedWith"/>
            </v-col>
          </v-row>
        </v-card>
      </v-col>
    </v-row>
    <v-overlay v-if="creationOverlayActive">
      <CreateGroup />
    </v-overlay>
  </v-container>
</template>

<script lang="ts">
  import {Component, Vue} from "nuxt-property-decorator";
  import WatchGroup from "~/components/Account/watch-group.vue";
  import CreateGroup from "~/components/Account/create-group.vue";
  import {watchgroup} from "~/store/watchgroup";

  interface Person {
    userName: string,
    link: string | null,
    picture: string,
  }

  interface WatchGroup {
    id: string,
    addedNames: Array<string>,
    description: string,
    title: string,

    // todo change any to an interface
    owner: any,
    members: Array<any>,
  }


  @Component({
    name: "account",
    components: {
      WatchGroup,
      CreateGroup,
    },
  })
  export default class Account extends Vue{
    private title:string = "title";
    private userWatchGroups = [];

    private person: Person = {
      userName: "john",
      link: "https://localhost:3000",
      picture: "https://m.media-amazon.com/images/M/MV5BMGE4YzY4NGEtOWYyYS00ZDk2LWExMmMtZDIyODhiMmNlMGE0XkEyXkFqcGdeQXVyMTkxNjUyNQ@@._V1_SX300.jpg",
    }

    private sharedWith: Array<Person> = [];

    get creationOverlayActive():boolean{
      return (this.$store.state.watchgroup as watchgroup).creationOverlayActive;
    }

    async created() {
    await this.getData();
    this.sharedWith = [this.person, this.person, this.person, this.person, this.person, this.person, this.person];
    }

    async getData(){
      const {data}:watchgroup = await this.$axios.get('api/watchgroup');
      this.userWatchGroups = data;
      console.log(data);
    }

    private toggleCreationOverlay():void{
      this.$store.commit('watchgroup/TOGGLE_CREATION_OVERLAY');
    }
  }
</script>

<style scoped>

</style>
