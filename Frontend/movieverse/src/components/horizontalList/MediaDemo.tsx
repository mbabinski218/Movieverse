import { CloudStore } from "../../CloudStore";
import { MediaDemoDto } from "../../core/dtos/media/mediaDemoDto";
import { SyntheticEvent, useCallback, useEffect, useState } from "react";
import { LocalStorage } from "../../hooks/useLocalStorage";
import { Api } from "../../Api";
import { Link } from "react-router-dom";
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
	const [trailerUrl, setTrailerUrl] = useState<string>("");
	const [isOnUserWatchlist, setIsOnUserWatchlist] = useState<boolean | null>(isOnWatchlist);
	const link = `/media/${mediaDemo.id}`;

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

	const setTrailerHandler = useCallback((e: React.MouseEvent<HTMLElement>) => {
		if (mediaDemo.trailerId) {
				Api.getVideoPath(mediaDemo.trailerId as string)
					.then(url => setTrailerUrl(url))
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
				<Link to={link}>
					<img className="img" src={imgSrc} onError={onError} alt={mediaDemo.title} />
				</Link>
				<div className="overlay">
					<Link className="item-title" to={link}>{mediaDemo.title}</Link>
					<div className={isWatchlistLoaded ? "add-to-watchlist enable-icon" : "add-to-watchlist disable-icon"} onClick={updateWatchlist}>
						<img className="icon" src={isOnUserWatchlist ? Check : Plus} alt="like" />
					</div>
					<Link className={trailerAvailable ? "trailer enable-icon" : "trailer disable-icon"} onMouseDown={setTrailerHandler} to={trailerUrl}>
						<img className="icon" src={Clapperboard} alt="trailer" />
					</Link>
					<div className="icon rating">
						<img className="star" src={Star} alt="star" />
						<span className="rating-value">{mediaDemo.rating}</span>
					</div>
				</div>
			</div>
	);
}