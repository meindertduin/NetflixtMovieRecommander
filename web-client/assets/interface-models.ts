export interface UserProfile {
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
