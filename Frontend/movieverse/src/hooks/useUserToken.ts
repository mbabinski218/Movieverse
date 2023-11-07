import { useEffect, useState } from "react";
import { AccessToken, LocalStorage } from "./useLocalStorage";
import jwtDecode from "jwt-decode";

export const useUserToken = () => {
  const [userToken, setUserToken] = useState<AccessToken | null>(null);

  useEffect(() => {
    try {
      const accessToken = LocalStorage.getAccessToken();

      if (!accessToken) {
        return;
      }
  
      const decodedToken = jwtDecode(accessToken) as AccessToken;
      setUserToken(decodedToken);      
		}
    catch {
      return;
    }
  }, []);

  return [userToken, setUserToken] as [typeof userToken, typeof setUserToken];
}