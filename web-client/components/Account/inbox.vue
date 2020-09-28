<template>
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


    @Component({
      components: {
        GeneralMessage,
        InviteMessage,
      }
    })
    export default class Inbox extends Vue{
      private inviteMessages: Array<InboxMessage> = [];
      private generalMessages: Array<InboxMessage> = []
      private messageCount: number = 0;

      private inbox:boolean = false;

      private created(){
        this.$axios.get('api/profile/inbox')
          .then(response => {
            const messages:Array<InboxMessage> = response.data;
            this.messageCount = messages.length;
            messages.forEach(x => x.messageType === 1? this.inviteMessages.push(x): this.generalMessages.push(x));
          })
      }

      private toggleInbox():void{
        this.inbox = ! this.inbox;
      }
    }
</script>

<style scoped>

</style>
