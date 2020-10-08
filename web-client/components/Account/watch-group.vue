<template>
  <v-card
    color="dark"
    dark
  >
    <v-card-title >
      <v-row>
        <v-col :cols="4" class="4">
          {{watchGroup.title}}
        </v-col>
        <v-col :cols="8" class="caption">
          created by:
          <v-avatar color="blue">
            <img v-if="watchGroup.owner.avatarUrl !== null" :src="watchGroup.owner.avatarUrl" alt="">
            <span v-else class="white--text headline">{{watchGroup.owner.userName[0].toUpperCase()}}</span>
          </v-avatar>
          {{watchGroup.owner.userName}}
        </v-col>
      </v-row>
    </v-card-title>

    <v-card-subtitle>{{watchGroup.description}}</v-card-subtitle>
    <v-card-text>
      <div v-if="sharedWithCount > 0">
        <p>Shared with: {{sharedWithCount}} {{sharedWithCount > 1? "others": "other"}}</p>
        <v-list-group
          active-class="deep-purple accent-4 white--text"
          column
        >
          <v-list-item v-for="(x, index) in watchGroup.members" :key="x.id">
            <v-list-item-avatar color="blue">
              <img v-if="x.avatarUrl !== null" :src="x.avatarUrl" alt="">
              <span v-else class="white--text headline">{{x.userName[0].toUpperCase()}}</span>
            </v-list-item-avatar>
            <v-list-item-title>
              {{x.userName}}
            </v-list-item-title>
          </v-list-item>

          <v-list-item v-for="(x, index) in watchGroup.addedNames" :key="index">
            <v-list-item-avatar color="blue">
              <span class="white--text headline">{{x[0].toUpperCase()}}</span>
            </v-list-item-avatar>
            <v-list-item-title>
              {{x}}
            </v-list-item-title>
          </v-list-item>

        </v-list-group>
      </div>
    </v-card-text>
    <v-card-actions>

      <v-btn v-if="isOwner" @click="toggleEditOverlay">Edit</v-btn>
      <v-btn @click="viewRecommendations">Watch Now!</v-btn>
    </v-card-actions>
  </v-card>

</template>

<script lang="ts">
  import {Component, Prop, Vue} from "nuxt-property-decorator";
  import {WatchGroupModel} from "~/assets/interface-models";
  import {auth} from "~/store/auth";

    @Component({})
    export default class WatchGroup extends Vue{
      @Prop({type: Object, required: true}) watchGroup!: WatchGroupModel

      get isOwner(): boolean{
        const userProfile = (this.$store.state.auth as auth).userProfile;
        return !!(userProfile && userProfile.id === this.watchGroup.owner.id);
      }

      private viewRecommendations():void{
        this.$router.push('account/' + this.watchGroup.id);
      }


      get sharedWithCount(){
        return this.watchGroup.members.length + this.watchGroup.addedNames.length;
      }

      private toggleEditOverlay():void{
        this.$store.commit('watchgroup/OPEN_EDIT_OVERLAY', this.watchGroup);
      }
    }
</script>

<style scoped>

</style>
