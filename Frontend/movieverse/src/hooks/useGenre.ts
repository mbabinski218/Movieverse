import { useEffect, useState } from "react";
import { Api } from "../Api";
import { GenreDto } from "../core/dtos/genre/genreDto";

export const useGenre = (mediaId: string) => {
  const [genre, setGenre] = useState<GenreDto[] | null>(null);

  useEffect(() => {
    try {
      Api.getGenre(mediaId)
        .then(res => {
          if (res.ok) {
            return res.json();
          }
        })
        .then(data => data as GenreDto[])
        .then(setGenre);          
		}
    catch {
      return;
    }
  }, []);

  return [genre, setGenre] as [typeof genre, typeof setGenre];
}