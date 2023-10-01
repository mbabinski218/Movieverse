import { QueryParams } from "./common/queryParams";
import { PaginatedList } from "./core/types/paginatedList";
import { SearchMediaDto } from "./core/dtos/media/searchMediaDto";
import { FilteredMediaDto } from "./core/dtos/media/filteredMediaDto";

export class Api {
	static readonly url: string = "https://localhost:44375/api";
	
	static async searchMedia(searchTerm: string, pageNumber: number | null = null, pageSize: number | null = null)
		: Promise<PaginatedList<SearchMediaDto>> {
		try {
			const queryParams = new QueryParams();
			queryParams.add("term", searchTerm);
	
			if (pageNumber !== null && pageSize !== null) {
				queryParams.add("pageNumber", pageNumber.toString());
				queryParams.add("pageSize", pageSize.toString());
			}
			
			return await fetch(`${this.url}/media/search?${queryParams.toString()}`)
				.then(response => response.json())
				.then(data => data as PaginatedList<SearchMediaDto>)
		}
		catch (error) {
			console.error(error);
			throw error;
		}
	}

	static async getLatestMedia(platformId: string | null = null, pageNumber: number | null = null, pageSize: number | null = null)
		: Promise<FilteredMediaDto[]> {
		try {
			const queryParams = new QueryParams();

			if (platformId !== null) {
				queryParams.add("platformId", platformId);
			}

			if (pageNumber !== null && pageSize !== null) {
				queryParams.add("pageNumber", pageNumber.toString());
				queryParams.add("pageSize", pageSize.toString());
			}
			
			return await fetch(`${this.url}/media/latest?${queryParams.toString()}`)
				.then(response => response.json())
				.then(data => data as FilteredMediaDto[])
		}
		catch (error) {
			console.error(error);
			throw error;
		}
	}
}