export interface InformationDto {
  firstName: string | undefined;
  lastName: string | undefined;
  age: string;
}

export interface UserDto {
  userName: string;
  email: string;
	phoneNumber: string;
	information: InformationDto;
	avatarPath: string | undefined;
	emailConfirmed: boolean;
	personId: string | undefined;
}