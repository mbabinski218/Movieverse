import { useEffect, useState } from "react";

export class LocalStorage {
  static readonly accessTokenKey: string = "accessToken";
  static readonly refreshTokenKey: string = "refreshToken";

  static getAccessToken(): string | null {
    const token = localStorage.getItem(this.accessTokenKey);
    return token ? JSON.parse(token) : null;
  }

  static getRefreshToken(): string | null {
    const token = localStorage.getItem(this.refreshTokenKey);
    return token ? JSON.parse(token) : null;
  }

  static getBearerToken(): string {
    return `Bearer ${this.getAccessToken()}`;
  }
}

export interface AccessToken {
  id: string;
  email: string;
  displayName: string;
  age: string;
  role: string[];
  exp: number;
  iss: string;
  aud: string;
}

export const useLocalStorage = <T>(key: string, initialValue: T | (() => T)) => {
  const [storedValue, setStoredValue] = useState<T>(() => {
		try {
			const item = localStorage.getItem(key);

			if(item !== null) {
				return JSON.parse(item);
			}

			return typeof initialValue === "function" ? initialValue as () => T : initialValue;
		} 
    catch (err) {
      console.log(err);
      return initialValue;
    }
  });

  useEffect(() => {
    localStorage.setItem(key, JSON.stringify(storedValue));
  });

  return [storedValue, setStoredValue] as [typeof storedValue, typeof setStoredValue];
}