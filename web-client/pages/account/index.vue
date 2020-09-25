﻿﻿﻿﻿﻿﻿﻿﻿<template>
  <v-container>
    <v-row justify="center">
      <v-col cols="12" md="4">
        <ProfileDisplay :user-profile="profileData" />
      </v-col>
      <v-col cols="12" md="8">
        <v-row  justify-md="end" justify="center">
          <v-card width="95%" color="green">
            <v-tabs v-model="tab" background-color="dark" dark id="account-tab-slider">
              <v-tab v-for="item in tabItems" :key="item.tab">
                {{ item.tab }}
              </v-tab>
            </v-tabs>
            <v-tabs-items v-model="tab" id="account-tab-slider">
              <v-tab-item v-for="(item, index) in tabItems" :key="index">
                <component :is="item.content"></component>
              </v-tab-item>
            </v-tabs-items>
          </v-card>
        </v-row>
      </v-col>
    </v-row>
    <v-overlay v-if="creationOverlayActive">
      <CreateGroup />
    </v-overlay>
    <v-overlay v-if="editOverlayActive">
      <v-row>
        <EditGroup />
      </v-row>
    </v-overlay>
  </v-container>
</template>

<script lang="ts">
  import {Component, Vue, Watch} from "nuxt-property-decorator";
  import WatchGroup from "~/components/Account/watch-group.vue";
  import CreateGroup from "~/components/Account/create-group.vue";
  import {watchgroup} from "~/store/watchgroup";
  import EditGroup from "~/components/Account/edit-group.vue";
  import WatchGroupDisplay from "~/components/Account/watch-group-display.vue";
  import ProfileDisplay from "~/components/Account/profile-display.vue";
  import InviteAsFollower from "~/components/Account/invite-as-follower.vue";

  @Component({
    name: "account",
    components: {
      WatchGroup,
      CreateGroup,
      EditGroup,
      WatchGroupDisplay,
      ProfileDisplay,
    },
  })
  export default class Account extends Vue{
    private tab = null;
    private profileData = {};

    private tabItems = [
      {tab: "Watch Groups Display", content: WatchGroupDisplay},
    ]

    private nonListedTabItems = [
      {tab: "...", content: WatchGroupDisplay}
    ]

    async created() {
      await this.$store.getters['getInitPromise'];
      await this.$store.dispatch('watchgroup/getUserWatchGroups');
      await this.$store.dispatch('user/getUserProfile');
    }

    get creationOverlayActive():boolean{
      return (this.$store.state.watchgroup as watchgroup).creationOverlayActive;
    }

    get editOverlayActive():boolean{
      return (this.$store.state.watchgroup as watchgroup).editOverlayActive;
    }
  }
</script>

<style scoped>
#account-tab-slider{
  max-width: 90%;
}
</style>
