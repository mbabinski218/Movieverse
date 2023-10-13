export type LoginContract = {
  grantType: string;
  email?: string | undefined;
  password?: string | undefined;
  refreshToken?: string | undefined;
  idToken?: string | undefined;
}