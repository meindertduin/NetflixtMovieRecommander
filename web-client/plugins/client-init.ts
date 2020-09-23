
import {UserManager, WebStorageStateStore} from "oidc-client";

const userManger: UserManager = new UserManager({
  authority: "https://localhost:5001",
  client_id: "web-client",
  redirect_uri: "https://localhost:3000/oidc/sign-in-callback.html",
  response_type: "code",
  scope: "openid profile IdentityServerApi Role",
  post_logout_redirect_uri: "https://localhost:3000",
  userStore: new WebStorageStateStore({store: window.localStorage}),
  //silent_redirect_uri: "http://localhost:3000/auth/silent-sign-in-callback",
});

declare module 'vuex/types/index' {
  interface Store<S> {
    $auth: UserManager
  }
}

declare module 'vue/types/vue' {
  interface Vue {
    $auth: UserManager
  }
}

const plugin: ({app, store}: { app: any; store: any }, inject:any) => void = async ({app, store}, inject) => {
  inject('auth', userManger)

  app.fetch = async () => {
    return store.dispatch('clientInit');
  }

  // await store.dispatch('clientInit');

}

export default plugin;


