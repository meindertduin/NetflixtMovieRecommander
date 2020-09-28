import {
  Recommendation,
  UpdateWatchGroupModel,
  WatchGroupModel,
  WatchGroupRecommendationForm
} from "~/assets/interface-models";

﻿import {ActionTree, GetterTree, MutationTree} from 'vuex';
import { RootState } from "~/store";
import WatchGroup from "~/components/Account/watch-group.vue";


const initState = () => ({
  creationOverlayActive: false as boolean,
  editOverlayActive: false as boolean,
  currentSelectedWatchGroup: null as WatchGroupModel | null,
  watchGroupRecommendations: null as null | Recommendation,

  watchGroups: [] as Array<WatchGroupModel>,

  recommendationsIndex: 0 as number,
  selectedGenres: [] as Array<string>,
  selectedType: "both" as string,
  seed: "" as string,
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
  SET_WATCH_GROUP_RECOMMENDATIONS: (STATE, recommendations:Array<Recommendation>) => {
    if(state.watchGroupRecommendations.length < 1){
      state.watchGroupRecommendations = recommendations;
    }
    else{
      state.watchGroupRecommendations.concat(recommendations);
    }
  },

  SET_RECOMMENDATIONS_INDEX: (state, index:number) => state.recommendationsIndex = index,
  INC_RECOMMENDATIONS_INDEX: (state) => state.recommendationsIndex++,
  SET_SELECTED_GENRES: (state, genres:Array<string>) => state.selectedGenres = genres,
  SET_SELECTED_TYPE: (state, type:string) => state.selectedType = type,
  SET_SEED: (state, seed:string) => state.seed = seed,

  SET_USER_WATCH_GROUPS: (state, watchGroups:Array<WatchGroup>) => state.watchGroups = watchGroups,

  REMOVE_WATCH_GROUP: (state, watchGroupId:string) => state.watchGroups = state.watchGroups.filter((group:WatchGroupModel) => group.id !== watchGroupId),

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
  async editGroup({dispatch}, payload:UpdateWatchGroupModel){
    this.$axios.put('/api/watchgroup/edit', payload).then(() =>{
      dispatch('getUserWatchGroups');
    });
  },
  createGroup({commit, dispatch}, {groupForm}):Promise<any>{
      return this.$axios.post('/api/watchgroup/create', groupForm)
        .then(response => {
          dispatch('getUserWatchGroups');
          return response;
        })
        .catch(err => console.log(err));
  },

  async getRecomemendations({commit, state}, {route}):Promise<void>{
    const payload: WatchGroupRecommendationForm = {
      genres: state.selectedGenres,
      index: state.index,
      seed: state.seed,
      type: state.selectedType,
    }

    return new Promise((resolve, reject) => {
      this.$axios.post(`api/watchgroup/${route}`, payload)
        .then(({data}) =>{
          commit('SET_WATCH_GROUP_RECOMMENDATIONS', data);
          resolve()
        })
        .catch(error => {
          reject();
        });
    })
  },
  async getUserWatchGroups({commit}):void{
    this.$axios.get('api/watchgroup').then(({data}) => {
      console.log(data);
      commit('SET_USER_WATCH_GROUPS', data)
    }).catch(err => {
      console.log(err);
    })
  },
  deleteWatchGroup({commit}, watchGroupId:string){
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
