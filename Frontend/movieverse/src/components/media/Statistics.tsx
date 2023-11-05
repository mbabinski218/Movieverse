import { useStatistics } from "../../hooks/useStatistics";
import { Text } from "../../components/basic/Text";
import "./Statistics.css";

export interface StatisticsProps {
  mediaId: string;
  className?: string;
}

const Statistics: React.FC<StatisticsProps> = ({mediaId, className}) => {
  const [statistics] = useStatistics(mediaId);
  
  return (
    <div className={`${className ? className : ""}`}>
      {
        statistics &&
        <div className="">
          <Text label={`BoxOffice:`} />
          <Text label={`Revenue`} text={`${statistics.boxOffice.revenue}$`} />
          <Text label={`Budget`} text={`${statistics.boxOffice.budget}$`} />
          <hr className="stats-break" />
          <Text label={`Gross:`} />
          <Text label={`US`} text={`${statistics.boxOffice.grossUs}$`} />
          <Text label={`Worldwide`} text={`${statistics.boxOffice.grossWorldwide}$`} />
          <hr className="stats-break" />
          <Text label={`Opening weekend:`} />
          <Text label={`US`} text={`${statistics.boxOffice.openingWeekendUs}$`} />
          <Text label={`Worldwide`} text={`${statistics.boxOffice.openingWeekendWorldwide}$`} />
          <hr className="stats-break" />
          <Text label={`Theaters`} text={`${statistics.boxOffice.theaters}`} />
        </div>
      }
    </div>
  )
}

export default Statistics;