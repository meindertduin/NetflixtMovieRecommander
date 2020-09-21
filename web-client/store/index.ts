
import { ActionTree, MutationTree } from 'vuex'

const initState = () => ({

});

export const state = initState;

export type RootState = ReturnType<typeof state>

export const mutations: MutationTree<RootState> = {

}

export const actions: ActionTree<RootState, RootState> = {
  async nuxtServerInit ({ commit, dispatch}) {

  },
  async clientInit({dispatch}, context){
    await this.$auth.getUser()
      .then(user => {
        if(user){
          console.log("user from storage");
          this.$axios.setToken(`Bearer ${user.access_token}`);
        }
      })
      .catch(err => {
        console.log("login required");
      })
  }
}
