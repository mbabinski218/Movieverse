import { useGenre } from "../../hooks/useGenre";
import "./Genres.css";

export interface GenresProps {
  mediaId: string;
  className?: string;
}

const Genres: React.FC<GenresProps> = ({mediaId, className}) => {
  const [genres] = useGenre(mediaId);
  
  return (
    <div className={`genre-menu ${className ? className : ""}`}>
      {
        (genres && genres.length > 0) ?
        genres.map((genre, index) => (
          <div className="genre-item"
               key={index}
          >
          {genre.name} 
          </div>
        )) :
        <span className="genre-name">No data</span>
      }
    </div>
  )
}

export default Genres;