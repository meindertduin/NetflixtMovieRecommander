import {watchlist} from "~/store/watchlist";

﻿import {ActionTree, MutationTree} from 'vuex';
import { RootState } from "~/store";

const initState = () => ({
  recommendations: [],
});

export const state = initState;

export type recommendation = ReturnType<typeof state>

export const mutations: MutationTree<recommendation> = {
  RESET: (state) => Object.assign(state, initState),
  SET_RECOMMENDATIONS: (state, recommendations) => state.recommendations = recommendations,
};

export const actions: ActionTree<recommendation, RootState> = {
  async GetRecommendations({dispatch, state, commit}, {watchedItems, genres, type}){
    const res = await this.$axios.$post('/api/recommendation/watchlist',
      {
        watchedItems,
        genres: genres,
        type: type
      });
    commit('SET_RECOMMENDATIONS', res)
  }
}
