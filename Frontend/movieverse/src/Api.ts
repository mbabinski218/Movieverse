import { QueryParams } from "./common/queryParams";
import { PaginatedList } from "./core/types/paginatedList";
import { SearchMediaDto } from "./core/dtos/media/searchMediaDto";
import { FilteredMediaDto } from "./core/dtos/media/filteredMediaDto";
import { WatchlistStatusDto } from "./core/dtos/user/watchlistStatusDto";
import { GetWatchlistStatusesContract } from "./core/contracts/getWatchlistStatusesContract";

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

	static async getVideoPath(id: string): Promise<string> {
		try {
			return await fetch(`${this.url}/content/${id}`)
				.then(response => response.text())
		}
		catch (error) {
			console.error(error);
			throw error;
		}
	}

	static async getWatchlistStatuses(mediaIds: string[]) 
		: Promise<WatchlistStatusDto[]> {
		try {
			const body: GetWatchlistStatusesContract = {
				mediaIds: mediaIds
			}

			return await fetch(`${this.url}/user/watchlist`, {
				mode: "cors",
				method: "POST",
				headers: {
					"Content-Type": "application/json",
					"Authorization": "Bearer eyJhbGciOiJodHRwOi8vd3d3LnczLm9yZy8yMDAxLzA0L3htbGRzaWctbW9yZSNobWFjLXNoYTUxMiIsInR5cCI6IkpXVCJ9.eyJpZCI6IjA1Yjc0ZDkxLWYzZmMtNGE2MC04N2ZkLTQzNzQzOGJlMWY4NSIsImVtYWlsIjoic3RyaW5nMkBzdHJpbmcucGwiLCJkaXNwbGF5TmFtZSI6IkJhcnRvc3oiLCJhZ2UiOiIxNSIsInJvbGUiOiJBZG1pbmlzdHJhdG9yIiwiZXhwIjoxNjk2Nzg0MjIxLCJpc3MiOiJNb3ZpZXZlcnNlIiwiYXVkIjoiTW92aWV2ZXJzZSJ9.fkVtRr0i8xtJDpg7kkqYfhDwylfgewNiWGi6P3y_2kwYPcCyH1VN6D1Dpb13H8DdkMaDWwPpW2JohSqhJxr-DQ",
				},
				body: JSON.stringify(body)
			})
			.then(response => response.json())
			.then(data => data as WatchlistStatusDto[])
		}
		catch (error) {
			console.error(error);
			throw error;
		}
	}

	static async updateWatchlistStatus(mediaId: string) 
	: Promise<void> {
	try {
		return await fetch(`${this.url}/user/watchlist/${mediaId}`, {
			headers: {
				Authorization: "Bearer eyJhbGciOiJodHRwOi8vd3d3LnczLm9yZy8yMDAxLzA0L3htbGRzaWctbW9yZSNobWFjLXNoYTUxMiIsInR5cCI6IkpXVCJ9.eyJpZCI6IjA1Yjc0ZDkxLWYzZmMtNGE2MC04N2ZkLTQzNzQzOGJlMWY4NSIsImVtYWlsIjoic3RyaW5nMkBzdHJpbmcucGwiLCJkaXNwbGF5TmFtZSI6IkJhcnRvc3oiLCJhZ2UiOiIxNSIsInJvbGUiOiJBZG1pbmlzdHJhdG9yIiwiZXhwIjoxNjk2Nzc5MDk0LCJpc3MiOiJNb3ZpZXZlcnNlIiwiYXVkIjoiTW92aWV2ZXJzZSJ9.nlThbNESyaBEGzEAeDNCg80TS3yzwbBEDMO2DaP71jquxbn7yisxx05qwOEuBTlfTYbwPCQ9cZWDuRGtDv9uTA",
			}
		})
		.then(response => response.json())
	}
	catch (error) {
		console.error(error);
		throw error;
	}
}
}