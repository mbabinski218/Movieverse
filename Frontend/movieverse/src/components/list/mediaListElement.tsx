import { SyntheticEvent, useCallback, useEffect, useState } from "react";
import { CloudStore } from "../../CloudStore";
import { ListItem } from "./List"
import { Link } from "react-router-dom";
import "./MediaListElement.css"
import Blank from "../../assets/blank.png";

export const MediaListElement: React.FC<ListItem> = ({ id, label, stats, description, image }) => {
  const [imgSrc, setImgSrc] = useState<string>(Blank);

  useEffect(() => {
		image ? setImgSrc(CloudStore.getImageUrl(image)) : setImgSrc(Blank);				
	}, []);

  const onError = useCallback((e: SyntheticEvent<HTMLImageElement, Event>): void => {
		const img = e.target as HTMLImageElement;
		img.src = Blank;
	}, []);

  return (
    <Link className="mle-element" to={`/media/${id}`}>
      <img className="mle-image" src={imgSrc} onError={onError} alt={label} />
      <span className="mle-title">{label}</span>
      <span className="mle-year">{stats}</span>
      <span className="mle-description">{description}</span>
    </Link>
  )
}