﻿export interface UserProfile {
  userName: string,
  avatarUrl: string | null,
  Id: string,
}

export interface WatchGroupModel {
  id: string,
  addedNames: Array<string>,
  description: string,
  title: string,

  owner: UserProfile,
  members: Array<UserProfile>,
}
