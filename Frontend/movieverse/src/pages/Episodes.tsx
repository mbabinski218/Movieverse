import { useCallback, useEffect, useState } from "react";
import { useParams } from "react-router-dom";
import { useSeasons } from "../hooks/useSeasons";
import { Loading } from "../components/basic/Loading";
import { Section } from "../components/basic/Section";
import { Text } from "../components/basic/Text";
import "./Episodes.css";

export const Episodes: React.FC = () => {
  const params = useParams();
  const [data] = useSeasons(params.id ?? "");
  const [loading, setLoading] = useState(true);
  const [seasonNumber, setSeasonNumber] = useState<number | null>(null);

  useEffect(() => {
    if (data) {
      document.title = `${data.title} - Episodes - Movieverse`;
      setLoading(false);
      if (data.seasons.length > 0) {        
        setSeasonNumber(data.seasons[0].seasonNumber);
      }
    }   
    else {
      if (!loading) {
        document.title = "Not found - Movieverse";
      }
    }
  }, [data]);

  const selectSeasonHandler = useCallback((e: React.ChangeEvent<HTMLSelectElement>) => {
    setSeasonNumber(Number(e.target.value));
  }, []);

  return (
    <>
      {
        loading ? <Loading /> :
        <div className="episodes-page">
          <div className="episodes-title">Episodes</div>
          <div className="episodes-data">
            <select className="episodes-select" 
                    title="Season" 
                    onChange={selectSeasonHandler}
            >
              {
                data &&
                data?.seasons.map((season, index) => (
                  <option key={index} value={season.seasonNumber}>{`Season: ${season.seasonNumber}`}</option>
                ))
              }
            </select>
            {
              data &&
              data.seasons.find(season => season.seasonNumber === seasonNumber)
                ?.episodes.map((episode, index) => (
                  <Section title={`${episode.episodeNumber}. ${episode.title}`} key={index}>
                    <Text label={`Runtime`} text={episode.details.runtime ? `${episode.details.runtime} min` : "unknown"} />
                    <Text label={`Certificate`} text={`${episode.details.certificate ?? "unknown"}`} />
                    <Text label={`Release date`} text={`${episode.details.releaseDate?.substring(0, 10) ?? "unknown"}`} />
                    <Text label={`Tagline`} text={`${episode.details.tagline ?? "unknown"}`} />
                    <Text label={`Country of origin`} text={`${episode.details.countryOfOrigin ?? "unknown"}`} />
                    <Text label={`Language`} text={`${episode.details.language ?? "unknown"}`} />
                    <Text label={`Filming locations`} text={`${episode.details.filmingLocations ?? "unknown"}`} />    
                    <div style={{marginBottom: "30px"}}></div>  
                  </Section>
                ))
            }
          </div>
        </div>        
      }
    </>
  )
}