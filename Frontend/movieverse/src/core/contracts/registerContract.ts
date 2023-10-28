export type RegisterContract = {
  email: string;
  username: string;
  age: number;
  firstName?: string | undefined;
  lastName?: string | undefined;
  password: string;
  confirmPassword: string;  
}