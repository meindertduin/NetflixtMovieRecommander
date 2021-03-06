﻿export interface Recommendation {
  id: number,
  title: string,
  deleted: boolean,
  type: string,
  genres: string,
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
  owner: Profile,
  members: Array<Profile>,
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
  type: string,
  alreadyLoaded: Array<number>,
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

export interface WatchGroupInviteResponseMessage {
  messageId: number
  accepted: boolean,
  inviterId: string,
}
