import { EpisodeDto } from "./episodeDto";

export type SeasonDto = {
  seasonNumber: number,
  episodes: EpisodeDto[] | null
};