import { ActionTree, MutationTree, GetterTree } from 'vuex'
import { RootState } from "~/store";
import {User} from "oidc-client";
import {recommendation} from "~/store/recommendation";

const ROLES = {
  MODERATOR: "Mod",
}


interface StateLayout {
  user: User | null,
  profile: any | null,
  authenticated: boolean,
}
const initState: StateLayout = {
  user: null,
  profile: null,
  authenticated: false,
}

const state = () => (
  initState
)

const authenticateOidcSilent = (context, payload = {}) => {
  const options = payload.options || {};
  return new Promise((resolve, reject) => {
    this.$auth.signinSilent(options)
      .then(user => {
        context.dispatch('auth/oidcWasAuthenticated', user);
        resolve(user);
      })
      .catch(err => {
        reject(err);
      })
  })
}


export type auth = ReturnType<typeof state>

export const getters: GetterTree<auth, RootState> = {

}

export const mutations: MutationTree<auth> = {
  SET_USER: (state, user:User) => {
    this.$axios.setToken(`Bearer ${user.access_token}`);
    state.user = user
  },
  SET_AUTH_CHECKED: (state) => state.authenticated = true,
  UNSET_OIDC_AUTH: (state) => {
    state.authenticated = false;
    state.profile = null;
    state.user = null;
  }
}

export const actions: ActionTree<auth, RootState> = {
  init({dispatch, commit}) {
    return this.$auth.querySessionStatus()
      .then(sessionStatus => {
        if (sessionStatus) {
          console.log(sessionStatus);
          return this.$auth.getUser();
        }
      })
      .then(async (user) => {
        if (user) {
          console.log(user);
          commit('SET_USER', user);
          this.$axios.setToken(`Bearer ${user.access_token}`);
        }
      }).catch(err => {
        console.log(err.message);
        if (err.message === "login_required") {
          return this.$auth.removeUser();
        }
      });
  },
  oidcWasAuthenticated(context, user) {
    context.commit('SET_USER', user);
    this.$auth.events.addAccessTokenExpired(() => {
      authenticateOidcSilent(context)
        .catch((err) => {
          console.log(err);
        });
    });
    context.commit('SET_AUTH_CHECKED');
  },
  async setBearer({commit}, access_token){
    this.$axios.setToken(access_token);
  },
  signOutOidc({commit}){
    return this.$auth.signoutRedirect().then(() => {
      commit('UNSET_OIDC_AUTH');
    });
  },
}
