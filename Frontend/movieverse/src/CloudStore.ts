export class CloudStore {
    static url: string = "https://d3ctyaxr4x7918.cloudfront.net";

    static getImageUrl(imageName: string): string {
        return `${this.url}/images/${imageName}`;
    }
}