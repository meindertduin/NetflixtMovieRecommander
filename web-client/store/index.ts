
import {ActionTree, GetterTree, MutationTree} from 'vuex'

const initState = () => ({
  initPromise: null
});

export const state = initState;

export type RootState = ReturnType<typeof state>

export const getters: GetterTree<RootState, RootState> = {
  getInitPromise: (state) => state.initPromise,
}

export const mutations: MutationTree<RootState> = {
  SET_INIT_PROMISE: (state, promise) => state.initPromise = promise,
}

export const actions: ActionTree<RootState, RootState> = {
  async nuxtServerInit ({ commit, dispatch}) {

  },
  clientInit({dispatch, commit}, context){
    const initPromise = dispatch('auth/initialize');
    commit('SET_INIT_PROMISE', initPromise);
    return initPromise;
  }
}
