
import { ActionTree, MutationTree } from 'vuex'

const initState = () => ({

});

export const state = initState;

export type RootState = ReturnType<typeof state>

export const mutations: MutationTree<RootState> = {

}

export const actions: ActionTree<RootState, RootState> = {
  async nuxtServerInit ({ commit, dispatch}) {

  },
  nuxtClientInit({dispatch}, context){
    //return dispatch('auth/init');
  }
}
