import {WatchGroupModel} from "~/assets/interface-models";

﻿import {ActionTree, GetterTree, MutationTree} from 'vuex';
import { RootState } from "~/store";


const initState = () => ({
  creationOverlayActive: false as boolean,
  editOverlayActive: false as boolean,
  currentSelectedWatchGroup: null as WatchGroupModel | null
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
  editGroupTitle({dispatch}, payload:editTitleModel) {

  },
  editGroupDescription({dispatch}, payload:editDescriptionModel){

  },
  editGroupAddedNames({dispatch}, payload:editAddedNamesModel){
    console.log(payload);
  },
};
