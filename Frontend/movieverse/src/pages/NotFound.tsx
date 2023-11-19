import { useEffect } from "react";
import { NotFound as NotFoundInfo } from "../components/basic/NotFound";

export const NotFound: React.FC = () => {
  useEffect(() => {
    document.title = "Not found - Movieverse"
  }, [])

  return <NotFoundInfo />  
}