import { CloudStore } from "../../CloudStore";
import { SearchMediaDto } from "../../core/dtos/media/searchMediaDto";
import { PaginatedList } from "../../core/types/paginatedList";
import Blank from "../../assets/blank.png";
import "./SearchList.css";


interface SearchListProps {
	searchResult: PaginatedList<SearchMediaDto>;
}

export const SearchList: React.FC<SearchListProps> = ({searchResult}) => {
	const onError = (e: any) => {
		e.target.src = Blank;
	} 

	return (
		<div>
			{
				searchResult.items.map((result) => {
					return (
						<a className="items" key={result.id} href={`/media/${result.id}`}>
							<img className="poster" 
									 src={CloudStore.getImageUrl(result.poster ? Blank : result.poster as string) } 
									 onError={onError}/>
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