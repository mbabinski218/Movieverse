import * as Yup from "yup";
import { ErrorMessage, Field, FieldArray, Form, Formik } from "formik";
import { useCallback, useEffect, useState } from "react";
import { useMedia } from "../../hooks/useMedia";
import { Loading } from "../basic/Loading";
import { PageBlur } from "../basic/PageBlur";
import { Role, UpdateMediaContract } from "../../core/contracts/updateMediaContract";
import { useGenres } from "../../hooks/useGenres";
import { usePlatforms } from "../../hooks/usePlatforms";
import "./EditMedia.css";
import Close from "../../assets/bars-close.svg";

// Validation schemas
const DetailsSchema = Yup.object().shape({
  Runtime: Yup.number().positive('Runtime must be positive').nullable(),
  Certificate: Yup.number().positive('Certificate must be positive').nullable().max(18, 'Certificate must be less than 18'),
  Storyline: Yup.string().nullable().max(1000, 'Storyline must be less than 1000 characters'),
  Tagline: Yup.string().nullable().max(1000, 'Tagline must be less than 1000 characters'),
  ReleaseDate: Yup.date().nullable(),
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
});

const MovieInfoDtoSchema = Yup.object().shape({
  PrequelId: Yup.string().nullable(),
  SequelId: Yup.string().nullable(),
});

const PostStaffDtoSchema = Yup.object().shape({
  PersonId: Yup.string().required('Person ID is required'),
  Role: Yup.mixed().oneOf(Object.values(Role)).required('Role is required'),
});

const updateMediaContractSchema = Yup.object().shape({
  Title: Yup.string().nullable().max(100, 'Episode title must be less than 100 characters'),
  Details: DetailsSchema,
  TechnicalSpecs: TechnicalSpecsSchema,
  Poster: Yup.mixed().nullable(),
  Trailer: Yup.string().url('Enter a valid URL').nullable(),
  ImagesToAdd: Yup.array().of(Yup.mixed().nullable()),
  VideosToAdd: Yup.array().of(Yup.string().url('Enter a valid URL').nullable()),
  ContentToRemove: Yup.array().of(Yup.string().required('Content ID is required')),
  PlatformIds: Yup.array().of(Yup.string().required('Platform ID is required')),
  GenreIds: Yup.array().of(Yup.number().positive('Genre ID must be positive').required('Genre ID is required')),
  Staff: Yup.array().of(PostStaffDtoSchema),
  MovieInfo: MovieInfoDtoSchema,
  SeriesInfo: SeriesInfoDtoSchema,
});

// Initial values
const initialValues: UpdateMediaContract = {
  Title: '',
  Details: {
    Runtime: undefined,
    Certificate: undefined,
    Storyline: '',
    Tagline: '',
    ReleaseDate: undefined,
    CountryOfOrigin: '',
    Language: '',
    FilmingLocations: '',
  },
  TechnicalSpecs: {
    Color: '',
    AspectRatio: '',
    SoundMix: '',
    Camera: '',
    NegativeFormat: '',
  },
  Poster: undefined,
  Trailer: '',
  ImagesToAdd: [],
  VideosToAdd: [],
  ContentToRemove: [],
  PlatformIds: [],
  GenreIds: [],
  Staff: [],
  MovieInfo: {
    PrequelId: undefined,
    SequelId: undefined,
  },
  SeriesInfo: {
    Seasons: [{
      SeasonNumber: undefined,
      Episodes: [{
        EpisodeNumber: undefined,
        Title: '',
        Details: {
          Runtime: undefined,
          Certificate: undefined,
          Storyline: '',
          Tagline: '',
          ReleaseDate: undefined,
          CountryOfOrigin: '',
          Language: '',
          FilmingLocations: '',
        },
      }],
    }],
    SeriesEnded: undefined,
  },
};

// Props
export interface EditMediaProps {
  mediaId: string;
  onClose?: () => void;
};

// Component
export const EditMedia: React.FC<EditMediaProps> = (props) => {
  const [currentMedia] = useMedia(props.mediaId);
  const [genres] = useGenres();
  const [platforms] = usePlatforms();
  const [loading, setLoading] = useState<boolean>(true);
  const [updateMediaContract, setUpdateMediaContract] = useState<UpdateMediaContract>();

  // On media change
  useEffect(() => {
    if (currentMedia) {
      setLoading(false);
    }
  }, [currentMedia]);

  // Submit
  const handleSubmit = useCallback((values: UpdateMediaContract) => {
    
  }, [updateMediaContract]);

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
                    initialValues={initialValues}
                    validationSchema={updateMediaContractSchema}
                    onSubmit={handleSubmit}
            >
              {({ values, setValues }) => (
                <Form className="form-container">
                  <div className="form-field">
                    <Field name="Title" type="text" placeHolder="Title"/>
                    <div className="formik-error">
                      <ErrorMessage name="Title" />
                    </div>
                  </div>

                  {/* Fields for Details */}
                  <div className="form-field">
                    <Field name="Details.Runtime" type="number" placeHolder="Runtime"/>
                    <div className="formik-error">
                      <ErrorMessage name="Details.Runtime" />
                    </div>
                  </div>
                  <div className="form-field">
                    <Field name="Details.Certificate" type="number" placeHolder="Certificate"/>
                    <div className="formik-error">
                      <ErrorMessage name="Details.Certificate" />
                    </div>
                  </div>
                  <div className="form-field">
                    <Field name="Details.Storyline" as="textarea" placeHolder="Storyline"/>
                    <div className="formik-error">
                      <ErrorMessage name="Details.Storyline" />
                    </div>
                  </div>
                  <div className="form-field">
                    <Field name="Details.Tagline" type="text" placeHolder="Tagline"/>
                    <div className="formik-error">
                      <ErrorMessage name="Details.Tagline" />
                    </div>
                  </div>
                  <div className="form-field">
                    <Field name="Details.ReleaseDate" type="date" placeHolder="ReleaseDate"/>
                    <div className="formik-error">
                      <ErrorMessage name="Details.ReleaseDate" />
                    </div>
                  </div>
                  <div className="form-field">
                    <Field name="Details.CountryOfOrigin" type="text" placeHolder="CountryOfOrigin"/>
                    <div className="formik-error">
                      <ErrorMessage name="Details.CountryOfOrigin" />
                    </div>
                  </div>
                  <div className="form-field">
                    <Field name="Details.Language" type="text" placeHolder="Language"/>
                    <div className="formik-error">
                      <ErrorMessage name="Details.Language" />
                    </div>
                  </div>
                  <div className="form-field">
                    <Field name="Details.FilmingLocations" type="text" placeHolder="FilmingLocations"/>
                    <div className="formik-error">
                      <ErrorMessage name="Details.FilmingLocations" />
                    </div>
                  </div>

                  {/* Fields for TechnicalSpecs */}
                  <div className="form-field">
                    <Field name="TechnicalSpecs.Color" type="text" placeHolder="Color"/>
                    <div className="formik-error">
                      <ErrorMessage name="TechnicalSpecs.Color" />
                    </div>
                  </div>
                  <div className="form-field">
                    <Field name="TechnicalSpecs.AspectRatio" type="text" placeHolder="AspectRatio"/>
                    <div className="formik-error">
                      <ErrorMessage name="TechnicalSpecs.AspectRatio" />
                    </div>
                  </div>
                  <div className="form-field">
                    <Field name="TechnicalSpecs.SoundMix" type="text" placeHolder="SoundMix"/>
                    <div className="formik-error">
                      <ErrorMessage name="TechnicalSpecs.SoundMix" />
                    </div>
                  </div>
                  <div className="form-field">
                    <Field name="TechnicalSpecs.Camera" type="text" placeHolder="Camera"/>
                    <div className="formik-error">
                      <ErrorMessage name="TechnicalSpecs.Camera" />
                    </div>
                  </div>
                  <div className="form-field">
                    <Field name="TechnicalSpecs.NegativeFormat" type="text" placeHolder="NegativeFormat"/>
                    <div className="formik-error">
                      <ErrorMessage name="TechnicalSpecs.NegativeFormat" />
                    </div>
                  </div>

                  {/* FieldArray for ImagesToAdd */}
                  <div className="form-field">
                    <FieldArray name="ImagesToAdd">
                      {arrayHelpers => (
                        <div>
                          {values.ImagesToAdd.map((image, index) => (
                            <div key={index}>
                              <Field name={`ImagesToAdd.${index}`} type="file" placeHolder="Image"/>
                              <button type="button" className="add-button" onClick={() => arrayHelpers.remove(index)}>
                                Remove Image
                              </button>
                            </div>
                          ))}
                          <button type="button" className="add-button" onClick={() => arrayHelpers.push('')}>
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
                              <Field name={`VideosToAdd.${index}`} type="text" placeHolder="YouTube url"/>
                              <button type="button" className="add-button" onClick={() => arrayHelpers.remove(index)}>
                                Remove Video
                              </button>
                            </div>
                          ))}
                          <button type="button" className="add-button" onClick={() => arrayHelpers.push('')}>
                            Add Video
                          </button>
                        </div>
                      )}
                    </FieldArray>
                  </div>

                  {/* FieldArray for ContentToRemove */}
                  <div className="form-field">
                    <FieldArray name="ContentToRemove">
                      {arrayHelpers => (
                        <div>
                          {values.ContentToRemove.map((contentId, index) => (
                            <div key={index}>
                              <Field name={`ContentToRemove.${index}`} type="text" />
                              <button type="button" className="add-button" onClick={() => arrayHelpers.remove(index)}>
                                Remove Content
                              </button>
                            </div>
                          ))}
                          <button type="button" className="add-button" onClick={() => arrayHelpers.push('')}>
                            Add Content ID to Remove
                          </button>
                        </div>
                      )}
                    </FieldArray>
                  </div>

                  {/* FieldArray for PlatformIds */}
                  <div className="form-field">
                    <FieldArray name="PlatformIds">
                      {arrayHelpers => (
                        <div>
                          {values.PlatformIds.map((platformId, index) => (
                            <div key={index}>
                              <Field name={`PlatformIds.${index}`} type="text" />
                              <button type="button" className="add-button" onClick={() => arrayHelpers.remove(index)}>
                                Remove Platform
                              </button>
                            </div>
                          ))}
                          <button type="button" className="add-button" onClick={() => arrayHelpers.push('')}>
                            Add Platform ID
                          </button>
                        </div>
                      )}
                    </FieldArray>
                  </div>

                  {/* FieldArray for GenreIds */}
                  {
                    genres &&
                    <div className="form-field">
                      <FieldArray name="GenreIds">
                        {arrayHelpers => (
                          <div>
                            <select
                              multiple
                              value={genres.map(genre => genre.id)}
                              onChange={(e) => {
                                const options = e.target.options;
                                const selectedValues = [];
                                for (let i = 0; i < options.length; i++) {
                                  if (options[i].selected) {
                                    selectedValues.push(parseInt(options[i].value, 10));
                                  }
                                }
                              }}
                            >
                              {genres.map((genre) => (
                                <option key={genre.id} value={genre.id}>
                                  {genre.name}
                                </option>
                              ))}
                            </select>
                          </div>
                        )}
                      </FieldArray>
                    </div>
                  }

                  {/* Fields for Staff using FieldArray */}
                  <div className="form-field">
                    <FieldArray name="Staff">
                      {staffHelpers => (
                        <div>
                          {values.Staff.map((staff, index) => (
                            <div key={index}>
                              <Field name={`Staff.${index}.PersonId`} type="text" placeHolder="Person Id"/>
                              <div className="formik-error">
                                <ErrorMessage name={`Staff.${index}.PersonId`} />
                              </div>
                              <Field name={`Staff.${index}.Role`} as="select">
                                {Object.values(Role).filter(value => typeof value === 'string').map(role => (
                                  <option key={role}>
                                    {role}
                                  </option>
                                ))}
                              </Field>
                              <div className="formik-error">
                                <ErrorMessage name={`Staff.${index}.Role`} />
                              </div>
                              <button type="button" className="add-button" onClick={() => staffHelpers.remove(index)}>Remove Staff</button>
                            </div>
                          ))}
                          <button type="button" className="add-button" onClick={() => staffHelpers.push({ PersonId: '', Role: Role.Actor })}>
                            Add Staff
                          </button>
                        </div>
                      )}
                    </FieldArray>
                  </div>

                  {/* Fields for MovieInfo */}
                  <div className="form-field">
                    <Field name="MovieInfo.PrequelId" type="text" placeHolder="Prequel Id"/>
                    <div className="formik-error">
                      <ErrorMessage name="MovieInfo.PrequelId" />
                    </div>
                  </div>
                  <div className="form-field">
                    <Field name="MovieInfo.SequelId" type="text" placeHolder="Sequel Id"/>
                    <div className="formik-error">
                      <ErrorMessage name="MovieInfo.SequelId" />
                    </div>
                  </div>
                  
                  {/* Fields for SeriesInfo */}
                  <div className="form-field">
                    <FieldArray name="SeriesInfo.Seasons">
                      {({ insert, remove, push }) => (
                        <div>
                          {values.SeriesInfo.Seasons.map((season, index) => (
                            <div key={index}>
                              <Field name={`SeriesInfo.Seasons.${index}.SeasonNumber`} type="number" placeHolder="Season number"/>
                              <div className="formik-error">
                               <ErrorMessage name={`SeriesInfo.Seasons.${index}.SeasonNumber`} />
                              </div>
                              <FieldArray name={`SeriesInfo.Seasons.${index}.Episodes`}>
                                {episodeHelpers => (
                                  <div>
                                    {season.Episodes.map((episode, episodeIndex) => (
                                      <div key={episodeIndex}>
                                        <Field name={`SeriesInfo.Seasons.${index}.Episodes.${episodeIndex}.EpisodeNumber`} type="number" placeHolder="Episode number"/>
                                        <div className="formik-error">
                                          <ErrorMessage className="formik-error" name={`SeriesInfo.Seasons.${index}.Episodes.${episodeIndex}.EpisodeNumber`} />
                                        </div>
                                        <Field name={`SeriesInfo.Seasons.${index}.Episodes.${episodeIndex}.Title`} type="text" placeHolder="Episode title"/>
                                        <div className="formik-error">
                                          <ErrorMessage name={`SeriesInfo.Seasons.${index}.Episodes.${episodeIndex}.Title`} />
                                        </div>
                                      </div>
                                    ))}
                                    <button type="button" className="add-button" onClick={() => episodeHelpers.push({ EpisodeNumber: undefined, Title: '' })}>
                                      Add Episode
                                    </button>
                                  </div>
                                )}
                              </FieldArray>
                              <button type="button" className="add-button" onClick={() => remove(index)}>
                                Remove Season
                              </button>
                            </div>
                          ))}
                          <button type="button" className="add-button" onClick={() => push({ SeasonNumber: undefined, Episodes: [] })}>
                            Add Season
                          </button>
                        </div>
                      )}
                    </FieldArray>
                  </div>
                  <div className="form-field">
                    <Field name="SeriesInfo.SeriesEnded" type="date" />
                    <div className="formik-error">
                      <ErrorMessage name="SeriesInfo.SeriesEnded" />
                    </div>
                  </div>
                  
                  <button type="submit" className="submit-button">Submit</button>
                </Form>
              )}
            </Formik>
          </div>
        </>
      }
    </>
  );
}