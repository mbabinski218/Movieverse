import { useEffect, useState } from "react"
import { AccessToken, LocalStorage } from "../hooks/useLocalStorage";
import { useNavigate } from "react-router-dom";
import { ManagementPanel } from "../components/payments/managementPanel";
import { UserRoles } from "../UserRoles";
import { UpgradePanel } from "../components/payments/upgradePanel";
import jwtDecode from "jwt-decode";
import "./Pro.css"

export const Pro: React.FC = () => {
  const [loading, setLoading] = useState<boolean>(true);
  const [userRole, setUserRole] = useState<string[]>([]);
  const navigate = useNavigate();
  
  useEffect(() => {
    const accessToken = LocalStorage.getAccessToken();

    if (!accessToken) {
      navigate("/user");
      return;
    }

    try {
      const decodedToken = jwtDecode(accessToken) as AccessToken;
      setUserRole(decodedToken.role);
    }
    catch {
      setUserRole([]);
    }
    
    setLoading(false);
    document.title = "Pro - Movieverse";
  }, [])

  return (
    <div className="pro-page">
      {
        !loading &&
        <>
          {
            userRole.includes(UserRoles.Pro) || userRole.includes(UserRoles.Administrator) ?
            <ManagementPanel /> :
            <UpgradePanel />
          }
        </>
      }
    </div>
  )
}