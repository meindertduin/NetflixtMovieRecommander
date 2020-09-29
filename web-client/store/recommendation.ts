import {watchlist} from "~/store/watchlist";

﻿import {ActionTree, GetterTree, MutationTree} from 'vuex';
import { RootState } from "~/store";
import {Recommendation} from "~/assets/interface-models";

const initState = () => ({
  recommendations: [] as Array<Recommendation>,
  recommendationsIndex: 0 as number,
  selectedGenres: [] as Array<string>,
  selectedType: "both" as string,
  index: 0 as number,
});

export const state = initState;

export type recommendation = ReturnType<typeof state>

export const getters: GetterTree<recommendation, RootState> = {

}

export const mutations: MutationTree<recommendation> = {
  SET_RECOMMENDATIONS: (state, recommendations:Array<Recommendation>) => {
    state.recommendations = recommendations;
  },

  ADD_RECOMMENDATIONS: (state, recommendations:Array<Recommendation>) => {
    recommendations.forEach(x => state.recommendations.push(x));
  },

  SET_TYPE: (state, type: string) => state.selectedType = type,

  SET_GENRES: (state, genres) => state.selectedGenres = genres,

  SET_INDEX: (state, value: number) => state.index = value,

  INC_INDEX: (state) => state.index++,
};

export const actions: ActionTree<recommendation, RootState> = {
  GetRecommendations({dispatch, state, commit}, {watchedItems, reset}){
    return this.$axios.post('/api/recommendation/watchlist',
      {
        watchedItems: watchedItems,
        genres: state.selectedGenres,
        index: state.index,
        type: state.selectedType,
      })
      .then(({data}) => {
        if(reset){
          commit('SET_RECOMMENDATIONS', data);
        }
        else{
          commit('ADD_RECOMMENDATIONS', data);
        }
      })
      .catch(err => console.log(err));
  },
}
