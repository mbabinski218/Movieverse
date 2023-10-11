export type LoginContract = {
  grantType: string;
  email: string | null;
  password: string | null;
  refreshToken: string | null;
  idToken: string | null;
}