import get = Reflect.get;

﻿import { ActionTree, MutationTree, GetterTree } from 'vuex'
import { RootState } from "~/store";
import {User} from "oidc-client";
import {recommendation} from "~/store/recommendation";
import {Getter} from "nuxt-property-decorator";
import {Profile} from "~/assets/interface-models";

const ROLES = {
  MODERATOR: "Mod",
}



const initState = () => ({
  user: null as User | null,
  loading: true as boolean,
  userProfile: null as Profile | null
});


export const state:any = initState;


export type auth = ReturnType<typeof state>

export const getters: GetterTree<auth, RootState> = {
  authenticated: (state) => !state.loading && state.user != null,
  moderator: (state, getters) => getters.authenticated && state.user.profile.role === ROLES.MODERATOR,
}

export const mutations: MutationTree<auth> = {
  SAVE_USER: (state, user:User) => state.user = user,
  FINISH: (state) => state.loading = false,
  SET_PROFILE: (state, profile:Profile) => state.userProfile = profile,
}

export const actions: ActionTree<auth, RootState> = {
  async setBearer({commit}, access_token){
    this.$axios.setToken(access_token);
  },
  async initialize({commit}){
    return this.$auth.getUser().then(user => {
      if(user){
        this.$axios.setToken(`Bearer ${user.access_token}`);
        commit('SAVE_USER', user);
        console.log("user Got")
        this.$axios.get('api/profile')
          .then(({data}) => {
            commit('SET_PROFILE', data);
          })
      }
    })
      .catch(err => {
        console.log(err);
      })
      .finally(() => {
        commit('FINISH');
      })
  }
}
