<template>
  <v-app dark>
    <v-navigation-drawer
      v-model="drawer"
      app
      clipped
    >
      <v-list dense>
        <v-list-item link>
          <v-list-item-action>
            <v-icon>mdi-alpha-h-box</v-icon>
          </v-list-item-action>
          <v-list-item-content @click="navigate('/')">
            <v-list-item-title>Home</v-list-item-title>
          </v-list-item-content>
        </v-list-item>
        <v-list-item link>
          <v-list-item-action>
            <v-icon>mdi-account</v-icon>
          </v-list-item-action>
          <v-list-item-content @click="navigate('/account')">
            <v-list-item-title>Account</v-list-item-title>
          </v-list-item-content>
        </v-list-item>
        <v-list-item>
          <v-list-item-content>
            <v-card>
              <v-card-title>
                <v-row justify="center">
                  Inbox
                </v-row>
              </v-card-title>
              <v-card-text>
                <v-row justify="center">
                  <v-btn v-if="true" text @click="toggleInbox">
                    click to open messages
                  </v-btn>
                  <div v-else class="text-button white--text">Your inbox is empty</div>
                  <v-expand-transition>
                    <v-list v-if="inbox">
                      <v-list-item>
                        <v-list-item-avatar>
                          <img src="/default_profile.jpg" alt="">
                        </v-list-item-avatar>
                        <v-list-item-content>
                          <v-list-item-title>
                            Invite From kaas
                          </v-list-item-title>
                          <v-list-item-action-text>
                            <v-btn text color="green">Accept</v-btn>
                            <v-btn text color="white">Decline</v-btn>
                          </v-list-item-action-text>
                        </v-list-item-content>
                      </v-list-item>
                    </v-list>
                  </v-expand-transition>
                </v-row>
              </v-card-text>
            </v-card>
          </v-list-item-content>
        </v-list-item>
      </v-list>
    </v-navigation-drawer>


      <v-app-bar app dense clipped-left>
        <v-app-bar-nav-icon @click.stop="drawer = !drawer"></v-app-bar-nav-icon>
        <v-toolbar-title>Netflix recommander</v-toolbar-title>
        <v-divider></v-divider>
        <v-skeleton-loader :loading="loadingState" transition="fade-transition" type="button">
          <v-btn v-if="authenticated" @click="$auth.signoutRedirect()">
            <v-icon>mdi-account-circle</v-icon>Sign Out
          </v-btn>
          <v-btn v-else @click="$auth.signinRedirect()">
            <v-icon>mdi-account-circle</v-icon>Sign in
          </v-btn>
        </v-skeleton-loader>
      </v-app-bar>
    <v-main>
          <nuxt />
    </v-main>
  </v-app>
</template>

<script lang="ts">
  import {Component, Vue} from "nuxt-property-decorator";
  import {auth} from "~/store/auth";

@Component({})
export default class DefaultLayout extends Vue
{
  private drawer = null;
  private inbox:boolean = false;

  private toggleInbox():void{
    this.inbox = ! this.inbox;
  }

  get loadingState(){
    return (this.$store.state.auth as auth).loading;
  }

  get authenticated(){
    return this.$store.getters['auth/authenticated'];
  }

  get getModState(){
    return this.$store.getters['auth/moderator'];
  }

  private navigate(path:string){
    this.$router.push(path);
  }
}
</script>
