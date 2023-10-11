import { QueryParams } from "./common/queryParams";
import { PaginatedList } from "./core/types/paginatedList";
import { SearchMediaDto } from "./core/dtos/media/searchMediaDto";
import { FilteredMediaDto } from "./core/dtos/media/filteredMediaDto";
import { WatchlistStatusDto } from "./core/dtos/user/watchlistStatusDto";
import { GetWatchlistStatusesContract } from "./core/contracts/getWatchlistStatusesContract";
import { RegisterContract } from "./core/contracts/registerContract";
import { LoginContract } from "./core/contracts/loginContract";
import { TokensDto } from "./core/dtos/user/tokensDto";

export class Api {
	static readonly url: string = "https://localhost:44375/api";
	
	static async searchMedia(searchTerm: string, pageNumber: number | null = null, pageSize: number | null = null)
		: Promise<PaginatedList<SearchMediaDto>> {
		const queryParams = new QueryParams();
		queryParams.add("term", searchTerm);

		if (pageNumber !== null && pageSize !== null) {
			queryParams.add("pageNumber", pageNumber.toString());
			queryParams.add("pageSize", pageSize.toString());
		}
		
		return await fetch(`${this.url}/media/search?${queryParams.toString()}`)
			.then(response => response.json())
			.then(data => data as PaginatedList<SearchMediaDto>)
			.catch(error => {
				console.error(error);
				throw error;
			})
	}

	static async getLatestMedia(platformId: string | null = null, pageNumber: number | null = null, pageSize: number | null = null)
		: Promise<FilteredMediaDto[]> {
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
			.catch(error => {
				console.error(error);
				throw error;
			})
	}

	static async getVideoPath(id: string): Promise<string> {
		return await fetch(`${this.url}/content/${id}`)
			.then(response => response.text())
			.catch(error => {
				console.error(error);
				throw error;
			})
	}

	static async getWatchlistStatuses(mediaIds: string[]) : Promise<WatchlistStatusDto[]> {
		const body: GetWatchlistStatusesContract = {
			mediaIds: mediaIds
		}

		return await fetch(`${this.url}/user/watchlist`, {
			mode: "cors",
			method: "POST",
			headers: {
				"Content-Type": "application/json",
				"Authorization": "Bearer eyJhbGciOiJodHRwOi8vd3d3LnczLm9yZy8yMDAxLzA0L3htbGRzaWctbW9yZSNobWFjLXNoYTUxMiIsInR5cCI6IkpXVCJ9.eyJpZCI6IjA1Yjc0ZDkxLWYzZmMtNGE2MC04N2ZkLTQzNzQzOGJlMWY4NSIsImVtYWlsIjoic3RyaW5nMkBzdHJpbmcucGwiLCJkaXNwbGF5TmFtZSI6IkJhcnRvc3oiLCJhZ2UiOiIxNSIsInJvbGUiOiJBZG1pbmlzdHJhdG9yIiwiZXhwIjoxNjk2ODczMzc3LCJpc3MiOiJNb3ZpZXZlcnNlIiwiYXVkIjoiTW92aWV2ZXJzZSJ9._YkUjK8PCiAKvO-bXay7CVLQ8y7zp36q4eIIsqZfbVW5KQo1wxNZc46CGT-NzIceXmNHtidNSzEPln4xkYA5AA",
			},
			body: JSON.stringify(body)
			})
			.then(response => response.json())
			.then(data => data as WatchlistStatusDto[])
			.catch(error => {
				console.error(error);
				throw error;
			})
	}

	static async updateWatchlistStatus(mediaId: string) : Promise<Response> {
		return await fetch(`${this.url}/user/watchlist/${mediaId}`, {
			mode: "cors",
			method: "PUT",
			headers: {
				"Authorization": "Bearer eyJhbGciOiJodHRwOi8vd3d3LnczLm9yZy8yMDAxLzA0L3htbGRzaWctbW9yZSNobWFjLXNoYTUxMiIsInR5cCI6IkpXVCJ9.eyJpZCI6IjA1Yjc0ZDkxLWYzZmMtNGE2MC04N2ZkLTQzNzQzOGJlMWY4NSIsImVtYWlsIjoic3RyaW5nMkBzdHJpbmcucGwiLCJkaXNwbGF5TmFtZSI6IkJhcnRvc3oiLCJhZ2UiOiIxNSIsInJvbGUiOiJBZG1pbmlzdHJhdG9yIiwiZXhwIjoxNjk2ODczMzc3LCJpc3MiOiJNb3ZpZXZlcnNlIiwiYXVkIjoiTW92aWV2ZXJzZSJ9._YkUjK8PCiAKvO-bXay7CVLQ8y7zp36q4eIIsqZfbVW5KQo1wxNZc46CGT-NzIceXmNHtidNSzEPln4xkYA5AA",
			}
			})
			.catch(error => {
				console.error(error);
				throw error;
			})
	}

	static async getGoogleClientId() : Promise<string> {
		return await fetch(`${this.url}/environment/google-client-id`, {
			mode: "no-cors",
			method: "GET"
			})
			.then(response => response.text())
			.catch(error => {
				console.error(error);
				throw error;
			})
	}

		static async register(registerContract: RegisterContract) : Promise<boolean> {
			return await fetch(`${this.url}/user/register`, {
				mode: "no-cors",
				method: "POST",
				body: JSON.stringify(registerContract)
				})
				.then(response => response.ok)
				.catch(error => {
					console.error(error);
					throw error;
				})
		}

	static async login(loginContract: LoginContract) : Promise<TokensDto> {
		return await fetch(`${this.url}/user/login`, {
			mode: "no-cors",
			method: "POST",
			body: JSON.stringify(loginContract)
			})
			.then(response => response.json())
			.catch(error => {
				console.error(error);
				throw error;
			})
	}
}