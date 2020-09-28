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
        <v-btn text color="white" @click="deleteMessage">Delete</v-btn>
      </v-list-item-action-text>
    </v-list-item-content>
  </v-list-item>
</template>

<script lang="ts">
  import {Component, Prop, Vue} from "nuxt-property-decorator";
  import {InboxMessage} from "~/assets/interface-models";

    @Component({})
    export default class GeneralMessage extends Vue{
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

      private deleteMessage():void{
        this.$store.dispatch('inbox/deleteGeneralMessage', this.message);
      }
    }
</script>

<style scoped>

</style>
