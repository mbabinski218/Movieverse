import { useEffect, useState } from "react";
import { AccessToken, LocalStorage } from "./useLocalStorage";
import jwtDecode from "jwt-decode";

export const useUserRoles = () => {
  const [userRole, setUserRole] = useState<string[] | null>(null);

  useEffect(() => {
    try {
      const accessToken = LocalStorage.getAccessToken();

      if (!accessToken) {
        return;
      }
  
      const decodedToken = jwtDecode(accessToken) as AccessToken;
      setUserRole(decodedToken.role);      
		}
    catch {
      return;
    }
  }, []);

  return [userRole, setUserRole] as [typeof userRole, typeof setUserRole];
}