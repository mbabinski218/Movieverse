import { useEffect, useState } from "react";
import { usePersonMedia } from "../../hooks/usePersonMedia";
import { Loading } from "../basic/Loading";
import { CloudStore } from "../../CloudStore";
import "./PersonMedia.css";
import Blank from "../../assets/blank.png";
import Star from "../../assets/star.svg";

export interface PersonMediaProps {
  personId: string;
  className?: string;
}

const PersonMedia: React.FC<PersonMediaProps> = ({personId, className}) => {
  const [personMedia] = usePersonMedia(personId);
  const [loading, setLoading] = useState<boolean>(true);

  useEffect(() => {
    if (personMedia) {
      setLoading(false);
    }
  }, [personMedia]);

  return (
    <>
      {
         loading ? <Loading /> :
          <div className={`person-media-page ${className ? className : ""}`}>
            {
              (personMedia && personMedia.length > 0) ?
              personMedia.map((media, index) => (
                <a className="person-media-item"
                  key={index}
                  href={media.id ? `/media/${media.id}` : ""}
                >
                  <img className={media.posterId ? "person-media-img" : "person-media-no-img"}
                       src={media.posterId ? CloudStore.getImageUrl(media.posterId) : Blank}
                       alt={media.title}
                  />
                  <span className="person-media-name">{`${media.title}`}</span>
                  <div className="person-media-rating">
                    <span>{media.year}</span>
                    <div className="person-media-star">
                      <img src={Star}
                          alt={"rating"}
                      />
                      <span>{media.rating}</span>
                    </div>
                  </div>
                </a>
              )) :
              <span className="person-media-name">No data</span>
            }
          </div>
      }
    </>
  );
}

export default PersonMedia;