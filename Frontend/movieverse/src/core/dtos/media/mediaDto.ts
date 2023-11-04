import { BasicStatisticsDto } from "./basicStatisticsDto"
import { DetailsDto } from "./detailsDto"
import { SeasonDto } from "./seasonDto"
import { StaffDto } from "./staffDto"
import { TechnicalSpecsDto } from "./technicalSpecsDto"

export interface MediaDto {
  title: string,
  details: DetailsDto,
  technicalSpecs: TechnicalSpecsDto,
  currentPosition: number | null,
  basicStatistics: BasicStatisticsDto,
  posterId: string | null,
  trailerId: string | null,
  platformIds: string[],
  genreIds: string[],
  staff: StaffDto[]
};

export interface MovieDto extends MediaDto {
  sequelId: string | null,
  sequelTitle: string | null,
  prequelId: string | null,
  prequelTitle: string | null
};

export interface SeriesDto extends MediaDto {
  seasons: SeasonDto[]
  seasonCount: number,
  episodeCount: number,
  seriesEnded: string | null
};