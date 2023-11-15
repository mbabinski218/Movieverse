export enum Role {
  Director,
  Writer,
  Actor,
  Creator,
  Producer,
  Composer,
  Cinematographer,
  Editor,
  ArtDirector,
  CostumeDesigner,
  MakeupArtist,
  SoundDesigner,
  Other
}

export interface UpdateMediaContract {
  Title?: string;
  Details?: Details;
  TechnicalSpecs?: TechnicalSpecs;
  Poster?: File;
  Trailer?: string;
  ImagesToAdd: File[];
  VideosToAdd: string[];
  ContentToRemove: string[];
  PlatformIds: string[];
  GenreIds: number[];
  Staff: PostStaffDto[];
  MovieInfo?: MovieInfoDto;
  SeriesInfo: SeriesInfoDto;
}

interface Details {
  Runtime?: number;
  Certificate?: number;
  Storyline?: string;
  Tagline?: string;
  ReleaseDate?: Date;
  CountryOfOrigin?: string;
  Language?: string;
  FilmingLocations?: string;
}

interface TechnicalSpecs {
  Color?: string;
  AspectRatio?: string;
  SoundMix?: string;
  Camera?: string;
  NegativeFormat?: string;
}

interface PostStaffDto {
  PersonId?: string;
  Role?: Role;
}

interface MovieInfoDto {
  PrequelId?: string;
  SequelId?: string;
}

interface SeriesInfoDto {
  Seasons: PostSeasonDto[];
  SeriesEnded?: Date;
}

interface PostSeasonDto {
  SeasonNumber?: number;
  Episodes: PostEpisodeDto[];
}

interface PostEpisodeDto {
  EpisodeNumber?: number;
  Title?: string;
  Details?: Details;
}