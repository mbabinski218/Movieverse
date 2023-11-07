import { useEffect, useState } from "react";
import { Api } from "../Api";
import { StatisticsDto } from "../core/dtos/media/statisticsDto";

export const useStatistics = (mediaId: string) => {
  const [statistics, setStatistics] = useState<StatisticsDto | null>(null);

  useEffect(() => {
    try {
      Api.getStatistics(mediaId)
        .then(res => {
          if (res.ok) {
            return res.json();
          }
        })
        .then(data => data as StatisticsDto)
        .then(setStatistics);          
		}
    catch {
      return;
    }
  }, []);

  return [statistics, setStatistics] as [typeof statistics, typeof setStatistics];
}