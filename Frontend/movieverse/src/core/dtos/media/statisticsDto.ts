export type StatisticsDto = {
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