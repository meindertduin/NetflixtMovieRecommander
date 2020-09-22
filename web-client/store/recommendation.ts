import {watchlist} from "~/store/watchlist";

﻿import {ActionTree, GetterTree, MutationTree} from 'vuex';
import { RootState } from "~/store";

const initState = () => ({
  recommendations: [],
  currentLoadedRecommendations: [],
  currentRecIndex: 0,
  selectedGenres: [],
  selectedType: ""
});

export const state = initState;

export type recommendation = ReturnType<typeof state>

export const getters: GetterTree<recommendation, RootState> = {
  getSelectedGenres(state){
    return state.selectedGenres;
  },
  getSelectedType(state){
    return state.selectedType;
  }
}

export const mutations: MutationTree<recommendation> = {
  RESET: (state) => {
    state.currentLoadedRecommendations = [];
    state.recommendations = [];
    state.currentRecIndex = 0;
    state.selectedGenres = [];
    state.selectedType = "";
  },

  SET_RECOMMENDATIONS: (state, recommendations) =>{
    if(state.recommendations.length < 1){
      state.recommendations = recommendations;
    }
    else{
      state.recommendations.concat(recommendations);
    }
  },

  SET_INITIAL_CURRENT_LOADED: (state) => state.currentLoadedRecommendations = state.recommendations.slice(0, 5),

  INC_CURR_REC_INDEX: (state, amount) => state.currentRecIndex += amount,

  SET_CURR_RECOMMENDATIONS: (state, {from, to}) => state.currentLoadedRecommendations = state.recommendations.slice(from, to),

  SET_TYPE: (state, type) => state.selectedType = type,

  SET_GENRES: (state, genres) => state.selectedGenres = genres,
};

export const actions: ActionTree<recommendation, RootState> = {
  async GetRecommendations({dispatch, state, commit}, {watchedItems, genres, type}){
    const res = await this.$axios.$post('/api/recommendation/watchlist',
      {
        watchedItems,
        genres: genres,
        type: type
      });
    console.log(res);
    commit('SET_RECOMMENDATIONS', res)
  },
}
