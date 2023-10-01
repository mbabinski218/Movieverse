import { MediaHorizontalList } from "../components/media/MediaHorizontalList";
import { FilteredMediaDto } from "../core/dtos/media/filteredMediaDto";
import { useCallback, useEffect, useState } from "react";
import { Api } from "../Api";
import "./Home.css";

export const Home: React.FC = () => {
	const [filteredMedia, setFilteredMedia] = useState<FilteredMediaDto[]>([]);

	useEffect(() => {
		Api.getLatestMedia(null, 1, 10)
			.then((media) => setFilteredMedia(media))
			.catch((err) => console.error(err));
	}, []);

	return (
		<div className="page">
			{filteredMedia.map((filteredMedia) => (
				<MediaHorizontalList key={filteredMedia.platformId} filteredMedia={filteredMedia} />
			))}
		</div>
	);
}