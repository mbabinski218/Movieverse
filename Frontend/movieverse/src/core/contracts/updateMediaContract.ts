export enum Role {
  Director = "Director",
  Writer = "Writer",
  Actor = "Actor",
  Creator = "Creator",
  Producer = "Producer",
  Composer = "Composer",
  Cinematographer = "Cinematographer",
  Editor = "Editor",
  ArtDirector = "ArtDirector",
  CostumeDesigner = "CostumeDesigner",
  MakeupArtist = "MakeupArtist",
  SoundDesigner = "SoundDesigner",
  Other = "Other"
}

export interface UpdateMediaContract {
  Title: string | null;
  Details: Details | null;
  TechnicalSpecs: TechnicalSpecs | null;
  Poster: File | null;
  ChangePoster: boolean | null;
  Trailer: string | null;
  ChangeTrailer: boolean | null;
  ImagesToAdd: File[];
  VideosToAdd: string[];
  ContentToRemove: string[];
  PlatformIds: string[];
  GenreIds: string[];
  Staff: PostStaffDto[];
  MovieInfo: MovieInfoDto | null;
  SeriesInfo: SeriesInfoDto | null;
}

export interface Details {
  Runtime: number | null;
  Certificate: number | null;
  Storyline: string | null;
  Tagline: string | null;
  ReleaseDate: string | null;
  CountryOfOrigin: string | null;
  Language: string | null;
  FilmingLocations: string | null;
}

export interface TechnicalSpecs {
  Color: string | null;
  AspectRatio: string | null;
  SoundMix: string | null;
  Camera: string | null;
  NegativeFormat: string | null;
}

export interface PostStaffDto {
  Name: string | null;
  PersonId: string | null;
  Role: Role | null;
}

export interface MovieInfoDto {
  PrequelName: string | null;
  PrequelId: string | null;
  SequelName: string | null;
  SequelId: string | null;
}

export interface SeriesInfoDto {
  Seasons: PostSeasonDto[];
  SeriesEnded: string  | null;
}

export interface PostSeasonDto {
  SeasonNumber: number | null;
  Episodes: PostEpisodeDto[];
}

export interface PostEpisodeDto {
  EpisodeNumber: number | null;
  Title: string | null;
  Details: Details | null;
}

export class UpdateMediaContractExtensions {
  static initialValues: UpdateMediaContract = {
    Title: "",
    Details: {
      Runtime: null,
      Certificate: null,
      Storyline: "",
      Tagline: "",
      ReleaseDate: "",
      CountryOfOrigin: "",
      Language: "",
      FilmingLocations: "",
    },
    TechnicalSpecs: {
      Color: "",
      AspectRatio: "",
      SoundMix: "",
      Camera: "",
      NegativeFormat: "",
    },
    ChangePoster: null,
    Poster: null,
    ChangeTrailer: null,
    Trailer: "",
    ImagesToAdd: [],
    VideosToAdd: [],
    ContentToRemove: [],
    PlatformIds: [],
    GenreIds: [],
    Staff: [],
    MovieInfo: null,
    SeriesInfo: null
  };
}