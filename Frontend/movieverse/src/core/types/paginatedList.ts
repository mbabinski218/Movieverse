export interface PaginatedList<T> {
	items: T[];
	pageNumber: number | null;
	totalPages: number | null;
	totalCount: number;
	totalItems: number;
	hasPreviousPage: boolean;
	hasNextPage: boolean;
}

export class PaginatedListWrapper {
		static empty<T>(): PaginatedList<T> {
			return {
				items: [],
				pageNumber: null,
				totalPages: null,
				totalCount: 0,
				totalItems: 0,
				hasPreviousPage: false,
				hasNextPage: false
			};
		}
}