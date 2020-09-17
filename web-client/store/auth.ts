﻿import { ActionTree, MutationTree, GetterTree } from 'vuex'
import { RootState } from "~/store";
import {User} from "oidc-client";
import {recommendation} from "~/store/recommendation";

const ROLES = {
  MODERATOR: "Mod",
}


interface StateLayout {
  user: User | null,
  profile: any | null,
  authenticated: boolean,
}
const initState: StateLayout = {
  user: null,
  profile: null,
  authenticated: false,
}

const state = () => (
  initState
)



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
