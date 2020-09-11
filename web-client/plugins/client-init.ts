
﻿import {UserManager, WebStorageStateStore} from "oidc-client";

import Vue from 'vue'
import Store from 'vuex';


const userManger: UserManager = new UserManager({
  authority: "http://localhost:5000",
  client_id: "web-client",
  redirect_uri: "http://localhost:3000/auth/sign-in-callback",
  response_type: "code",
  scope: "openid profile IdentityServerApi role",
  post_logout_redirect_uri: "http://localhost:3000/",
  //silent_redirect_uri: "https://localhost:3000/",
  userStore: new WebStorageStateStore({store: window.localStorage}),
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

const plugin: Plugin = (context, inject) => {
  inject('auth', userManger)
}

export default plugin;
