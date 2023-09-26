import { SearchMediaDto } from "./core/dtos/media/searchMediaDto";
import { PaginatedList } from "./core/types/paginatedList";

export class Api {
	static readonly url: string = "https://localhost:44375/api";
	
	static async searchMedia(searchTerm: string, pageNumber: number | null = null, pageSize: number | null = null): Promise<PaginatedList<SearchMediaDto>> {
		try {
			let queryString = `term=${searchTerm}`;
	
			if (pageNumber !== null && pageSize !== null) {
				queryString += `&pageNumber=${pageNumber}&pageSize=${pageSize}`;
			}
			
			return await fetch(`${this.url}/media/search?${queryString}`)
				.then(response => response.json())
				.then(data => data as PaginatedList<SearchMediaDto>)
		}
		catch (error) {
			console.error(error);
			throw error;
		}
	}
}