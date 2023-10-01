interface Param {
	readonly name: string;
	readonly value: string;
}

export class QueryParams {
	readonly params: Param[] = [];

	add(name: string, value: string) {
		this.params.push({ name, value });
	}

	toString(): string {
		return this.params.map(param => `${param.name}=${param.value}`).join("&");
	}
}