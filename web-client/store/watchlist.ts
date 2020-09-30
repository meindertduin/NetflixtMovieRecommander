
﻿import {ActionTree, GetterTree, MutationTree} from 'vuex';
import { RootState } from "~/store";

const initState = () => ({
  overlayActive: true,
  showGuide: false,
  watchedItems: [] as Array<string>,
  selectedGenres: [],
  selectedType: null,
  uploadPromise: null,

  uploadCount: 0 as number,
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
  SET_WATCHED_ITEMS: (state, watchedItems:Array<string>) => {
    console.log(watchedItems);
    watchedItems.forEach(x => {
      if(!state.watchedItems.includes(x)){
        state.watchedItems.push(x);
      }
    });
  },
  ADD_TO_WATCHED_ITEMS: (state, extraItems) => state.watchedItems.concat(extraItems),
  ADD_UPLOAD_COUNT : (state, value:number) => state.uploadCount += value,
  SET_UPLOAD_COUNT: (state, value:number) => state.uploadCount = value,
};

export const actions: ActionTree<watchlist, RootState> = {
  uploadWatchLists({dispatch, commit}, {form, count}) {
    return this.$axios.post('/api/watchlist', form, {
      headers: {
        'Content-Type': 'multipart/form-data'
      },
    })
      .then((response) => {
        commit('SET_WATCHED_ITEMS', response.data);
        commit('ADD_UPLOAD_COUNT', count);
      })
      .catch(err => console.log(err));
  }
};
