﻿import { ActionTree, MutationTree, GetterTree } from 'vuex'
import { RootState } from "~/store";
import {InboxMessage, WatchGroupInviteResponseMessage} from "~/assets/interface-models";


const initState = () => ({
  watchGroupInviteMessage: [] as Array<InboxMessage>,
  generalMessage: [] as Array<InboxMessage>,
  messageCount: 0 as number,
});

export const state = initState;

export type inbox = ReturnType<typeof state>


export const mutations: MutationTree<inbox> = {
  ADD_GENERAL_MESSAGE: (state, message: InboxMessage) => state.generalMessage.push(message),
  ADD_WATCH_GROUP_INVITE_MESSAGE: (state, message: InboxMessage) => state.watchGroupInviteMessage.push(message),
  SET_MESSAGE_COUNT: (state, count:number) => state.messageCount = count,
  REMOVE_MESSAGE: (state, message: InboxMessage) => {
    if(message.messageType === 0) state.generalMessage.filter((x) => x.messageId !== message.messageId)
    if(message.messageType === 1) state.watchGroupInviteMessage.filter((x) => x.messageId !== message.messageId)
  }
}


export const actions: ActionTree<inbox, RootState> = {
  LoadInboxMessages({commit}):Promise<void>{
    return this.$axios.get('api/profile/inbox')
      .then((response) => {
        const messages: Array<InboxMessage> = response.data;
        commit('SET_MESSAGE_COUNT', messages.length);
        messages.forEach(x => {
          if(x.messageType === 0) commit('ADD_GENERAL_MESSAGE', x);
          if(x.messageType === 1) commit('ADD_WATCH_GROUP_INVITE_MESSAGE', x);
        })
    })
      .catch(err => console.log(err));
  },
  watchGroupInviteResponse({state, commit}, {payload, message}){
    return this.$axios.put('api/watchgroup/invite/response', payload)
      .then(() => {
        commit('REMOVE_MESSAGE', message);
      })
      .catch(err => console.log(err));
  },
  deleteGeneralMessage({commit}, message:InboxMessage){
    return this.$axios.delete(`api/profile/inbox?id=${message.messageId}`)
      .then(() => {
        commit('REMOVE_MESSAGE', message);
      });
  }
}
