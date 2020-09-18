﻿import {ActionTree, GetterTree, MutationTree} from 'vuex';
import { RootState } from "~/store";

interface initalStateModel {
  creationOverlayActive: boolean
}

const initState: initalStateModel = {
  creationOverlayActive: false,
}

export const state:any = initState;

export type watchgroup = ReturnType<typeof state>

export const getters: GetterTree<watchgroup, RootState> = {

}


export const mutations: MutationTree<watchgroup> = {
  TOGGLE_CREATION_OVERLAY: (state) => state.creationOverlayActive = ! state.creationOverlayActive,
};

export const actions: ActionTree<watchgroup, RootState> = {

};
