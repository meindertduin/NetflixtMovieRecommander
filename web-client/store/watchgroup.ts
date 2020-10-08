import {
  Recommendation,
  UpdateWatchGroupModel,
  WatchGroupModel,
  WatchGroupRecommendationForm
} from "~/assets/interface-models";

﻿import {ActionTree, GetterTree, MutationTree} from 'vuex';
import { RootState } from "~/store";
import WatchGroup from "~/components/Account/watch-group.vue";
import {recommendation} from "~/store/recommendation";


const initState = () => ({
  creationOverlayActive: false as boolean,
  editOverlayActive: false as boolean,
  currentSelectedWatchGroup: null as WatchGroupModel | null,
  watchGroupRecommendations: [] as Array<Recommendation>,

  watchGroups: [] as Array<WatchGroupModel>,

  recommendationsIndex: 0 as number,
  selectedGenres: [] as Array<string>,
  selectedType: "both" as string,

  alreadyLoadedRecommendations: [] as Array<number>,
});

export const state:any = initState;

export type watchgroup = ReturnType<typeof state>

export const getters: GetterTree<watchgroup, RootState> = {

}


export const mutations: MutationTree<watchgroup> = {
  TOGGLE_CREATION_OVERLAY: (state) => state.creationOverlayActive = ! state.creationOverlayActive,
  OPEN_EDIT_OVERLAY: (state, selectedWatchGroup:WatchGroupModel) =>{
    state.editOverlayActive = ! state.editOverlayActive;
    state.currentSelectedWatchGroup = selectedWatchGroup;
  },
  CLOSE_EDIT_OVERLAY: (state) => {
    state.currentSelectedWatchGroup = null;
    state.editOverlayActive = ! state.editOverlayActive;
  },

  SET_WATCH_GROUP_RECOMMENDATIONS: (state, recommendations:Array<Recommendation>) => {
    recommendations.forEach(x => state.alreadyLoadedRecommendations.push(x.id));
    state.watchGroupRecommendations = recommendations;
  },

  ADD_WATCH_GROUP_RECOMMENDATIONS: (state, recommendations:Array<Recommendation>) => {
    recommendations.forEach(x => {
      state.watchGroupRecommendations.push(x)
      state.alreadyLoadedRecommendations.push(x.id);
    });
  },
  REMOVE_RECOMMENDATION_FROM_DISPLAY: (state, recommendation:Recommendation) =>{
    console.log(recommendation.id);
    state.watchGroupRecommendations =  state.watchGroupRecommendations.filter((x:Recommendation) => x.id !== recommendation.id);
    console.log(state.watchGroupRecommendations);
  },

  SET_RECOMMENDATIONS_INDEX: (state, index:number) => state.recommendationsIndex = index,

  INC_RECOMMENDATIONS_INDEX: (state) => state.recommendationsIndex++,

  SET_SELECTED_GENRES: (state, genres:Array<string>) => state.selectedGenres = genres,
  SET_SELECTED_TYPE: (state, type:string) => state.selectedType = type,

  SET_USER_WATCH_GROUPS: (state, watchGroups:Array<WatchGroup>) => state.watchGroups = watchGroups,

  REMOVE_WATCH_GROUP: (state, watchGroupId:string) => state.watchGroups = state.watchGroups
    .filter((group:WatchGroupModel) => group.id !== watchGroupId),

  RESET_ALREADY_LOADED: (state) => state.alreadyLoadedRecommendations = [],
};

interface editTitleModel{
  title: string,
  id: string,
}

interface editDescriptionModel{
  description: string,
  id: string,
}

interface editAddedNamesModel{
  addedNames: Array<string>,
  id: string,
}


export const actions: ActionTree<watchgroup, RootState> = {
  editGroup({dispatch}, payload:UpdateWatchGroupModel){
    return this.$axios.put('/api/watchgroup/edit', payload)
      .then(() =>{
        dispatch('getUserWatchGroups');
    })
      .catch(err => console.log(err));
  },
  createGroup({commit, dispatch}, {groupForm}):Promise<any>{
      return this.$axios.post('/api/watchgroup/create', groupForm)
        .then(response => {
          dispatch('getUserWatchGroups');
          return response;
        })
        .catch(err => console.log(err));
  },
  getRecomemendations({commit, state}, {route, reset}):Promise<void>{
    const payload: WatchGroupRecommendationForm = {
      genres: state.selectedGenres,
      index: state.recommendationsIndex,
      type: state.selectedType,
      alreadyLoaded: state.alreadyLoadedRecommendations,
    }
    return this.$axios.post(`api/watchgroup/${route}`, payload)
      .then(({data}) =>{
        if(reset === true){
          commit('SET_WATCH_GROUP_RECOMMENDATIONS', data);
        }
        else {
          commit('ADD_WATCH_GROUP_RECOMMENDATIONS', data);
        }

      })
      .catch(error => {
        console.log(error);
      });
  },
  addAlreadyWatchedItem({commit}, {groupId, title}): Promise<void> {
    return this.$axios.post('api/watchgroup/watch-item', {
      groupId: groupId,
      watchItemTitle: title,
    });
  },
  getUserWatchGroups({commit}):Promise<void>{
    return this.$axios.get('api/watchgroup')
      .then(({data}) => {
      commit('SET_USER_WATCH_GROUPS', data)
    }).catch(err => {
      console.log(err);
    })
  },
  deleteWatchGroup({commit}, watchGroupId:string):Promise<void>{
    return this.$axios.delete('api/watchgroup', {
      params: {
        id: watchGroupId,
      },
    })
      .then(() => {
        commit('REMOVE_WATCH_GROUP', watchGroupId);
    })
      .catch((err) => {
        console.log(err);
      })
  }
};
