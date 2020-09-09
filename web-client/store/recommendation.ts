import {watchlist} from "~/store/watchlist";

﻿import {ActionTree, MutationTree} from 'vuex';
import { RootState } from "~/store";

const initState = () => ({
  selectedGenres: [],
  selectedType: null,
  recommendations: [],
});

export const state = initState;

export type recommendation = ReturnType<typeof state>

export const mutations: MutationTree<recommendation> = {
  RESET: (state) => Object.assign(state, initState),
  SET_RECOMMENDATIONS: (state, recommendations) => state.recommendations = recommendations,
};

export const actions: ActionTree<recommendation, RootState> = {
  async GetRecommendations({dispatch, state, commit}, {watchedItems}){
    console.log(watchedItems)
    console.log({watchedItems, Genres: state.selectedGenres})
    const res = await this.$axios.$post('/api/recommendation/watchlist',
      {
        watchedItems,
        Genres: state.selectedGenres,
        type: state.selectedType
      });
    console.log(res);
    commit('SET_RECOMMENDATIONS', res)
  }
}
