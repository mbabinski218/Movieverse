import { SyntheticEvent, useCallback, useEffect, useState } from "react";
import { CloudStore } from "../../CloudStore";
import { ListItem } from "./List"
import "./personListElement.css"
import Blank from "../../assets/blank.png";

export const PersonListElement: React.FC<ListItem> = ({ id, label, stats, description, image }) => {
  const [imgSrc, setImgSrc] = useState<string>(Blank);

  useEffect(() => {
		image ? setImgSrc(CloudStore.getImageUrl(image)) : setImgSrc(Blank);				
	}, []);

  const onError = useCallback((e: SyntheticEvent<HTMLImageElement, Event>): void => {
		const img = e.target as HTMLImageElement;
		img.src = Blank;
	}, []);

  return (
    <div className="ple-element">
      <img className="ple-image" src={imgSrc} onError={onError} alt={label} />
      <span className="ple-title">{label}</span>
      <span className="ple-age">{`Age: ${stats}`}</span>
      <span className="ple-description">{description}</span>
    </div>
  )
}