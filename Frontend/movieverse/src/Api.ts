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
import { UpdateUserContract } from "./core/contracts/updateUserContract";
import { FormDataHelper } from "./common/formDataHelper";
import { SearchPersonDto } from "./core/dtos/person/searchPersonDto";
import { GenreDto } from "./core/dtos/genre/genreDto";
import { AddMediaContract } from "./core/contracts/addMediaContract";
import { UpdateRolesContract } from "./core/contracts/updateRolesContract";

export class Api {
	static readonly url: string = "https://localhost:44375/api";
	static culture: string = "en-US";
	
	static async fetchWithAuthorization(endpoint: string, init?: RequestInit | undefined, queryParams?: QueryParams | undefined): Promise<Response> {
		const url: string = `${this.url}/${endpoint}${queryParams ? `?${queryParams.toString()}` : ""}`

		const response = await fetch(url, init);
			
		if (response.status !== StatusCodes.Unauthorized && response.status !== StatusCodes.Forbidden) {
			return response;
		}

		const refreshToken = LocalStorage.getRefreshToken();
		if (!refreshToken) {
			console.log("No refresh token found.");
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
		const keys: HeadersInit | undefined = init?.headers;

		if (!keys) {
			console.log("No headers found.");
			return response;
		}

		const headers: Headers = new Headers(keys);
		headers.set("Authorization", LocalStorage.getBearerToken());
		init!.headers = headers;
		
		return await fetch(url, init);
	}

	static async getGenres(): Promise<GenreDto[]> {
		return await fetch(`${this.url}/genre`)
		.then(response => response.json())
		.then(data => data as GenreDto[])
		.catch(error => {
			console.error(error);
			throw error;
		})	
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

	static async searchWithFiltersMedia(term: string | null, type: string, genreId: string | null, pageNumber: number | null = null, pageSize: number | null = null)
	: Promise<PaginatedList<SearchMediaDto>> {
	const queryParams = new QueryParams();

	if (term !== null) {
		queryParams.add("term", term);
	}

	queryParams.add("type", type);

	if (genreId !== null) {
		queryParams.add("genreId", genreId);
	}

	if (pageNumber !== null && pageSize !== null) {
		queryParams.add("pageNumber", pageNumber.toString());
		queryParams.add("pageSize", pageSize.toString());
	}
	
	return await fetch(`${this.url}/media/searchWithFilters?${queryParams.toString()}`)
		.then(response => response.json())
		.then(data => data as PaginatedList<SearchMediaDto>)
		.catch(error => {
			console.error(error);
			throw error;
		})			
	}

	static async searchPersons(searchTerm: string | null, pageNumber: number | null = null, pageSize: number | null = null)
	: Promise<PaginatedList<SearchPersonDto>> {
	const queryParams = new QueryParams();
	if (searchTerm !== null) {
		queryParams.add("term", searchTerm);
	}

	if (pageNumber !== null && pageSize !== null) {
		queryParams.add("pageNumber", pageNumber.toString());
		queryParams.add("pageSize", pageSize.toString());
	}
	
	return await fetch(`${this.url}/person/search?${queryParams.toString()}`)
		.then(response => response.json())
		.then(data => data as PaginatedList<SearchPersonDto>)
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

	static async getWatchlistStatuses(mediaIds: string[]): Promise<WatchlistStatusDto[]> {
		const body: GetWatchlistStatusesContract = {
			mediaIds: mediaIds
		}

		return await this.fetchWithAuthorization(`user/watchlist`, {
			mode: "cors",
			method: "POST",
			headers: {
				"Content-Type": "application/json",
				"Accept-Language": this.culture,
				"Authorization": LocalStorage.getBearerToken()
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

	static async updateWatchlistStatus(mediaId: string): Promise<Response> {
		return await this.fetchWithAuthorization(`user/watchlist/${mediaId}`, {
			mode: "cors",
			method: "PUT",
			headers: {
				"Content-Type": "application/json",
				"Accept-Language": this.culture,
				"Authorization": LocalStorage.getBearerToken()
			}
		})
	}

	static async register(registerContract: RegisterContract): Promise<Response> {
		return await fetch(`${this.url}/user/register`, {
			mode: "cors",
			method: "POST",
			headers: {
				"Content-Type": "application/json",
				"Accept-Language": this.culture
			},
			body: JSON.stringify(registerContract)
		})
	}

	static async login(loginContract: LoginContract): Promise<Response> {
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

	static async logout() : Promise<Response> {
		return await fetch(`${this.url}/user/logout`, {
			mode: "cors",
			method: "POST",
			headers: {
				"Content-Type": "application/json",
				"Accept-Language": this.culture,
				"Authorization": LocalStorage.getBearerToken()
			}
		})
	}

	static async getUserData() : Promise<Response> {
		console.log(LocalStorage.getBearerToken());
		return await this.fetchWithAuthorization(`user`, {
			mode: "cors",
			method: "GET",
			headers: {
				"Content-Type": "application/json",
				"Accept-Language": this.culture,
				"Authorization": LocalStorage.getBearerToken()
			}
		})
	}

	static async updateUserData(data: UpdateUserContract) : Promise<Response> {
		const form = FormDataHelper.toFormData(data);

		return await this.fetchWithAuthorization(`user`, {
			mode: "cors",
			method: "PUT",
			headers: {
				"Accept-Language": this.culture,
				"Authorization": LocalStorage.getBearerToken()
			},
			body: form
		})
	}

	static async getWatchlist(pageNumber: number | null = null, pageSize: number | null = null) : Promise<Response> {
		const queryParams = new QueryParams();
		
		if (pageNumber !== null && pageSize !== null) {
			queryParams.add("pageNumber", pageNumber.toString());
			queryParams.add("pageSize", pageSize.toString());
		}
		
		return await this.fetchWithAuthorization(`user/watchlist`, {
			mode: "cors",
			method: "GET",
			headers: {
				"Content-Type": "application/json",
				"Accept-Language": this.culture,
				"Authorization": LocalStorage.getBearerToken()
			}
		}, queryParams);
	}

	static async getMediaChart(type: string | null = null, category: string | null = null, pageNumber: number | null = null, pageSize: number | null = null) : Promise<Response> {
		const queryParams = new QueryParams();
		
		if (type !== null) {
			queryParams.add("type", type);
		}

		if (category !== null) {
			queryParams.add("category", category);
		}

		if (pageNumber !== null && pageSize !== null) {
			queryParams.add("pageNumber", pageNumber.toString());
			queryParams.add("pageSize", pageSize.toString());
		}
		
		return await this.fetchWithAuthorization(`media/chart`, {
			mode: "cors",
			method: "GET",
			headers: {
				"Content-Type": "application/json",
				"Accept-Language": this.culture,
				"Authorization": LocalStorage.getBearerToken()
			}
		}, queryParams);
	}

	static async getPersonsChart(category: string | null = null, pageNumber: number | null = null, pageSize: number | null = null) : Promise<Response> {
		const queryParams = new QueryParams();

		if (category !== null) {
			queryParams.add("category", category);
		}

		if (pageNumber !== null && pageSize !== null) {
			queryParams.add("pageNumber", pageNumber.toString());
			queryParams.add("pageSize", pageSize.toString());
		}
		
		return await this.fetchWithAuthorization(`person/chart`, {
			mode: "cors",
			method: "GET",
			headers: {
				"Content-Type": "application/json",
				"Accept-Language": this.culture,
				"Authorization": LocalStorage.getBearerToken()
			}
		}, queryParams);
	}

	static async addMedia(body: AddMediaContract) : Promise<Response> {		
		return await this.fetchWithAuthorization(`media`, {
			mode: "cors",
			method: "POST",
			headers: {
				"Content-Type": "application/json",
				"Accept-Language": this.culture,
				"Authorization": LocalStorage.getBearerToken()
			},
			body: JSON.stringify(body)
		});
	}

	static async resendConfirmationEmail(email: string) : Promise<Response> {
		const queryParams = new QueryParams();

		if (email !== null) {
			queryParams.add("email", email);
		}

		return await this.fetchWithAuthorization(`user/resend-email-confirmation`, {
			mode: "cors",
			method: "POST",
			headers: {
				"Content-Type": "application/json",
				"Accept-Language": this.culture
			}
		}, queryParams);
	}

	static async updateRoles(body: UpdateRolesContract) : Promise<Response> {
		return await this.fetchWithAuthorization(`user/roles`, {
			mode: "cors",
			method: "PUT",
			headers: {
				"Content-Type": "application/json",
				"Accept-Language": this.culture,
				"Authorization": LocalStorage.getBearerToken()
			},
			body: JSON.stringify(body)
		});
	}

	static async paypalAuthorization() : Promise<Response> {
		return await this.fetchWithAuthorization(`payment/paypal/authorization`, {
			mode: "cors",
			method: "POST",
			headers: {
				"Content-Type": "application/json",
				"Accept-Language": this.culture,
				"Authorization": LocalStorage.getBearerToken()
			}
		});
	}

	static async paypalPlan(paypalAccessToken: string) : Promise<Response> {
		const queryParams = new QueryParams();

		if (paypalAccessToken) {
			queryParams.add("paypalAccessToken", paypalAccessToken);
		}

		return await this.fetchWithAuthorization(`payment/paypal/plan`, {
			mode: "cors",
			method: "GET",
			headers: {
				"Content-Type": "application/json",
				"Accept-Language": this.culture,
				"Authorization": LocalStorage.getBearerToken()
			}
		}, queryParams);
	}
}