import { useEffect } from "react"
import { useNavigate } from "react-router-dom";
import { LocalStorage } from "../hooks/useLocalStorage";

export const Account: React.FC = () => {
  const navigate = useNavigate();

  useEffect(() => {
    if (!LocalStorage.getAccessToken()) {
      navigate("/user", { replace: true });
    }

    document.title = "Account - Movieverse"
  });

  return <h1>Account</h1>
}