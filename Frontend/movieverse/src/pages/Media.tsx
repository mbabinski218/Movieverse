import { useNavigate, useParams } from "react-router-dom";
import { useMedia } from "../hooks/useMedia";
import { useUserMediaInfo } from "../hooks/useUserMediaInfo";
import { LinkButton } from "../components/basic/LinkButton";
import "./Media.css";
import { useCallback, useEffect } from "react";
import { NotFound } from "../components/basic/NotFound";

export const Media: React.FC = () => {
  const params = useParams();
  const [media, setMedia] = useMedia(params.id ?? "");
  const [userMediaInfo, setUserMediaInfo] = useUserMediaInfo(params.id ?? "");
  const navigate = useNavigate();

  useEffect(() => {
    if (media) {
      document.title = `${media.title} - Movieverse`;
    }
    else {
      document.title = "Not found - Movieverse";
    }
  }, [media]);

  return (
    <>      
      {
        !media ? <NotFound /> : 
        <div>
          
        </div>   
      } 
    </>
  );
}