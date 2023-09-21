import SearchIcon from "../../assets/search.svg";
import "./Navbar.css";

export const SearchBar: React.FC = () => {
  return (
  <div className="search"> 
    <input placeholder="Search" />
    <img src={SearchIcon} className="search-icon" />
  </div>
  )
}