import { ChangeEvent, useCallback, useEffect, useState } from "react";
import { useSearchParams } from "react-router-dom";
import { PaginatedList, PaginatedListWrapper } from "../core/types/paginatedList";
import { SearchPersonDto } from "../core/dtos/person/searchPersonDto";
import { SearchMediaDto } from "../core/dtos/media/searchMediaDto";
import { Api } from "../Api";
import { GenreDto } from "../core/dtos/genre/genreDto";
import { List, ListItem } from "../components/list/List";
import { PersonListElement } from "../components/list/PersonListElement";
import { MediaListElement } from "../components/list/MediaListElement";
import "./Find.css";

interface Filters {
  type: "Movie" | "Series" | "Person";
  genre: string | null;
}

export const Find: React.FC = () => {
  const [loading, setLoading] = useState<boolean>(true);
  const [searchParams] = useSearchParams();
  const [term] = useState<string | null>(searchParams.get("term"));
  const [filters, setFilters] = useState<Filters>({ type: "Movie", genre: null });
  const [genre, setGenre] = useState<GenreDto[]>([]);
  const [pageNumber, setPageNumber] = useState<number>(1);
  const [list, setList] = useState<PaginatedList<SearchMediaDto | SearchPersonDto>>(PaginatedListWrapper.empty);
  const pageSize = 10;

  useEffect(() => {    
    document.title = "Find - Movieverse";
  }, []);

  useEffect(() => {
    if (filters.type === "Person") {
      Api.searchPeople(term, pageNumber, pageSize)
        .then(res => {
          setList(res);
          setLoading(false);
        })
        .catch(console.error);
    } 
    else {
      const genreId = genre.find(g => g.name === filters.genre)?.id ?? null;
      Api.searchWithFiltersMedia(term, filters.type, genreId, pageNumber, pageSize)
        .then(res => {
          setList(res);
          setLoading(false);
        })
        .catch(console.error);
    }
  }, [filters, pageNumber]);

  const loadGenres = useCallback(() => {
    if (genre.length > 0) {
      return;
    }

    Api.getGenres()
      .then(setGenre)
      .catch(console.error);
  }, [genre]);

  const onPageChange = useCallback((e: ChangeEvent<unknown>, page: number) => {
    setPageNumber(page);
  }, []);

  const selectTypeHandler = useCallback((e: React.ChangeEvent<HTMLSelectElement>) => {
    setFilters({ ...filters, type: e.target.value as "Movie" | "Series" | "Person" });
    setPageNumber(1);
  }, [filters]);

  const selectGenreHandler = useCallback((e: React.ChangeEvent<HTMLSelectElement>) => {
    const genre = e.target.value === "All genres" ? null : e.target.value;
    setFilters({ ...filters, genre: genre });
    setPageNumber(1);
  }, [filters]);

  return (
    <div className="find-page">
      <div className="find-filters">
        <span className="find-term">{term ? `Search: ${term}` : "Advanced search"}</span>
        <div className="find-items">
          <select className="find-select" 
                  title="Type" 
                  onChange={selectTypeHandler}
          >
            <option value="Movie">Movie</option>
            <option value="Series">Series</option>
            <option value="Person">Person</option>
          </select>
          <select className="find-select" 
                  title="Genre" 
                  disabled={filters.type === "Person"} 
                  onClick={loadGenres} 
                  onChange={selectGenreHandler}
          >
            <option value={undefined}>All genres</option>
            {
              genre.map((genre) => (
                <option key={genre.id} value={genre.name}>{genre.name}</option>
              ))
            }
          </select>
        </div>
      </div>
      {
        filters.type !== "Person" &&
        <List loading={false}
              element={MediaListElement}
              list={PaginatedListWrapper.mapTo<SearchMediaDto, ListItem>(list as PaginatedList<SearchMediaDto>, (item) => {
                const date = item.releaseDate ? new Date(item.releaseDate).getFullYear().toString() : "Unknown release date";
                return {
                  id: item.id,
                  label: item.title,
                  stats: date,
                  description: item.description,
                  image: item.poster
                }
              })}
              pageSize={pageSize}
              onPageChange={onPageChange}
        /> ||      
        <List loading={false}
              element={PersonListElement}
              list={PaginatedListWrapper.mapTo<SearchPersonDto, ListItem>(list as PaginatedList<SearchPersonDto>, (item) => {
                return {
                  id: item.id,
                  label: item.fullName,
                  stats: item.age?.toString() ?? "Unknown age",
                  description: item.biography,
                  image: item.picture
                }
              })}
              pageSize={pageSize}
              onPageChange={onPageChange}
        />
      }
    </div>
  );
}