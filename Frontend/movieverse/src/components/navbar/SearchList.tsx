import { CloudStore } from "../../CloudStore";
import { SearchMediaDto } from "../../core/dtos/media/searchMediaDto";
import { PaginatedList } from "../../core/types/paginatedList";

interface SearchListProps {
	searchResult: PaginatedList<SearchMediaDto>;
}

export const SearchList: React.FC<SearchListProps> = ({searchResult}) => {  
	return (
		<div>
			{
				searchResult.items.map((result) => {
					return (
						<div key={result.id}>
							<span>{result.id}</span>
							<span>{result.title}</span>
							<span>{result.year}</span>
							<img src={CloudStore.getImageUrl(result.poster ? "none" : result.poster as string)} />
						</div>
					);
				})
			}
		</div>
	);
}