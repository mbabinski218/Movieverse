import { useNavigate } from "react-router-dom";
import { useCallback } from "react";
import { LinkButton } from "./LinkButton";
import "./NotFound.css"

export const NotFound: React.FC = () => {
  const navigate = useNavigate();
  
  const goBackHandler = useCallback(() => {
    navigate(-1);
  }, []);

  return (
    <div className="not-found">
      <span>404 Not found</span>
      <LinkButton label="Go back" onClick={goBackHandler} />
    </div>
  );
}