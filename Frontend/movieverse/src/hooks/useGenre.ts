import { useEffect, useState } from "react";
import { Api } from "../Api";
import { GenreInfoDto } from "../core/dtos/genre/genreInfoDto";

export const useGenre = (mediaId: string) => {
  const [genre, setGenre] = useState<GenreInfoDto[] | null>(null);

  useEffect(() => {
    try {
      Api.getGenre(mediaId)
        .then(res => {
          if (res.ok) {
            return res.json();
          }
        })
        .then(data => data as GenreInfoDto[])
        .then(setGenre);          
		}
    catch {
      return;
    }
  }, []);

  return [genre, setGenre] as [typeof genre, typeof setGenre];
}