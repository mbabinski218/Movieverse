import { ChangeEvent, useCallback, useEffect, useState } from "react"
import { useNavigate, useParams } from "react-router-dom";
import { List, ListItem } from "../components/list/List"
import { MediaListElement } from "../components/list/mediaListElement"
import { PaginatedList, PaginatedListWrapper } from "../core/types/paginatedList"
import { SearchMediaDto } from "../core/dtos/media/searchMediaDto"
import { LocalStorage } from "../hooks/useLocalStorage";
import { StringFormatter } from "../common/stringFormatter";
import { SearchPersonDto } from "../core/dtos/person/searchPersonDto";
import { Api } from "../Api";
import "./Chart.css"

export const Chart: React.FC = () => {
  const params = useParams();
  const [type] = useState<string | null>(params.type ?? null)
  const [category] = useState<string | null>(params.category ?? null)
  const [title, setTitle] = useState<string>("")
  const [pageNumber, setPageNumber] = useState<number>(1);
  const [list, setList] = useState<PaginatedList<ListItem>>(PaginatedListWrapper.empty<ListItem>)
  const [func, setFunc] = useState<((pageNumber: number, pageSize: number) => {}) | null>(null)
  const navigate = useNavigate()
  const pageSize = 10;

  useEffect(() => {
    if (!LocalStorage.getAccessToken()) {
      navigate("/user");
    }

    if (type === null) {
      navigate("/")
    }

    switch (type) {    
      case "watchlist":
        setTitle("Your watchlist");
        document.title = "Watchlist - Movieverse";
        const updateWatchlistFunc = ((pageNumber: number, pageSize: number) => {
          Api.getWatchlist(pageNumber, pageSize)
            .then(res => res.json())
            .then(res => res as PaginatedList<SearchMediaDto>)
            .then(res => setList(PaginatedListWrapper.mapTo<SearchMediaDto, ListItem>(res, (item) => {
              const date = item.releaseDate ? new Date(item.releaseDate).getFullYear().toString() : "Unknown release date";
              return {
                id: item.id,
                label: item.title,
                stats: date,
                description: item.description,
                image: item.poster
              }
            })))
            .catch(console.error)
        })
        setFunc(() => updateWatchlistFunc);
        break

      case "movies":
      case "series":
        setTitle(StringFormatter.convertFromCamelCase(type) + " > " + StringFormatter.convertFromCamelCase(category ?? ""));
        document.title = "Chart - Movieverse";
        const fullDate = category === "releaseCalendar";

        const updateMoviesFunc = ((pageNumber: number, pageSize: number) => {
          Api.getMediaChart(type, category, pageNumber, pageSize)
            .then(res => res.json())
            .then(res => res as PaginatedList<SearchMediaDto>)
            .then(res => setList(PaginatedListWrapper.mapTo<SearchMediaDto, ListItem>(res, (item) => {
              let date = "Unknown release date";
              if (item.releaseDate) {
                date = fullDate ? new Date(item.releaseDate).toLocaleDateString() : new Date(item.releaseDate).getFullYear().toString();
              }

              return {
                id: item.id,
                label: item.title,
                stats: date,
                description: item.description,
                image: item.poster
              }
            })))
            .catch(console.error)
        })
        setFunc(() => updateMoviesFunc);

        break;

      case "persons":
        setTitle("Persons > " + StringFormatter.convertFromCamelCase(category ?? ""));
        document.title = "Chart - Movieverse";
        const updatePersonsFunc = ((pageNumber: number, pageSize: number) => {
          Api.getPersonsChart(category, pageNumber, pageSize)
            .then(res => res.json())
            .then(res => res as PaginatedList<SearchPersonDto>)
            .then(res => setList(PaginatedListWrapper.mapTo<SearchPersonDto, ListItem>(res, (item) => {
              return {
                id: item.id,
                label: item.fullName,
                stats: "Age: " + item.age?.toString() ?? "Unknown",
                description: item.biography,
                image: item.picture
              }
            })))
            .catch(console.error)
        })
        setFunc(() => updatePersonsFunc);
        break;

      default:
        navigate("/")
        break;
    }
  }, [])

  useEffect(() => {
    if (func !== null) {
     func(pageNumber, pageSize)    
    }
  }, [pageNumber, pageSize, func])

  const onPageChange = useCallback((e: ChangeEvent<unknown>, page: number) => {
    setPageNumber(page);
  }, []);

  return (
    <div className="chart-page">
      <div className="chart-title">
        <span>{title}</span>
      </div>
      <List element={MediaListElement}
            list={list}
            pageSize={pageSize}
            onPageChange={onPageChange}
      />
    </div>
  )
}