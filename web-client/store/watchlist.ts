
﻿import {ActionTree, GetterTree, MutationTree} from 'vuex';
import { RootState } from "~/store";

const initState = () => ({
  overlayActive: true,
  showGuide: false,
  watchedItems: [],
  selectedGenres: [],
  selectedType: null,
  uploadPromise: null,
});

export const state = initState;

export type watchlist = ReturnType<typeof state>

export const getters: GetterTree<watchlist, RootState> = {
  getWatchedItems(state){
    return state.watchedItems;
  }
}

export const mutations: MutationTree<watchlist> = {
  TOGGLE_OVERLAY: (state) => state.overlayActive = ! state.overlayActive,
  TOGGLE_GUIDE: (state) => state.showGuide = ! state.showGuide,
  SET_WATCHED_ITEMS: (state, watchedItems) => {
    console.log(watchedItems);
    state.watchedItems = watchedItems;
  },
  ADD_TO_WATCHED_ITEMS: (state, extraItems) => state.watchedItems.concat(extraItems),
};

export const actions: ActionTree<watchlist, RootState> = {
  async uploadWatchLists({dispatch, commit}, {form}) {
    try{
      const res = await this.$axios.post('/api/watchlist', form, {
        headers: {
          'Content-Type': 'multipart/form-data'
        },
      })
      commit('SET_WATCHED_ITEMS', res.data);
      return res.status;
    } catch(err){
      return err.response.status;
    }

  },
};
