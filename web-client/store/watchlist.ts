
﻿import {ActionTree, MutationTree} from 'vuex';
import { RootState } from "~/store";

const initState = () => ({
  overlayActive: false,
  watchedItems: [],
  watchedItemsLoaded: false,
  selectedGenres: [],
});

export const state = initState;

export type watchlist = ReturnType<typeof state>

export const mutations: MutationTree<watchlist> = {
  TOGGLE_OVERLAY: (state) => state.overlayActive = !state.overlayActive,
  SET_WATCHED_ITEMS: (state, watchedItems) => {
    state.watchedItems = watchedItems;
    state.watchedItemsLoaded = true;
  },
};

export const actions: ActionTree<watchlist, RootState> = {
  async uploadWatchLists({dispatch, commit}, {form}){
    const res = await this.$axios.$post('/api/watchlist', form);
    console.log(res);
    commit('SET_WATCHED_ITEMS', res);
    dispatch('GetRecommendations', {watchedItems: res});
  },
  async GetRecommendations({dispatch, state}, {watchedItems}){
    console.log(watchedItems)
    console.log({watchedItems, Genres: state.selectedGenres})
    const res = await this.$axios.$post('/api/recommendation/watchlist', {watchedItems, Genres: ["War"]})
    console.log(res);
  }
};
