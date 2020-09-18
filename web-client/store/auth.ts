﻿import { ActionTree, MutationTree, GetterTree } from 'vuex'
import { RootState } from "~/store";
import {User} from "oidc-client";
import {recommendation} from "~/store/recommendation";

const ROLES = {
  MODERATOR: "Mod",
}



const initState = () => ({
  user: null as User | null,
  profile: null as any,
  authenticated: false as boolean,
});


export const state:any = initState;


export type auth = ReturnType<typeof state>

export const getters: GetterTree<auth, RootState> = {

}

export const mutations: MutationTree<auth> = {

}

export const actions: ActionTree<auth, RootState> = {
  async setBearer({commit}, access_token){
    this.$axios.setToken(access_token);
  },
}
