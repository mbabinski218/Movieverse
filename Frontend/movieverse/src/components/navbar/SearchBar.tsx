import { SearchMediaDto } from "../../core/dtos/media/searchMediaDto";
import { PaginatedList, PaginatedListWrapper } from "../../core/types/paginatedList";
import { useEffect, useState } from "react";
import { useDebounce } from "../../hooks/useDebounce";
import { SearchList } from "./SearchList";
import { Api } from "../../Api";
import SearchIcon from "../../assets/search.svg";
import "./SearchBar.css";

interface SearchBarProps {
  searchBarOpen: boolean;
  onSelect: () => void;
  searchRef?: React.MutableRefObject<null>;
}

export const SearchBar: React.FC<SearchBarProps> = ({searchBarOpen, onSelect, searchRef}) => {
  const [input, setInput] = useState<string>("");
  const [searchResult, setSearchResult] = useState<PaginatedList<SearchMediaDto>>(PaginatedListWrapper.empty<SearchMediaDto>);

  const debouncedInput = useDebounce(input);

  useEffect(() => {  
    if (debouncedInput) {
      Api.searchMedia(debouncedInput, 1, 4)
        .then((res) => {
          setSearchResult(res);
        })
        .catch((err) => {
          console.error(err);
        });
    }
    else {
      setSearchResult(PaginatedListWrapper.empty<SearchMediaDto>());
    }
  }, [debouncedInput]);

  return (
    <div className="search" id="search" ref={searchRef}> 
      <div className="search-bar">
        <input placeholder="Search" onSelect={onSelect} onChange={(e => setInput(e.target.value))} />
        <a className="search-center" href={input ? `/find?term=${input}` : "/find"}>
          <img src={SearchIcon} alt="search" className="search-icon" />
        </a>          
      </div>
      <div className={searchBarOpen ? "list-open" : "list-close"} >
        <SearchList searchResult={searchResult} />
      </div>
    </div>
  )
}