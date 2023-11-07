import { useEffect, useState } from "react";
import { Api } from "../Api";

export interface AccountProps {
  email: string;
  username: string;
  age: string;
  firstName: string;
  lastName: string;
  avatar: File | null;
}

export const emptyAccountProps: AccountProps = {
  email: "",
  username: "",
  age: "",
  firstName: "",
  lastName: "",
  avatar: null
};

export const useUserData = () => {
  const [accountProps, setAccountProps] = useState<AccountProps>(emptyAccountProps);

  useEffect(() => {
    try {
      Api.getUserData()
        .then(res => {
          if (res.ok) {
            return res.json();
          }
          return;
        })
        .then(data => {
          const accountProps: AccountProps = {
            email: data.email,
            username: data.userName,
            age: data.information.age,
            firstName: data.information.firstName ?? "",
            lastName: data.information.lastName ?? "",
            avatar: new File([], "")
          };        
          setAccountProps(accountProps);
        })
		}
    catch {
      return;
    }
  }, []);

  return [accountProps, setAccountProps] as [typeof accountProps, typeof setAccountProps];
}