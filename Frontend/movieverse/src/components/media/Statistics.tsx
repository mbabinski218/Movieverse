import { useStatistics } from "../../hooks/useStatistics";
import { Section } from "../basic/Section";
import { Text } from "../../components/basic/Text";
import { Line } from "react-chartjs-2";
import { Chart as ChartJS,  CategoryScale,  LinearScale,  PointElement,  LineElement,  Title,  Tooltip,  Legend } from 'chart.js'
import "./Statistics.css";

ChartJS.register(
  CategoryScale,
  LinearScale,
  PointElement,
  LineElement,
  Title,
  Tooltip,
  Legend
)

ChartJS.defaults.borderColor = '#424242';
ChartJS.defaults.backgroundColor = '#424242';
ChartJS.defaults.color = '#ffffff';

export interface StatisticsProps {
  mediaId: string;
  className?: string;
}

const Statistics: React.FC<StatisticsProps> = ({mediaId, className}) => {
  const [statistics] = useStatistics(mediaId);

  const config = {
    plugins: {
      legend: {
        display: true,
        borderColor: '#ffcb74'
      }
    }
  }

  return (
    <div className={`${className ? className : ""}`}>
      {
        statistics &&
        <>
          <Section title="BoxOffice (Pro)">
            <Text label={`Revenue`} text={`${statistics.boxOffice.revenue}$`} />
            <Text label={`Budget`} text={`${statistics.boxOffice.budget}$`} />
            <hr className="stats-break" />
            <Text label={`Gross`} />
            <Text label={`US`} text={`${statistics.boxOffice.grossUs}$`} />
            <Text label={`Worldwide`} text={`${statistics.boxOffice.grossWorldwide}$`} />
            <hr className="stats-break" />
            <Text label={`Opening weekend`} />
            <Text label={`US`} text={`${statistics.boxOffice.openingWeekendUs}$`} />
            <Text label={`Worldwide`} text={`${statistics.boxOffice.openingWeekendWorldwide}$`} />
            <hr className="stats-break" />
            <Text label={`Theaters`} text={`${statistics.boxOffice.theaters}`} />
          </Section>
          <div style={{marginBottom: "15px"}} />
          <Section title="Popularity (Pro)">
            {
              statistics.popularity.length > 0 &&
              statistics.popularity.map((popularity, index) => (
                <div key={index}>
                  <Text label={`${popularity.date.substring(0, 7)}`} />
                  <div className="popularity-item">
                    <Text label={`Position`} text={`${popularity.position}`} />
                    <Text label={`Change`} text={`${popularity.change}`} />
                    <Text label={`Views`} text={`${popularity.views}`} />
                  </div>
                  <hr className="stats-break" />
                </div>
              ))
            }
          </Section>
          <div style={{marginBottom: "15px"}} />
          <Section title="Chart (Pro)">
            {
              statistics.popularity.length > 1 ?
              <Line data={{
                      labels: statistics.popularity.map(pop => pop.date.substring(0, 7)),
                      datasets: [{
                        label: "Position",
                        data: statistics.popularity.map(pop => pop.position),
                        borderColor: "#ffffff",
                        backgroundColor: '#ffcb74',
                        borderWidth: 2,
                      }],
                    }}
                    options={config}
              /> :
              <span style={{fontWeight: "bold"}}>Not enough data</span>
            }
          </Section>
        </>
      }
    </div>
  )
}

export default Statistics;