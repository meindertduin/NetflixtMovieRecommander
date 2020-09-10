
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
    state.watchedItems = watchedItems;
  },
  ADD_TO_WATCHED_ITEMS: (state, extraItems) => state.watchedItems.concat(extraItems),
};

export const actions: ActionTree<watchlist, RootState> = {
  async uploadWatchLists({dispatch, commit}, {watchLists}) {
    await this.$axios.post('/api/watchlist', watchLists,
      {
        headers:
          {
            contentType: "multipart/form-data"
          }
      })
      .then(({data}) => {
        commit('SET_WATCHED_ITEMS', data);
      })
      .catch(err => {
        console.log(err);
      });
  },
};
