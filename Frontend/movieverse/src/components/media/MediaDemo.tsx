import { CloudStore } from "../../CloudStore";
import { MediaDemoDto } from "../../core/dtos/media/mediaDemoDto";
import { SyntheticEvent, useCallback, useEffect, useState } from "react";
import { Api } from "../../Api";
import "./MediaDemo.css";
import Blank from "../../assets/blank.png";
import Clapperboard from "../../assets/clapperboard.svg";
import Plus from "../../assets/plus.svg";
import Check from "../../assets/check-gold.svg";
import Star from "../../assets/star.svg";

interface MediaDemoProps {
	mediaDemo: MediaDemoDto;
}

export const MediaDemo: React.FC<MediaDemoProps> = ({mediaDemo}) => {
	const [imgSrc, setImgSrc] = useState<string>(Blank);
	const [trailerAvailable, setTrailerAvailable] = useState<boolean>(false);
	const [isOnWatchlist, setIsOnWatchlist] = useState<boolean>(false);

	const onError = useCallback((e: SyntheticEvent<HTMLImageElement, Event>): void => {
		const img = e.target as HTMLImageElement;
		img.src = Blank;
	}, []);

	useEffect(() => {
		mediaDemo.posterId ? setImgSrc(CloudStore.getImageUrl(mediaDemo.posterId as string)) : setImgSrc(Blank);				
		setTrailerAvailable(mediaDemo.trailerId ? true : false);		
	}, [mediaDemo]);

	const openTrailer = useCallback(() => {
		if (mediaDemo.trailerId) {
			Api.getVideoPath(mediaDemo.trailerId as string)
				.then(url => window.location.href = url)
				.catch(err => console.error(err));
		}
	}, []);

	return (
		<div className="item zoom">
			<img className="img" src={imgSrc} onError={onError} alt={mediaDemo.title} />
			<div className="overlay">
				<a className="item-title">{mediaDemo.title}</a>
				<div className="add-to-watchlist enable-icon">
					<img className="icon" src={Plus} alt="like" />
				</div>
				<div className={trailerAvailable ? "trailer enable-icon" : "no-trailer"} onClick={openTrailer}>
					<img className="icon" src={Clapperboard} alt="trailer" />
				</div>
				<div className="icon rating">
					<img className="star" src={Star} alt="star" />
					<span className="rating-value">{mediaDemo.rating}</span>
				</div>
			</div>
		</div>
	);
}