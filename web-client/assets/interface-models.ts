export interface Recommendation {
  id: number,
  title: string,
  deleted: boolean,
  type: string,
  genres: Array<string>,
  poster: string,
  plot: string,
}

export interface UserProfile {
  userName: string,
  avatarUrl: string | null,
  Id: string,
}

export interface Profile {
  userName: string,
  avatarUrl: string,
  id: string,
}

export interface WatchGroupInvite {
  subjectId: string,
  groupId: string,
}

export interface WatchGroupModel {
  id: string,
  addedNames: Array<string>,
  description: string,
  title: string,

  owner: UserProfile,
  members: Array<UserProfile>,
}

export interface UpdateWatchGroupModel {
  id: string,
  addedNames: Array<string>,
  title: string,
  description: string,
}

export interface WatchGroupRecommendationForm {
  genres: Array<string>,
  index: number,
  seed: string,
  type: string,
}

export interface InboxMessage {
  messageId: number,
  messageType: number,
  appendix: object,
  title: string,
  description: string,
  sender: Profile,
  dateSend: string,
}
