import { useEffect } from "react";
import { Authentication } from "../components/user/Authentication";
import "./User.css";

export const User: React.FC = () => {
  useEffect(() => {
    document.title = "Sign in - Movieverse"
  })

  return (
    <div className="user-page">
      <Authentication />
    </div>
  )
}