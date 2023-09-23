import { useState } from "react";
import { useDebounce } from "../../hooks/useDebounce";
import SearchIcon from "../../assets/search.svg";
import "./SearchBar.css";

interface SearchBarProps {
  onClick: () => void;
}

export const SearchBar: React.FC<SearchBarProps> = ({onClick}) => {
  const [input, setInput] = useState("");
  const debouncedInput = useDebounce(input);

  return (
    <div className="search"> 
      <input placeholder="Search" onClick={onClick} onChange={
        (e) => {
          setInput(e.target.value);
        }
      }/>
      <img src={SearchIcon} className="search-icon" />
    </div>
  )
}