import { PaginatedList } from "../../types/paginatedList";
import { MediaDemoDto } from "./mediaDemoDto";

export interface FilteredMediaDto {
    platformId: string;
    platformName: string;
    media: PaginatedList<MediaDemoDto>
}