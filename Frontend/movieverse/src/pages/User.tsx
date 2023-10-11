import { Authentication } from "../components/user/Authentication";
import "./User.css";

export const User: React.FC = () => {
    return (
      <div className="user-page">
        <Authentication />
      </div>
    )
  }