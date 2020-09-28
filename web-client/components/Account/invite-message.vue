<template>
  <v-list-item>
    <v-list-item-avatar>
      <img :src="message.sender.avatarUrl? message.sender.avatarUrl: 'default_profile.png'" alt="">
    </v-list-item-avatar>
    <v-list-item-content>
      <v-list-item-title>
        {{message.title}}
      </v-list-item-title>
      <v-list-item-action-text>
        {{dateString}}
        {{message.description}}
        <v-btn text color="green" @click="accept">Accept</v-btn>
        <v-btn text color="white" @click="decline">Decline</v-btn>
      </v-list-item-action-text>
    </v-list-item-content>
  </v-list-item>
</template>

<script lang="ts">
  import {Component, Prop, Vue} from "nuxt-property-decorator";
  import {InboxMessage, WatchGroupInviteResponseMessage} from "~/assets/interface-models";

    @Component({})
    export default class InviteMessage extends Vue{
      @Prop({ type: Object, required: true}) readonly message !:InboxMessage;

      get dateString():string{
        const monthNames = ["January", "February", "March", "April", "May", "June",
          "July", "August", "September", "October", "November", "December"
        ];
        let date = new Date(this.message.dateSend);
        let days = date.getDay();
        let month = date.getMonth();

        return "Send: " + days.toString() + " " + monthNames[month];
      }

      private accept(): void{
        const payload: WatchGroupInviteResponseMessage = {
          messageId: this.message.messageId,
          accepted: true,
          inviterId: this.message.sender.id,
        }
        this.$store.dispatch('inbox/watchGroupInviteResponse', {payload, message: this.message})
      }

      private decline():void{
        const payload: WatchGroupInviteResponseMessage = {
          messageId: this.message.messageId,
          accepted: true,
          inviterId: this.message.sender.id,
        }
        this.$store.dispatch('inbox/watchGroupInviteResponse', {payload, message: this.message})
      }
    }
</script>

<style scoped>

</style>
