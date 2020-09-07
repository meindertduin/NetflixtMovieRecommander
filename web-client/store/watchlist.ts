
﻿import {ActionTree, MutationTree} from 'vuex';
import { RootState } from "~/store";

const initState = () => ({
  overlayActive: false,
});

export const state = initState;

export type watchlist = ReturnType<typeof state>

export const mutations: MutationTree<watchlist> = {
  TOGGLE_OVERLAY: (state) => state.overlayActive = !state.overlayActive,
};

export const actions: ActionTree<watchlist, RootState> = {
  async uploadWatchLists({dispatch}, {form}){
    const res = await this.$axios.$post('/api/watchlist', form);
    console.log(res);
  }
};
