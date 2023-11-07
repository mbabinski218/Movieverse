import { useEffect, useState } from "react";
import { Api } from "../Api";
import { SeasonsInfoDto } from "../core/dtos/media/seasonsInfoDto";


export const useSeasons = (mediaId: string) => {
  const [seasons, setSeasons] = useState<SeasonsInfoDto | null>(null);

  useEffect(() => {
    try {
      Api.getSeasons(mediaId)
        .then(res => {
          if (res.ok) {
            return res.json();
          }
        })
        .then(data => data as SeasonsInfoDto)
        .then(setSeasons);          
		}
    catch {
      return;
    }
  }, []);

  return [seasons, setSeasons] as [typeof seasons, typeof setSeasons];
}