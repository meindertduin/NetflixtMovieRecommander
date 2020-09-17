import oidc from 'oidc-client'
import { objectAssign, parseJwt, firstLetterUppercase, camelCaseToSnakeCase } from './utils'


const defaultOidcConfig = {
  userStore: new oidc.WebStorageStateStore(),
  loadUserInfo: true,
  automaticSilentRenew: true,
}

const requiredConfigProperties:Array<string> = [
  'authority',
  'client_id',
  'redirect_uri',
  'response_type',
  'scope'
]

const settingsThatAreSnakeCasedInOidcClient = [
  'clientId',
  'redirectUri',
  'responseType',
  'maxAge',
  'uiLocales',
  'loginHint',
  'acrValues',
  'postLogoutRedirectUri',
  'popupRedirectUri',
  'silentRedirectUri'
]

const snakeCasedSettings = (oidcSettings) => {
  settingsThatAreSnakeCasedInOidcClient.forEach(setting => {
    if (typeof oidcSettings[setting] !== 'undefined') {
      oidcSettings[camelCaseToSnakeCase(setting)] = oidcSettings[setting]
    }
  })
  return oidcSettings
}






