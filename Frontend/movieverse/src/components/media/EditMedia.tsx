import * as Yup from "yup";
import { ErrorMessage, Field, FieldArray, FieldArrayRenderProps, Form, Formik } from "formik";
import { useCallback, useEffect, useState } from "react";
import { useMedia } from "../../hooks/useMedia";
import { Loading } from "../basic/Loading";
import { PageBlur } from "../basic/PageBlur";
import { Role, UpdateMediaContractExtensions, UpdateMediaContract } from "../../core/contracts/updateMediaContract";
import { useGenres } from "../../hooks/useGenres";
import { usePlatforms } from "../../hooks/usePlatforms";
import { useContent } from "../../hooks/useContent";
import { MovieDto, SeriesDto } from "../../core/dtos/media/mediaDto";
import { useGenre } from "../../hooks/useGenre";
import { usePlatform } from "../../hooks/usePlatform";
import { SearchPersonDto } from "../../core/dtos/person/searchPersonDto";
import { SearchMediaDto } from "../../core/dtos/media/searchMediaDto";
import { SeasonsInfoDto } from "../../core/dtos/media/seasonsInfoDto";
import { useDebounce } from "../../hooks/useDebounce";
import { useStaff } from "../../hooks/useStaff";
import { CloudStore } from "../../CloudStore";
import { Api } from "../../Api";
import "./EditMedia.css";
import Close from "../../assets/bars-close.svg";

// Validation schemas
const DetailsSchema = Yup.object().shape({
  Runtime: Yup.number().positive('Runtime must be positive').nullable(),
  Certificate: Yup.number().positive('Certificate must be positive').nullable().max(18, 'Certificate must be less than 18'),
  Storyline: Yup.string().nullable().max(1000, 'Storyline must be less than 1000 characters'),
  Tagline: Yup.string().nullable().max(1000, 'Tagline must be less than 1000 characters'),
  CountryOfOrigin: Yup.string().nullable(),
  Language: Yup.string().nullable(),
  FilmingLocations: Yup.string().nullable(),
});

const TechnicalSpecsSchema = Yup.object().shape({
  Color: Yup.string().nullable(),
  AspectRatio: Yup.string().nullable(),
  SoundMix: Yup.string().nullable(),
  Camera: Yup.string().nullable(),
  NegativeFormat: Yup.string().nullable(),
});

const PostEpisodeDtoSchema = Yup.object().shape({
  EpisodeNumber: Yup.number().positive('Episode number must be positive').required('Episode number is required'),
  Title: Yup.string().required('Episode title is required').max(100, 'Episode title must be less than 100 characters'),
  Details: DetailsSchema,
});

const PostSeasonDtoSchema = Yup.object().shape({
  SeasonNumber: Yup.number().positive('Season number must be positive').required('Season number is required'),
  Episodes: Yup.array().of(PostEpisodeDtoSchema),
});

const SeriesInfoDtoSchema = Yup.object().shape({
  Seasons: Yup.array().of(PostSeasonDtoSchema),
  SeriesEnded: Yup.date().nullable(),
}).nullable();

const MovieInfoDtoSchema = Yup.object().shape({
  PrequelId: Yup.string().nullable(),
  SequelId: Yup.string().nullable(),
}).nullable();

const PostStaffDtoSchema = Yup.object().shape({
  Name: Yup.string().required('Person is required'),
  Role: Yup.mixed().oneOf(Object.values(Role)).required('Role is required'),
});

const updateMediaContractSchema = Yup.object().shape({
  Title: Yup.string().required("Title is required").max(100, 'Episode title must be less than 100 characters'),
  Details: DetailsSchema,
  TechnicalSpecs: TechnicalSpecsSchema,
  Poster: Yup.mixed().nullable(),
  Trailer: Yup.string().url('Enter a valid URL').nullable(),
  ImagesToAdd: Yup.array().of(Yup.mixed().nullable()),
  VideosToAdd: Yup.array().of(Yup.string().url('Enter a valid URL').nullable()),
  ContentToRemove: Yup.array().of(Yup.string().required('Content is required')),
  PlatformIds: Yup.array().of(Yup.string().nullable()),
  GenreIds: Yup.array().of(Yup.string().nullable()),
  Staff: Yup.array().of(PostStaffDtoSchema),
  MovieInfo: MovieInfoDtoSchema,
  SeriesInfo: SeriesInfoDtoSchema,
});

// Props
export interface EditMediaProps {
  mediaId: string;
  onClose?: () => void;
};

// Component
export const EditMedia: React.FC<EditMediaProps> = (props) => {
  const [media] = useMedia(props.mediaId);
  const [mediaGenres] = useGenre(props.mediaId);
  const [mediaPlatform] = usePlatform(props.mediaId);
  const [content, setContent] = useContent(props.mediaId);
  const [staff] = useStaff(props.mediaId);

  const [genres] = useGenres();
  const [platforms] = usePlatforms();
  const [people, setPeople] = useState<SearchPersonDto[]>([]);
  const [sequels, setSequels] = useState<SearchMediaDto[]>([]);
  const [prequels, setPrequels] = useState<SearchMediaDto[]>([]);
  const [personInput, setPersonInput] = useState<string>("");
  const [sequelInput, setSequelInput] = useState<string>("");
  const [prequelInput, setPrequelInput] = useState<string>("");
  const debouncedPersonInput = useDebounce(personInput);
  const debouncedSequelInput = useDebounce(sequelInput);
  const debouncedPrequelInput = useDebounce(prequelInput);

  const [updating, setUpdating] = useState<boolean>(false);
  const [loading, setLoading] = useState<boolean>(true);
  const [updateMediaContract, setUpdateMediaContract] = useState<UpdateMediaContract>(UpdateMediaContractExtensions.initialValues);

  // On load
  useEffect(() => {
    if (media && content && mediaGenres && mediaPlatform && staff) {      
      const data: UpdateMediaContract = UpdateMediaContractExtensions.initialValues;

      data.Title = media.title;
      data.Details = {
        Runtime: media.details.runtime,
        Certificate: media.details.certificate,
        Storyline: media.details.storyline,
        Tagline: media.details.tagline,
        ReleaseDate: media.details.releaseDate ? formatDate(media.details.releaseDate) : null,
        CountryOfOrigin: media.details.countryOfOrigin,
        Language: media.details.language,
        FilmingLocations: media.details.filmingLocations,
      };
      data.TechnicalSpecs = {
        Color: media.technicalSpecs.color,
        AspectRatio: media.technicalSpecs.aspectRatio,
        SoundMix: media.technicalSpecs.soundMix,
        Camera: media.technicalSpecs.camera,
        NegativeFormat: media.technicalSpecs.negativeFormat,
      };
      
      if (media.trailerId) {
        data.ChangeTrailer = false;
        Api.getVideoPath(media.trailerId)
          .then(res => {
            data.Trailer = res;          
          })
      }
      else {
        data.ChangeTrailer = true;
      }
        
      data.ChangePoster = media.posterId ? false : true;

      const genres = mediaGenres?.map(genre => genre.id);
      data.GenreIds = genres ? genres : [];

      const selectedIds = mediaPlatform?.map(platform => platform.id);
      data.PlatformIds = selectedIds ? selectedIds : [];

      data.Staff = staff.map(staff => ({
        Name: staff.firstName + " " + staff.lastName,
        PersonId: staff.personId,
        Role: Role[staff.role as keyof typeof Role]
      }));

      if (isMovie(media)) {
        data.MovieInfo= {
          PrequelId: media.prequelId,
          PrequelName: media.prequelTitle,
          SequelId: media.sequelId,
          SequelName: media.sequelTitle,
        };
        setPrequelInput(media.prequelTitle ?? "");
        setSequelInput(media.sequelTitle ?? "");
      }

      if (isSeries(media)) {
        Api.getSeasons(props.mediaId)
          .then(res => {
            if(res.ok) {
              res.json()
              .then(res => res as SeasonsInfoDto)
                .then(info => {                
                  data.SeriesInfo = {
                    Seasons: info.seasons.map(season => ({
                      SeasonNumber: season.seasonNumber,
                      Episodes: season.episodes.map(episode => ({
                        EpisodeNumber: episode.episodeNumber,
                        Title: episode.title,
                        Details: {
                          Runtime: episode.details.runtime,
                          Certificate: episode.details.certificate,
                          Storyline: episode.details.storyline,
                          Tagline: episode.details.tagline,
                          ReleaseDate: episode.details.releaseDate ? formatDate(episode.details.releaseDate) : null,
                          CountryOfOrigin: episode.details.countryOfOrigin ,
                          Language: episode.details.language,
                          FilmingLocations: episode.details.filmingLocations,
                        },
                      })),
                    })),
                    SeriesEnded: media.seriesEnded ? formatDate(media.seriesEnded) : null,
                  };
                });
            }
          });
        }

      setUpdateMediaContract(data);
      setLoading(false);
    }
  }, [media, content, mediaGenres, mediaPlatform, staff]);

  // Check if media is movie
  const isMovie = useCallback((media: MovieDto | SeriesDto | null): media is MovieDto => {
    return media !== null && "sequelId" in media && "prequelId" in media ;
  }, []);

  // Check if media is series
  const isSeries = useCallback((media: MovieDto | SeriesDto | null): media is SeriesDto => {
    return media !== null && "seasonCount" in media && "episodeCount" in media;
  }, []);

  // Format date
  const formatDate = useCallback((date: Date | string) => {
    if (typeof date === "string") {
      date = new Date(date);
    }

    if (date) {
      const year = date.getFullYear();
      const month = (`0${date.getMonth() + 1}`).slice(-2);
      const day = (`0${date.getDate()}`).slice(-2);

      return `${year}-${month}-${day}`;
    }
    return "";
  }, []);

  // Remove picture
  const removePicture = useCallback((arrayHelpers: FieldArrayRenderProps, index: number) => {
    if (content) {
      const pictureId = content[index].id;
      arrayHelpers.push(pictureId); 

      setContent(currentPictures => {
        if (currentPictures) {
          return currentPictures.filter((_, i) => i !== index);
        }
        return currentPictures;
      });
    }
  }, [content]);

  // Fetch people
  useEffect(() => {
    if (debouncedPersonInput) {
      Api.searchPeople(debouncedPersonInput, 1, 10)
        .then(res => {
          setPeople(res.items);
        });
    }
    else {
      setPeople([]);
    }
  }, [debouncedPersonInput]);

  // Fetch sequels
  useEffect(() => {
    if (debouncedSequelInput) {
      Api.searchMedia(debouncedSequelInput, 1, 10)
        .then(res => {
          setSequels(res.items);
        });
    }
    else {
      setSequels([]);
    }
  }, [debouncedSequelInput]);

  // Fetch prequels
  useEffect(() => {
    if (debouncedPrequelInput) {
      Api.searchMedia(debouncedPrequelInput, 1, 10)
        .then(res => {
          setPrequels(res.items);
        });
    }
    else {
      setPrequels([]);
    }
  }, [debouncedPrequelInput]);

  // Submit
  const handleSubmit = useCallback((values: UpdateMediaContract) => {
    setUpdating(true);
    Api.updateMedia(props.mediaId, values)
      .then(res => {
        if (res.ok) {
          setUpdating(false);
          props.onClose?.();
          window.location.reload();
        }
      });

  }, [props.onClose]);

  // Render
  return (
    <>      
      <PageBlur />
      {
        loading ? 
        <div className="content-viewer">
          <Loading />
        </div> :
        <>
          <div className="me-page">
            <img className="me-close"
                src={Close}
                alt="Close"
                onClick={props.onClose}
            />
            <Formik className="formik-container"
                    initialValues={updateMediaContract}
                    validationSchema={updateMediaContractSchema}
                    onSubmit={handleSubmit}
            >
              {({ values, setValues }) => (
                <Form className="form-container">
                  <div className="form-field">
                    <Field name="Title" type="text" placeholder="Title"/>
                    <div className="formik-error">
                      <ErrorMessage name="Title" />
                    </div>
                  </div>

                  {/* Fields for Details */}
                  <span className="edit-break">Details</span>
                  <hr className="edit-break" />
                  <div className="form-field">
                    <Field name="Details.Runtime" type="number" placeholder="Runtime"/>
                    <div className="formik-error">
                      <ErrorMessage name="Details.Runtime" />
                    </div>
                  </div>
                  <div className="form-field">
                    <Field name="Details.Certificate" type="number" placeholder="Certificate"/>
                    <div className="formik-error">
                      <ErrorMessage name="Details.Certificate" />
                    </div>
                  </div>
                  <div className="form-field">
                    <Field name="Details.Storyline" as="textarea" placeholder="Storyline"/>
                    <div className="formik-error">
                      <ErrorMessage name="Details.Storyline" />
                    </div>
                  </div>
                  <div className="form-field">
                    <Field name="Details.Tagline" type="text" placeholder="Tagline"/>
                    <div className="formik-error">
                      <ErrorMessage name="Details.Tagline" />
                    </div>
                  </div>
                  <div className="form-field">
                    <Field name="Details.ReleaseDate" type="date" placeholder="Release date" />
                  </div>
                  <div className="form-field">
                    <Field name="Details.CountryOfOrigin" type="text" placeholder="Country of origin"/>
                    <div className="formik-error">
                      <ErrorMessage name="Details.CountryOfOrigin" />
                    </div>
                  </div>
                  <div className="form-field">
                    <Field name="Details.Language" type="text" placeholder="Language"/>
                    <div className="formik-error">
                      <ErrorMessage name="Details.Language" />
                    </div>
                  </div>
                  <div className="form-field">
                    <Field name="Details.FilmingLocations" type="text" placeholder="Filming locations"/>
                    <div className="formik-error">
                      <ErrorMessage name="Details.FilmingLocations" />
                    </div>
                  </div>

                  {/* Fields for TechnicalSpecs */}
                  <span className="edit-break">Technical specs</span>
                  <hr className="edit-break" />
                  <div className="form-field">
                    <Field name="TechnicalSpecs.Color" type="text" placeholder="Color"/>
                    <div className="formik-error">
                      <ErrorMessage name="TechnicalSpecs.Color" />
                    </div>
                  </div>
                  <div className="form-field">
                    <Field name="TechnicalSpecs.AspectRatio" type="text" placeholder="Aspect ratio"/>
                    <div className="formik-error">
                      <ErrorMessage name="TechnicalSpecs.AspectRatio" />
                    </div>
                  </div>
                  <div className="form-field">
                    <Field name="TechnicalSpecs.SoundMix" type="text" placeholder="Sound mix"/>
                    <div className="formik-error">
                      <ErrorMessage name="TechnicalSpecs.SoundMix" />
                    </div>
                  </div>
                  <div className="form-field">
                    <Field name="TechnicalSpecs.Camera" type="text" placeholder="Camera"/>
                    <div className="formik-error">
                      <ErrorMessage name="TechnicalSpecs.Camera" />
                    </div>
                  </div>
                  <div className="form-field">
                    <Field name="TechnicalSpecs.NegativeFormat" type="text" placeholder="Negative format"/>
                    <div className="formik-error">
                      <ErrorMessage name="TechnicalSpecs.NegativeFormat" />
                    </div>
                  </div>

                  {/* Poster */}
                  <span className="edit-break">Poster</span>
                  <hr className="edit-break" />
                  <div className="form-field">
                    <FieldArray name="Poster">
                      {(helper) => (
                        <>
                          {
                            !values.ChangePoster ?
                            <div className="edit-poster">
                              <img src={media?.posterId ? CloudStore.getImageUrl(media.posterId) : ""} 
                                  alt="Poster" 
                                  className="poster-image"
                              /> 
                              <button type="button" 
                                      className={"poster-remove-button"} 
                                      onClick={() => {
                                        setValues(prevState => ({
                                          ...prevState,
                                          ChangePoster: true
                                        }));
                                      }}
                              >
                                X
                              </button>
                            </div> :
                            <input name="Poster"
                                   type="file" 
                                   onChange={(e: any) => {
                                      helper.form.setFieldValue("Poster", e.currentTarget.files[0]);
                                   }}
                            />
                          }
                          <div className="formik-error">
                            <ErrorMessage name="Poster" />
                          </div>                        
                        </>                        
                        )
                      }
                    </FieldArray>
                  </div>

                  {/* Trailer */}
                  <span className="edit-break">Trailer</span>
                  <hr className="edit-break" />
                  <div className="form-field">
                    <Field name="Trailer" 
                           type="text" 
                           placeholder="Trailer url"
                           onChange={(e: React.ChangeEvent<HTMLInputElement>) => {
                              setValues(prevState => ({
                                ...prevState,
                                ChangeTrailer: true,
                                Trailer: e.target.value,
                              }));
                           }} 
                    />
                    <div className="formik-error">
                      <ErrorMessage name="Trailer" />
                    </div>
                  </div>

                  {/* Content */}
                  <span className="edit-break">Add content</span>
                  <hr className="edit-break" />
                  {/* FieldArray for ImagesToAdd */}
                  <div className="form-field">
                    <FieldArray name="ImagesToAdd">
                      {arrayHelpers => (
                        <div>
                          {values.ImagesToAdd.map((image, index) => (
                            <div key={index}>
                              <input name={`ImagesToAdd.${index}`} 
                                     type="file" 
                                     placeholder="Image"
                                     onChange={(e: any) => {
                                        arrayHelpers.form.setFieldValue(`ImagesToAdd.${index}`, e.currentTarget.files[0]);
                                     }}
                              />
                              <button type="button" 
                                      className="add-button" 
                                      onClick={() => arrayHelpers.remove(index)}
                              >
                                Remove Image
                              </button>
                            </div>
                          ))}
                          <button type="button" 
                                  className="add-button" 
                                  onClick={() => arrayHelpers.push('')}
                          >
                            Add Image
                          </button>
                        </div>
                      )}
                    </FieldArray>
                  </div>

                  {/* FieldArray for VideosToAdd */}
                  <div className="form-field">
                    <FieldArray name="VideosToAdd">
                      {arrayHelpers => (
                        <div>
                          {values.VideosToAdd.map((video, index) => (
                            <div key={index}>
                              <Field name={`VideosToAdd.${index}`} 
                                     type="text" 
                                     placeholder="YouTube url"
                              />
                              <button type="button" 
                                      className="add-button" 
                                      onClick={() => arrayHelpers.remove(index)}
                              >
                                Remove Video
                              </button>
                            </div>
                          ))}
                          <button type="button" 
                                  className="add-button" 
                                  onClick={() => arrayHelpers.push('')}
                          >
                            Add Video
                          </button>
                        </div>
                      )}
                    </FieldArray>
                  </div>

                  {/* FieldArray for ContentToRemove */}
                  {
                    (content && content.length > 0) &&
                    <>
                      <span className="edit-break">Remove content</span>
                      <hr className="edit-break" />
                      <FieldArray name="ContentToRemove">
                        {arrayHelpers => (
                          <div className="content-section">
                            {content.map((picture, index) => (
                              <div key={index} 
                                  className="content-container"
                              >
                                {picture.contentType === "video" ?
                                  <div className="video-content">{picture.path}</div> :
                                  <img src={CloudStore.getImageUrl(picture.path)} 
                                       alt={`Content ${index}`} 
                                  />
                                }
                                <button type="button" 
                                        className={`remove-content-button ${picture.contentType === "video" ? "remove-button-video" : ""}`} 
                                        onClick={() => removePicture(arrayHelpers, index)}
                                >
                                  X
                                </button>
                                <Field name={`ContentToRemove.${index}`} 
                                       type="hidden" 
                                       value={picture.id} 
                                />
                              </div>
                            ))}
                          </div>
                        )}
                      </FieldArray>
                    </>
                  }

                  {/* FieldArray for PlatformIds */}                  
                  {
                    platforms && 
                    <>
                      <span className="edit-break">Platforms</span>
                      <hr className="edit-break" />
                      <div className="form-field">
                        <FieldArray name="PlatformIds">
                          {arrayHelpers => (
                            <div>
                              <select
                                multiple
                                value={values.PlatformIds}
                                onChange={(e: React.ChangeEvent<HTMLSelectElement>) => {
                                  const options = e.target.options;
                                  const selectedIds: string[] = [];
                                  for (let i = 0; i < options.length; i++) {
                                    if (options[i].selected) {
                                      selectedIds.push(options[i].value);
                                    }
                                  }
                                  arrayHelpers.form.setFieldValue('PlatformIds', selectedIds);
                                }}
                              >
                                {platforms.map((platform) => (
                                  <option key={platform.id} 
                                          value={platform.id}
                                  >
                                    {platform.name}
                                  </option>
                                ))}
                              </select>
                            </div>
                          )}
                        </FieldArray>
                      </div>
                    </>
                  }

                  {/* FieldArray for GenreIds */}
                  {
                    genres &&
                    <>
                      <span className="edit-break">Genres</span>
                      <hr className="edit-break" />
                      <div className="form-field">
                        <FieldArray name="GenreIds">
                          {arrayHelpers => (
                            <div>
                              <select
                                className="form-size-l"
                                multiple
                                value={values.GenreIds}
                                onChange={(e: React.ChangeEvent<HTMLSelectElement>) => {
                                  const options = e.target.options;
                                  const selectedIds: number[] = [];
                                  for (let i = 0; i < options.length; i++) {
                                    if (options[i].selected) {
                                      selectedIds.push(parseInt(options[i].value, 10));
                                    }
                                  }
                                  arrayHelpers.form.setFieldValue('GenreIds', selectedIds);
                                }}
                              >
                                {genres.map((genre) => (
                                  <option key={genre.id} 
                                          value={genre.id}
                                  >
                                    {genre.name}
                                  </option>
                                ))}
                              </select>
                            </div>
                          )}
                        </FieldArray>
                      </div>
                    </>
                  }

                  {/* Fields for Staff using FieldArray */}
                  <span className="edit-break">Staff</span>
                  <hr className="edit-break" />
                  <div className="form-field">
                    <FieldArray name="Staff">
                      {staffHelpers => (
                        <div>
                          {values.Staff.map((staff, index) => (
                            <div key={index}>                              
                              {
                                (index !== values.Staff.length - 1 && staff.PersonId) ?
                                <input type="text"
                                       value={staff.Name ?? ""}
                                       readOnly={true}
                                /> :
                                <>
                                  <input type="text"
                                         placeholder="Search for person"
                                         defaultValue={staff.Name ?? ""}
                                         onChange={(e: React.ChangeEvent<HTMLInputElement>) => {
                                            setPersonInput(e.target.value)
                                            if(e.target.value === "") {
                                              staffHelpers.form.setFieldValue(`Staff.${index}.PersonId`, e.target.value);
                                            }
                                         }}
                                  />
                                  {
                                    personInput &&
                                    <Field name={`Staff.${index}.PersonId`} 
                                           as="select"
                                           onChange={(e: React.ChangeEvent<HTMLSelectElement>) => {
                                              staffHelpers.form.setFieldValue(`Staff.${index}.PersonId`, e.target.value);
                                              const person = people.find(p => p.id === e.target.value)
                                              staffHelpers.form.setFieldValue(`Staff.${index}.Name`, person?.fullName);
                                           }}
                                    >
                                      <option value="" disabled>Select person</option>
                                      {
                                        people.map((person) => (
                                          <option key={person.id}
                                                  value={person.id}                                              
                                          >
                                            {person.fullName}
                                          </option>
                                        ))
                                      }
                                    </Field>
                                  }
                                  <div className="formik-error">
                                    <ErrorMessage name={`Staff.${index}.Name`} />
                                  </div>
                                </>
                              }
                              {
                                (index !== values.Staff.length - 1 && staff.PersonId) ?
                                <input type="text"
                                       value={staff.Role ?? ""}
                                       readOnly={true}
                                /> :
                                <>
                                  <Field name={`Staff.${index}.Role`} 
                                         as="select"
                                  >
                                    <option value="" disabled>Select role</option>                                     
                                    {
                                      Object.values(Role).filter(value => typeof value === "string").map(role => (
                                        <option key={role}>
                                          {role}
                                        </option>
                                      ))
                                    }
                                  </Field>
                                  <div className="formik-error">
                                    <ErrorMessage name={`Staff.${index}.Role`} />
                                  </div>
                                </>
                              }
                              <button type="button" 
                                      className="add-button" 
                                      onClick={() => staffHelpers.remove(index)}
                              >
                                Remove Person
                              </button>
                            </div>
                          ))}
                          <button type="button" 
                                  className="add-button"                                  
                                  onClick={() => {
                                    setPersonInput("");
                                    staffHelpers.push({ PersonId: "", Role: "" })
                                  }}
                          >
                            Add Person
                          </button>
                        </div>
                      )}
                    </FieldArray>
                  </div>

                  {/* Fields for MovieInfo or SeriesInfo */}
                  {
                    isMovie(media) ?
                    <div className="form-field">
                      <span className="edit-break">Movie info</span>
                      <hr className="edit-break" />
                      <FieldArray name="MovieInfo">
                        {movieInfoHelper => (
                          <>
                            <input type="text"
                                   placeholder="Search for prequel"
                                   defaultValue={values.MovieInfo?.PrequelName ?? ""}
                                   onChange={(e: React.ChangeEvent<HTMLInputElement>) => {
                                      setPrequelInput(e.target.value)
                                   }}
                            />                 
                              {
                                (prequelInput && prequelInput.length > 0) &&
                                <Field name={"MovieInfo.PrequelId"} 
                                       as="select"
                                >
                                  <option value="" disabled>Select prequel</option>
                                  {
                                    prequels.map((prequel) => (
                                      <option key={prequel.id}
                                              value={prequel.id}                                              
                                      >
                                        {`${prequel.title} ${prequel.releaseDate ? `(${prequel.releaseDate.substring(0, 4)})` : ""}`}
                                      </option>
                                    ))
                                  }
                                </Field>
                              }
                          </>                    
                        )}
                      </FieldArray>
                      <FieldArray name="MovieInfo">
                        {movieInfoHelper => (
                          <>
                            <input type="text"
                                  placeholder="Search for sequel"
                                  defaultValue={values.MovieInfo?.SequelName ?? ""}
                                  onChange={(e: React.ChangeEvent<HTMLInputElement>) => {
                                      setSequelInput(e.target.value)
                                  }}
                            />                 
                              {
                                (sequelInput && sequelInput.length > 0) &&
                                <Field name={"MovieInfo.SequelId"} 
                                       as="select"
                                >
                                  <option value="" disabled>Select sequel</option>
                                  {
                                    sequels.map((sequel) => (
                                      <option key={sequel.id}
                                              value={sequel.id}                                              
                                      >
                                        {`${sequel.title} ${sequel.releaseDate ? `(${sequel.releaseDate.substring(0, 4)})` : ""}`}
                                      </option>
                                    ))
                                  }
                                </Field>
                              }
                          </>                    
                        )}
                      </FieldArray>
                    </div> :
                      <div className="form-field">
                        <span className="edit-break">Series info</span>
                        <hr className="edit-break" />
                        <FieldArray name="SeriesInfo.Seasons">
                        {({ remove, push }) => (
                          <div>
                            {values.SeriesInfo?.Seasons.map((season, seasonIndex) => (
                              <div key={seasonIndex}>
                                <Field name={`SeriesInfo.Seasons.${seasonIndex}.SeasonNumber`} 
                                       type="number" 
                                       id={`SeriesInfo.Seasons.${seasonIndex}.SeasonNumber`}
                                       placeholder="Season number"
                                />
                                <div className="formik-error">
                                  <ErrorMessage name={`SeriesInfo.Seasons.${seasonIndex}.SeasonNumber`} />
                                </div>
                                <FieldArray name={`SeriesInfo.Seasons.${seasonIndex}.Episodes`}>
                                  {episodeHelpers => (
                                    <div>
                                      {season.Episodes.map((episode, episodeIndex) => (
                                        <div key={episodeIndex}>
                                          <Field name={`SeriesInfo.Seasons.${seasonIndex}.Episodes.${episodeIndex}.EpisodeNumber`} 
                                                 type="number" 
                                                 id={`SeriesInfo.Seasons.${seasonIndex}.Episodes.${episodeIndex}.EpisodeNumber`} 
                                                 placeholder="Episode number"
                                          />
                                          <div className="formik-error">
                                            <ErrorMessage name={`SeriesInfo.Seasons.${seasonIndex}.Episodes.${episodeIndex}.EpisodeNumber`} />
                                          </div>
                                          <Field name={`SeriesInfo.Seasons.${seasonIndex}.Episodes.${episodeIndex}.Title`} 
                                                 type="text" 
                                                 id={`SeriesInfo.Seasons.${seasonIndex}.Episodes.${episodeIndex}.Title`} 
                                                 placeholder="Episode title"
                                          />
                                          <div className="formik-error">
                                            <ErrorMessage name={`SeriesInfo.Seasons.${seasonIndex}.Episodes.${episodeIndex}.Title`} />
                                          </div>  
                                          <div className="form-field">
                                            <Field name={`SeriesInfo.Seasons.${seasonIndex}.Episodes.${episodeIndex}.Details.Runtime`} type="number" placeholder="Runtime"/>
                                            <div className="formik-error">
                                              <ErrorMessage name={`SeriesInfo.Seasons.${seasonIndex}.Episodes.${episodeIndex}.Details.Runtime`} />
                                            </div>
                                          </div>
                                          <div className="form-field">
                                            <Field name={`SeriesInfo.Seasons.${seasonIndex}.Episodes.${episodeIndex}.Details.Certificate`} type="number" placeholder="Certificate"/>
                                            <div className="formik-error">
                                              <ErrorMessage name={`SeriesInfo.Seasons.${seasonIndex}.Episodes.${episodeIndex}.Details.Certificate`} />
                                            </div>
                                          </div>
                                          <div className="form-field">
                                            <Field name={`SeriesInfo.Seasons.${seasonIndex}.Episodes.${episodeIndex}.Details.Storyline`} as="textarea" placeholder="Storyline"/>
                                            <div className="formik-error">
                                              <ErrorMessage name={`SeriesInfo.Seasons.${seasonIndex}.Episodes.${episodeIndex}.Details.Storyline`} />
                                            </div>
                                          </div>
                                          <div className="form-field">
                                            <Field name={`SeriesInfo.Seasons.${seasonIndex}.Episodes.${episodeIndex}.Details.Tagline`} type="text" placeholder="Tagline"/>
                                            <div className="formik-error">
                                              <ErrorMessage name={`SeriesInfo.Seasons.${seasonIndex}.Episodes.${episodeIndex}.Details.Tagline`} />
                                            </div>
                                          </div>
                                          <div className="form-field">
                                            <Field name={`SeriesInfo.Seasons.${seasonIndex}.Episodes.${episodeIndex}.Details.ReleaseDate`} type="date" placeholder="Release date" />
                                          </div>
                                          <div className="form-field">
                                            <Field name={`SeriesInfo.Seasons.${seasonIndex}.Episodes.${episodeIndex}.Details.CountryOfOrigin`} type="text" placeholder="Country of origin"/>
                                            <div className="formik-error">
                                              <ErrorMessage name={`SeriesInfo.Seasons.${seasonIndex}.Episodes.${episodeIndex}.Details.CountryOfOrigin`} />
                                            </div>
                                          </div>
                                          <div className="form-field">
                                            <Field name={`SeriesInfo.Seasons.${seasonIndex}.Episodes.${episodeIndex}.Details.Language`} type="text" placeholder="Language"/>
                                            <div className="formik-error">
                                              <ErrorMessage name={`SeriesInfo.Seasons.${seasonIndex}.Episodes.${episodeIndex}.Details.Language`} />
                                            </div>
                                          </div>
                                          <div className="form-field">
                                            <Field name={`SeriesInfo.Seasons.${seasonIndex}.Episodes.${episodeIndex}.Details.FilmingLocations`} type="text" placeholder="Filming locations"/>
                                            <div className="formik-error">
                                              <ErrorMessage name={`SeriesInfo.Seasons.${seasonIndex}.Episodes.${episodeIndex}.Details.FilmingLocations`} />
                                            </div>
                                          </div>
                                          <button type="button" 
                                                  className="add-button" 
                                                  onClick={() => episodeHelpers.remove(episodeIndex)}
                                          >
                                            Remove Episode
                                          </button>
                                        </div>
                                      ))}
                                      <button type="button" 
                                              className="add-button" 
                                              onClick={() => episodeHelpers.push({ EpisodeNumber: "", Title: "" })}
                                      >
                                        Add Episode
                                      </button>
                                    </div>
                                  )}
                                </FieldArray>
                                <button type="button" 
                                        className="add-button" 
                                        onClick={() => remove(seasonIndex)} 
                                >
                                  Remove Season
                                </button>
                              </div>
                            ))}
                            <button type="button" 
                                    className="add-button" 
                                    onClick={() => push({ SeasonNumber: "", Episodes: [] })}
                            >
                              Add Season
                            </button>
                          </div>
                        )}
                      </FieldArray>
                      <Field name="SeriesInfo.SeriesEnded" 
                             type="date" 
                             placeholder="Series ended"
                      />
                      <div className="formik-error">
                        <ErrorMessage name="SeriesInfo.SeriesEnded" />
                      </div>
                    </div>
                  }

                  <button className="submit-button"
                          type="submit"
                          disabled={updating}
                  >
                    {updating ? "Updating..." : "Submit"}
                  </button>
                </Form>
              )}
            </Formik>
          </div>
        </>
      }
    </>
  );
}