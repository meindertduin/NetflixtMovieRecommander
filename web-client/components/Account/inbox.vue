﻿<template>
  <v-card>
    <v-card-title>
      <v-row justify="center">
        Inbox
      </v-row>
    </v-card-title>
    <v-card-text>
      <v-row justify="center">
        <v-btn v-if="messageCount > 0" text @click="toggleInbox">
          click to open messages
        </v-btn>
        <div v-else class="text-button white--text">Your inbox is empty</div>
        <v-expand-transition>
          <v-list v-if="inbox">
            <InviteMessage v-for="(invite, index) in inviteMessages" :key="index" :message="invite"/>
            <GeneralMessage v-for="(message, index) in generalMessages" :key="index" :message="message" />
          </v-list>
        </v-expand-transition>
      </v-row>
    </v-card-text>
  </v-card>
</template>

<script lang="ts">
  import {Component, Vue} from "nuxt-property-decorator";
  import InviteMessage from "~/components/Account/invite-message.vue";
  import {InboxMessage} from "~/assets/interface-models";
  import GeneralMessage from "~/components/Account/general-message.vue";
  import {inbox} from '~/store/inbox'

    @Component({
      components: {
        GeneralMessage,
        InviteMessage,
      }
    })
    export default class Inbox extends Vue{
      get messageCount():number{
        return (this.$store.state.inbox as inbox).messageCount;
      }

      get inviteMessages():Array<InboxMessage>{
        return (this.$store.state.inbox as inbox).watchGroupInviteMessage;
      }

      get generalMessages():Array<InboxMessage>{
        return (this.$store.state.inbox as inbox).generalMessage;
      }

      private inbox:boolean = false;

      async created() {
        await this.$store.dispatch('inbox/LoadInboxMessages');
      }

      private toggleInbox():void{
        this.inbox = ! this.inbox;
      }
    }
</script>

<style scoped>

</style>
