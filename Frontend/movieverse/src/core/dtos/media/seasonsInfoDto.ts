import { DetailsDto } from "./detailsDto";

export type SeasonsInfoDto = {
  title: string;
  seasons: SeasonDto[];
}

export type SeasonDto = {
  seasonNumber: number;
  episodes: EpisodeDto[];
}

export type EpisodeDto = {
  episodeNumber: number;
  title: string;
  details: DetailsDto;
}