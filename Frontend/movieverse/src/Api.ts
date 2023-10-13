import { LocalStorage } from "./hooks/useLocalStorage";
import { QueryParams } from "./common/queryParams";
import { PaginatedList } from "./core/types/paginatedList";
import { SearchMediaDto } from "./core/dtos/media/searchMediaDto";
import { FilteredMediaDto } from "./core/dtos/media/filteredMediaDto";
import { WatchlistStatusDto } from "./core/dtos/user/watchlistStatusDto";
import { GetWatchlistStatusesContract } from "./core/contracts/getWatchlistStatusesContract";
import { RegisterContract } from "./core/contracts/registerContract";
import { LoginContract } from "./core/contracts/loginContract";
import { StatusCodes } from "./StatusCodes";
import { TokensDto } from "./core/dtos/user/tokensDto";

export class Api {
	static readonly url: string = "https://localhost:44375/api";
	static culture: string = "en-US";
	
	static async fetchWithAuthorization(endpoint: string, init?: RequestInit | undefined, queryParams?: QueryParams | undefined): Promise<Response> {
		const url: string = `${this.url}/${endpoint}${queryParams ? `?${queryParams.toString()}` : ""}`

		const response = await fetch(url, init);
			
		if (response.status !== StatusCodes.Unauthorized && response.status !== StatusCodes.Forbidden) {
			return response;
		}

		const refreshToken = LocalStorage.refreshToken;
		if (!refreshToken) {
			return response;
		}

		console.log("Refreshing token...");

		const loginContract: LoginContract = {
			grantType: "RefreshToken",
			refreshToken: refreshToken
		}

		const loginResponse = await fetch(`${this.url}/user/login`, {
			mode: "cors",
			method: "POST",
			headers: {
				"Content-Type": "application/json"
			},
			body: JSON.stringify(loginContract)
		});

		if (loginResponse.status !== StatusCodes.Ok) {
			console.log("Token refresh failed. Redirecting to login...");
			localStorage.clear();			
			window.location.href = "/user";
			return response;
		}

		const tokens: TokensDto = await loginResponse.json() as TokensDto;
		localStorage.setItem(LocalStorage.accessTokenKey, JSON.stringify(tokens.accessToken));
		localStorage.setItem(LocalStorage.refreshTokenKey, JSON.stringify(tokens.refreshToken));

		console.log("Token refreshed successfully. Retrying request...");
		return await fetch(url, init);
	}

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
		
		return await this.fetchWithAuthorization(`user/watchlist`, {
			mode: "cors",
			method: "POST",
			headers: {
				"Content-Type": "application/json",
				"Authorization": LocalStorage.bearerToken
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
		return await this.fetchWithAuthorization(`user/watchlist/${mediaId}`, {
			mode: "cors",
			method: "PUT",
			headers: {
				"Content-Type": "application/json",
				"Authorization": `Bearer ${LocalStorage.accessToken}`
			}
			})
	}

	static async register(registerContract: RegisterContract) : Promise<Response> {
		return await fetch(`${this.url}/user/register`, {
			mode: "cors",
			method: "POST",
			headers: {
				"Content-Type": "application/json"
			},
			body: JSON.stringify(registerContract)
		})
	}

	static async login(loginContract: LoginContract) : Promise<Response> {
		return await fetch(`${this.url}/user/login`, {
			mode: "cors",
			method: "POST",
			headers: {
				"Content-Type": "application/json",
				"Accept-Language": this.culture
			},
			body: JSON.stringify(loginContract)
		})
	}
}