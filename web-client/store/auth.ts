import { ActionTree, MutationTree, GetterTree } from 'vuex'
import { RootState } from "~/store";
import {User} from "oidc-client";
import {recommendation} from "~/store/recommendation";

const ROLES = {
  MODERATOR: "Mod",
}

interface StateLayout {
  user: User | null,
  profile: any | null
}
const initState: StateLayout = {
  user: null,
  profile: null,
}

const state = () => (
  initState
)

export type auth = ReturnType<typeof state>

export const getters: GetterTree<auth, RootState> = {

}

export const mutations: MutationTree<auth> = {
  SET_USER: (state, user:User) => state.user = user,
  SET_PROFILE: (state, profile) => state.profile = profile,
}

export const actions: ActionTree<auth, RootState> = {

}
