import { MediaHorizontalList } from "../components/horizontalList/MediaHorizontalList";
import { useLatestMedia } from "../hooks/useLatestMedia";
import { useEffect } from "react";
import "./Home.css";

export const Home: React.FC = () => {
	const [filteredMedia, setFilteredMedia] = useLatestMedia(null, 1, 10);

	useEffect(() => {
		document.title = "Movieverse";
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