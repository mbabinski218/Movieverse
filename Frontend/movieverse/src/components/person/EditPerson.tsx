import * as Yup from "yup";
import { usePerson } from "../../hooks/usePerson";
import { UpdatePersonContract, UpdatePersonContractExtensions } from "../../core/contracts/updatePersonContract";
import { ErrorMessage, Field, FieldArray, FieldArrayRenderProps, Form, Formik } from "formik";
import { useCallback, useEffect, useState } from "react";
import { Loading } from "../basic/Loading";
import { PageBlur } from "../basic/PageBlur";
import { CloudStore } from "../../CloudStore";
import { Api } from "../../Api";
import "../media/EditMedia.css";
import Close from "../../assets/bars-close.svg";

// Validation schemas
const Information = Yup.object().shape({
  FirstName: Yup.string().nullable(),
  LastName: Yup.string().nullable(),
  Age: Yup.number().positive('Age must be positive').required('Age is required')
});

const LifeHistory = Yup.object().shape({
  BirthPlace: Yup.string().nullable(),
  DeathPlace: Yup.string().nullable()
});

const updatePersonContractSchema = Yup.object().shape({
  Information: Information,
  LifeHistory: LifeHistory,
  Biography: Yup.string().max(1000, 'Biography must be less than 1000 characters'),
  FunFacts: Yup.string().max(1000, 'Fun facts title must be less than 1000 characters'),
  Picture: Yup.mixed().nullable(),
  Pictures: Yup.array().of(Yup.mixed().nullable()),
  PicturesToRemove: Yup.array().of(Yup.string().required('Content is required')),
});

// Props
export interface EditPersonProps {
  personId: string;
  onClose?: () => void;
};

// Component
export const EditPerson: React.FC<EditPersonProps> = (props) => {
  const [person] = usePerson(props.personId);
  const [content, setContent] = useState<string[] | null>(null);

  const [updating, setUpdating] = useState<boolean>(false);
  const [loading, setLoading] = useState<boolean>(true);
  const [updatePersonContract, setUpdatePersonContract] = useState<UpdatePersonContract>(UpdatePersonContractExtensions.initialValues);

  // On load
  useEffect(() => {
    if (person) {      
      const data: UpdatePersonContract = UpdatePersonContractExtensions.initialValues;
      setContent(person.contentIds);
      
      data.Information = {
        FirstName: person.information?.firstName ?? "",
        LastName: person.information?.lastName ?? "",
        Age: Number(person.information?.age) ?? 0
      };      
      data.LifeHistory = {
        BirthPlace: person.lifeHistory?.birthPlace ?? "",
        BirthDate: person.lifeHistory?.birthDate ? formatDate(person.lifeHistory.birthDate) : null,
        DeathPlace: person.lifeHistory?.deathPlace ?? "",
        DeathDate: person.lifeHistory?.deathDate ? formatDate(person.lifeHistory.deathDate) : null,
        CareerStart: person.lifeHistory?.careerStart ? formatDate(person.lifeHistory.careerStart) : null,
        CareerEnd: person.lifeHistory?.careerEnd ? formatDate(person.lifeHistory.careerEnd) : null,
      };
      data.Biography = person.biography ?? "";
      data.FunFacts = person.funFacts ?? "";

      data.ChangePicture = person.pictureId ? false : true;

      setUpdatePersonContract(data);
      setLoading(false);
    }
  }, [person]);

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
      const pictureId = content[index];
      arrayHelpers.push(pictureId); 

      setContent(currentPictures => {
        if (currentPictures) {
          return currentPictures.filter((_, i) => i !== index);
        }
        return currentPictures;
      });
    }
  }, [content]);

  // Submit
  const handleSubmit = useCallback((values: UpdatePersonContract) => {
    setUpdating(true);
    Api.updatePerson(props.personId, values)
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
                    initialValues={updatePersonContract}
                    validationSchema={updatePersonContractSchema}
                    onSubmit={handleSubmit}
            >
              {({ values, setValues }) => (
                <Form className="form-container">

                  {/* Fields for Information */}
                  <span className="edit-break">Information</span>
                  <hr className="edit-break" />
                  <div className="form-field">
                    <Field name="Information.FirstName" type="text" placeholder="First name"/>
                    <div className="formik-error">
                      <ErrorMessage name="Information.FirstName" />
                    </div>
                  </div>
                  <div className="form-field">
                    <Field name="Information.LastName" type="text" placeholder="Last name"/>
                    <div className="formik-error">
                      <ErrorMessage name="Information.LastName" />
                    </div>
                  </div>
                  <div className="form-field">
                    <Field name="Information.Age" type="number" placeholder="Age"/>
                    <div className="formik-error">
                      <ErrorMessage name="Information.Age" />
                    </div>
                  </div>                  

                  {/* Fields for LifeHistory */}
                  <span className="edit-break">Life history</span>
                  <hr className="edit-break" />
                  <div className="form-field">
                    <Field name="LifeHistory.BirthPlace" type="text" placeholder="Birth place"/>
                    <div className="formik-error">
                      <ErrorMessage name="LifeHistory.BirthPlace" />
                    </div>
                  </div>
                  <div className="form-field">
                    <Field name="LifeHistory.BirthDate" type="date" placeholder="Birth date"/>
                  </div>
                  <div className="form-field">
                    <Field name="LifeHistory.DeathPlace" type="text" placeholder="Death place"/>
                    <div className="formik-error">
                      <ErrorMessage name="LifeHistory.DeathPlace" />
                    </div>
                  </div>
                  <div className="form-field">
                    <Field name="LifeHistory.DeathDate" type="date" placeholder="Death date"/>
                  </div>
                  <div className="form-field">
                    <Field name="LifeHistory.CareerStart" type="date" placeholder="Career start"/>
                  </div>
                  <div className="form-field">
                    <Field name="LifeHistory.CareerEnd" type="date" placeholder="Career end"/>
                  </div>

                  <span className="edit-break">Details</span>
                  <hr className="edit-break" />

                  {/* Biography */}
                  <div className="form-field">
                    <Field name="Biography" as="textarea" placeholder="Biography"/>
                    <div className="formik-error">
                      <ErrorMessage name="Biography" />
                    </div>
                  </div>

                  {/* FunFacts */}
                  <div className="form-field">
                    <Field name="FunFacts" as="textarea" placeholder="Fun facts"/>
                    <div className="formik-error">
                      <ErrorMessage name="FunFacts" />
                    </div>
                  </div>

                  {/* Picture */}
                  <span className="edit-break">Picture</span>
                  <hr className="edit-break" />
                  <div className="form-field">
                    <FieldArray name="Picture">
                      {(helper) => (
                        <>
                          {
                            !values.ChangePicture ?
                            <div className="edit-poster">
                              <img src={person?.pictureId ? CloudStore.getImageUrl(person.pictureId) : ""} 
                                   alt="Picture" 
                                   className="poster-image"
                              /> 
                              <button type="button" 
                                      className={"poster-remove-button"} 
                                      onClick={() => {
                                        setValues(prevState => ({
                                          ...prevState,
                                          ChangePicture: true
                                        }));
                                      }}
                              >
                                X
                              </button>
                            </div> :
                            <input name="Picture"
                                   type="file" 
                                   onChange={(e: any) => {
                                      helper.form.setFieldValue("Picture", e.currentTarget.files[0]);
                                   }}
                            />
                          }
                          <div className="formik-error">
                            <ErrorMessage name="Picture" />
                          </div>                        
                        </>                        
                        )
                      }
                    </FieldArray>
                  </div>

                  {/* Content */}
                  <span className="edit-break">Add content</span>
                  <hr className="edit-break" />
                  {/* FieldArray for Pictures */}
                  <div className="form-field">
                    <FieldArray name="Pictures">
                      {arrayHelpers => (
                        <div>
                          {values.Pictures.map((picture, index) => (
                            <div key={index}>
                              <input name={`Pictures.${index}`} 
                                     type="file" 
                                     placeholder="Image"
                                     onChange={(e: any) => {
                                        arrayHelpers.form.setFieldValue(`Pictures.${index}`, e.currentTarget.files[0]);
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

                  {/* FieldArray for PicturesToRemove */}
                  {
                    (content && content.length > 0) &&
                    <>
                      <span className="edit-break">Remove content</span>
                      <hr className="edit-break" />
                      <FieldArray name="PicturesToRemove">
                        {arrayHelpers => (
                          <div className="content-section">
                            {content.map((picture, index) => (
                              <div key={index} 
                                  className="content-container"
                              >
                                <img src={CloudStore.getImageUrl(picture)} 
                                     alt={`Content ${index}`} 
                                />
                                <button type="button" 
                                        className={"remove-content-button"} 
                                        onClick={() => removePicture(arrayHelpers, index)}
                                >
                                  X
                                </button>
                                <Field name={`PicturesToRemove.${index}`} 
                                       type="hidden" 
                                       value={picture} 
                                />
                              </div>
                            ))}
                          </div>
                        )}
                      </FieldArray>

                      <button className="submit-button"
                              type="submit"
                              disabled={updating}
                      >
                        {updating ? "Updating..." : "Submit"}
                      </button>
                    </>
                  }                  
                </Form>
              )}
            </Formik>
          </div>
        </>
      }
    </>
  );
}