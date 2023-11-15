import { useEffect, useState } from "react";
import { Api } from "../Api";
import { GenreDto } from "../core/dtos/genre/genreDto";

export const useGenres = () => {
  const [genres, setGenres] = useState<GenreDto[] | null>(null);

  useEffect(() => {
    try {
      Api.getGenres()
        .then(setGenres);          
		}
    catch {
      return;
    }
  }, []);

  return [genres, setGenres] as [typeof genres, typeof setGenres];
}