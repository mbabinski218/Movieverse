export type StatisticsDto = {
  popularity: PopularityDto[];
  boxOffice: BoxOfficeDto;
}

export type BoxOfficeDto = {
  budget: number;
  revenue: number;
  grossUs: number;
  grossWorldwide: number;
  openingWeekendUs: number;
  openingWeekendWorldwide: number;
  theaters: number;
}

export type PopularityDto = {
  date: string;
  position: number;
  change: number;
  views: number;
}