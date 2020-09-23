
import {ActionTree, GetterTree, MutationTree} from 'vuex'
import {RootState} from "~/store/index";
import {Profile} from "~/assets/interface-models";

const initState = () => ({
  userProfile: null as Profile | null
});

export const state = initState;

export type user = ReturnType<typeof state>

export const getters: GetterTree<RootState, user> = {

}

export const mutations: MutationTree<user> = {
  SET_USER_PROFILE: (state, profile:Profile) => state.userProfile = profile
}

export const actions: ActionTree<RootState, user> = {
  async getUserProfile({commit}){
    this.$axios.get('api/profile').then(({data}) => {
      commit('SET_USER_PROFILE', data);
      return data;
    })
  }
}
