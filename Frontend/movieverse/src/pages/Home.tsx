import { MediaHorizontalList } from "../components/horizontalList/MediaHorizontalList";
import { useLatestMedia } from "../hooks/useLatestMedia";
import { useEffect, useState } from "react";
import "./Home.css";
import { Loading } from "../components/basic/Loading";

export const Home: React.FC = () => {
	const [loading, setLoading] = useState<boolean>(true);
	const [filteredMedia, setFilteredMedia] = useLatestMedia(null, 1, 10);

	useEffect(() => {
		document.title = "Movieverse";
	}, []);

	useEffect(() => {
		if (filteredMedia.length > 0) {
			setLoading(false);
		}
	}, [filteredMedia]);

	return (
		<div className="page">
			{
				loading ? 
				<div className="page-loading">
					<Loading />
				</div> :
				<>
					<span className="page-title">What to watch</span>
					<div>
						{filteredMedia.map((filteredMedia) => (
							<MediaHorizontalList key={filteredMedia.platformId} filteredMedia={filteredMedia} />
						))}
					</div>
				</>
			}
		</div>
	);
}