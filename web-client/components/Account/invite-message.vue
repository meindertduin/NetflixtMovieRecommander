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
        <v-btn text color="green" @click="response(true)">Accept</v-btn>
        <v-btn text color="white" @click="response(false)">Decline</v-btn>
      </v-list-item-action-text>
    </v-list-item-content>
  </v-list-item>
</template>

<script lang="ts">
  import {Component, Prop, Vue} from "nuxt-property-decorator";
  import {InboxMessage, WatchGroupInviteResponseMessage} from "~/assets/interface-models";
  import DateStringHelper from "~/assets/date-string-helper";

    @Component({})
    export default class InviteMessage extends Vue{
      @Prop({ type: Object, required: true}) readonly message !:InboxMessage;

      get dateString():string{
        let date = new Date(this.message.dateSend);
        const helper = new DateStringHelper();
        return helper.convertToDayMonth(date);
      }

      private response(accepted: boolean): void{
        const payload: WatchGroupInviteResponseMessage = {
          messageId: this.message.messageId,
          accepted,
          inviterId: this.message.sender.id,
        }
        this.$store.dispatch('inbox/watchGroupInviteResponse', {payload, message: this.message})
      }

    }
</script>

<style scoped>

</style>
