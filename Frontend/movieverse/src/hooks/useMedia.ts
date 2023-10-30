import { useEffect, useState } from "react";
import { Api } from "../Api";
import { MovieDto, SeriesDto } from "../core/dtos/media/mediaDto";

export const useMedia = (id: string) => {
  const [media, setMedia] = useState<MovieDto | SeriesDto | null>(null);

  useEffect(() => {
    try {
      Api.getMedia(id)
        .then(res => {
          if (res.ok) {
            return res.json();
          }
        })
        .then(data => data as MovieDto | SeriesDto)
        .then(setMedia);          
		}
    catch {
      return;
    }
  }, []);

  return [media, setMedia] as [typeof media, typeof setMedia];
}