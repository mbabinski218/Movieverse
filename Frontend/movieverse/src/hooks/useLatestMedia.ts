import { useEffect, useState } from "react";
import { FilteredMediaDto } from "../core/dtos/media/filteredMediaDto";
import { Api } from "../Api";

export const useLatestMedia = (platformId?: string | null, pageNumber?: number | null, pageSize?: number | null) => {
  const [filteredMedia, setFilteredMedia] = useState<FilteredMediaDto[]>([]);

  useEffect(() => {
    try {
      Api.getLatestMedia(platformId, pageNumber, pageSize)
			  .then((media) => setFilteredMedia(media))
		}
    catch {
      console.error("Error while fetching latest media");
      return;
    }
  }, []);

  return [filteredMedia, setFilteredMedia] as [typeof filteredMedia, typeof setFilteredMedia];
}