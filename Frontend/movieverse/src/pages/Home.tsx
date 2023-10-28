import { MediaHorizontalList } from "../components/horizontalList/MediaHorizontalList";
import { FilteredMediaDto } from "../core/dtos/media/filteredMediaDto";
import { useEffect, useState } from "react";
import { Api } from "../Api";
import "./Home.css";

export const Home: React.FC = () => {
	const [filteredMedia, setFilteredMedia] = useState<FilteredMediaDto[]>([]);

	useEffect(() => {
		document.title = "Movieverse"
		
		Api.getLatestMedia(null, 1, 10)
			.then((media) => setFilteredMedia(media))
			.catch((err) => console.error(err));
	}, []);

	return (
		<div className="page">
			<span className="page-title">What to watch</span>
			<div>
				{filteredMedia.map((filteredMedia) => (
					<MediaHorizontalList key={filteredMedia.platformId} filteredMedia={filteredMedia} />
				))}
			</div>
		</div>
	);
}