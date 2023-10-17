export type informationContract = {
  firstName?: string | undefined;
  lastName?: string | undefined;
  age?: number | undefined;
}

export type UpdateUserContract = {
  email?: string | undefined;
  userName?: string | undefined;
  information?: informationContract | undefined;
  avatar?: File | undefined;
}