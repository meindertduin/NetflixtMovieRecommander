import {UpdateWatchGroupModel, WatchGroupModel, WatchGroupRecommendationForm} from "~/assets/interface-models";

﻿import {ActionTree, GetterTree, MutationTree} from 'vuex';
import { RootState } from "~/store";


const initState = () => ({
  creationOverlayActive: false as boolean,
  editOverlayActive: false as boolean,
  currentSelectedWatchGroup: null as WatchGroupModel | null,
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
  }
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
    const res = this.$axios.put('/api/watchgroup/edit', payload);
  },
  getRecomemendations({commit}, {payload, route}){
    const res = this.$axios.post(`api/watchgroup/${route}`, payload);
    console.log(res);
  }
};
