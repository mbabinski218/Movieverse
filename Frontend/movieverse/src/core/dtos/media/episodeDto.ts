import { DetailsDto } from "./detailsDto";

export type EpisodeDto = {
  episodeNumber: number,
  title: string,
  details: DetailsDto
};