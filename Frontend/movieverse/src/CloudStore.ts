export class CloudStore {
    static url: string = "https//d1mucvmtgjoryw.cloudfront.net";

    static getImageUrl(imageName: string): string {
        return `${this.url}/images/${imageName}`;
    }
}