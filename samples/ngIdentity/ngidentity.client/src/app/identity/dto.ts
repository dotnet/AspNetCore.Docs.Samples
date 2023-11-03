// login and register
export interface UserDto {
  email: string;
  password: string;
}

// manage/info
export interface UserInfo {
  email: string;
  isEmailConfirmed: boolean;
}
