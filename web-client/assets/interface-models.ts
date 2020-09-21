export interface UserProfile {
  userName: string,
  avatarUrl: string | null,
  Id: string,
}

export interface WatchGroup {
  id: string,
  addedNames: Array<string>,
  description: string,
  title: string,

  owner: UserProfile,
  members: Array<UserProfile>,
}
