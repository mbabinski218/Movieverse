import { useEffect, useState } from "react";
import { Api } from "../Api";
import { MovieDto, SeriesDto } from "../core/dtos/media/mediaDto";
import { useNavigate } from "react-router-dom";

export const useMedia = (id: string) => {
  const [media, setMedia] = useState<MovieDto | SeriesDto | null>(null);
  const navigate = useNavigate();

  useEffect(() => {
    try {
      Api.getMedia(id)
        .then(res => {
          if (res.ok) {
            return res.json();
          }
          else if (res.status === 404) {
            navigate("/not-found", { replace: true });
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