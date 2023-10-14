import { CloudStore } from "../../CloudStore";
import { MediaDemoDto } from "../../core/dtos/media/mediaDemoDto";
import { SyntheticEvent, useCallback, useEffect, useState } from "react";
import { LocalStorage } from "../../hooks/useLocalStorage";
import { Api } from "../../Api";
import "./MediaDemo.css";
import Blank from "../../assets/blank.png";
import Clapperboard from "../../assets/clapperboard.svg";
import Plus from "../../assets/plus.svg";
import Check from "../../assets/check-gold.svg";
import Star from "../../assets/star.svg";

interface MediaDemoProps {
	mediaDemo: MediaDemoDto;
	isOnWatchlist: boolean | null;
	isWatchlistLoaded: boolean;
}

export const MediaDemo: React.FC<MediaDemoProps> = ({mediaDemo, isOnWatchlist, isWatchlistLoaded}) => {
	const [imgSrc, setImgSrc] = useState<string>(Blank);
	const [trailerAvailable, setTrailerAvailable] = useState<boolean>(false);
	const [isOnUserWatchlist, setIsOnUserWatchlist] = useState<boolean | null>(isOnWatchlist);

	const onError = useCallback((e: SyntheticEvent<HTMLImageElement, Event>): void => {
		const img = e.target as HTMLImageElement;
		img.src = Blank;
	}, []);

	useEffect(() => {
		setIsOnUserWatchlist(isOnWatchlist);
	}, [isOnWatchlist]);

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

	const updateWatchlist = useCallback(() => {
		if (!isWatchlistLoaded || !LocalStorage.getAccessToken()) {
			return;
		}

		Api.updateWatchlistStatus(mediaDemo.id as string)
			.then((res) => {
				if (res.ok) {
					setIsOnUserWatchlist(!isOnUserWatchlist);
				}				
			})
			.catch(err => console.error(err));
	}, [isWatchlistLoaded, isOnUserWatchlist]);

	return (
		<div className="item zoom">
			<img className="img" src={imgSrc} onError={onError} alt={mediaDemo.title} />
			<div className="overlay">
				<a className="item-title">{mediaDemo.title}</a>
				<div className={isWatchlistLoaded ? "add-to-watchlist enable-icon" : "add-to-watchlist disable-icon"} onClick={updateWatchlist}>
					<img className="icon" src={isOnUserWatchlist ? Check : Plus} alt="like" />
				</div>
				<div className={trailerAvailable ? "trailer enable-icon" : "trailer disable-icon"} onClick={openTrailer}>
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