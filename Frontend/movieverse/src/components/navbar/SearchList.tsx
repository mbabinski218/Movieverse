import { CloudStore } from "../../CloudStore";
import { SearchMediaDto } from "../../core/dtos/media/searchMediaDto";
import { PaginatedList } from "../../core/types/paginatedList";
import { SyntheticEvent, useCallback } from "react";
import "./SearchList.css";
import Blank from "../../assets/blank.png";


interface SearchListProps {
	searchResult: PaginatedList<SearchMediaDto>;
}

export const SearchList: React.FC<SearchListProps> = ({searchResult}) => {
	const onError = useCallback((e: SyntheticEvent<HTMLImageElement, Event>): void => {
		const img = e.target as HTMLImageElement;
		img.src = Blank;
	}, []);

	return (
		<div>
			{
				searchResult.items.map((result) => {
					let imgSrc = Blank;
					if (result.poster)
					{
						imgSrc = CloudStore.getImageUrl(result.poster as string);
					}

					return (
						<a className="items" 
							 key={result.id} 
							 href={`/media/${result.id}`}
						>
							<img className="poster"
									 src={imgSrc} 
									 onError={onError}
							/>
							<div className="info">
								<span className="title">{result.title}</span>
								<span className="year">{result.year ? result.year : "Unknown release date"}</span>
								<span className="description">{result.description}</span>
							</div>							
						</a>
					);
				})
			}
		</div>
	);
}