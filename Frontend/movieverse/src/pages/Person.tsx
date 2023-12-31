import React, { Suspense, SyntheticEvent, useCallback, useEffect, useState } from "react";
import { useParams } from "react-router-dom";
import { usePerson } from "../hooks/usePerson";
import { Loading } from "../components/basic/Loading";
import { NotFound } from "../components/basic/NotFound";
import { CloudStore } from "../CloudStore";
import { Section } from "../components/basic/Section";
import { Text } from "../components/basic/Text";
import { Button } from "../components/basic/Button";
import { useUserToken } from "../hooks/useUserToken";
import { UserRoles } from "../UserRoles";
import { EditPerson } from "../components/person/EditPerson";
import "./Person.css";
import Blank from "../assets/blank.png";
import Pen from "../assets/pen.svg";

// Dynamic imports
const LazyPersonMedia = React.lazy(() => import("../components/person/PersonMedia"));

export const Person: React.FC = () => {
  const params = useParams();
  const [loading, setLoading] = useState<boolean>(true);
  const [person] = usePerson(params.id ?? "");
  const [imgSrc, setImgSrc] = useState<string>("");
  const [moreContent, setMoreContent] = useState<boolean>(false);
  const [token] = useUserToken();
  const [editMode, setEditMode] = useState<boolean>(false);

  // On person change
  useEffect(() => {
    if (person) {
      document.title = `${getFullName()} - Movieverse`;
      person.pictureId ? setImgSrc(CloudStore.getImageUrl(person.pictureId as string)) : setImgSrc(Blank);
      setLoading(false);
    }
    else {
      if (!loading) {
        document.title = "Not found - Movieverse";
      }
    }
  }, [person]);

  // Error handling
  const onImgError = useCallback((e: SyntheticEvent<HTMLImageElement, Event>): void => {
    const img = e.target as HTMLImageElement;
    img.src = Blank;
  }, []);

  // Get full name
  const getFullName = useCallback(() => {
    if (person?.information?.firstName && person?.information?.lastName) {
      return `${person.information.firstName} ${person.information.lastName}`;
    }
    else {
      return "Person";
    }
  }, [person]);

  // Toggle more content
  const toggleMoreContent = useCallback(() => {
    setMoreContent(!moreContent);
  }, [moreContent]);

  //Toggle edit mode
  const toggleEditMode = useCallback(() => {
    setEditMode(!editMode);
  }, [editMode]);

  return (
    <>      
      {
        loading ? <Loading /> :
        !person ? <NotFound /> : 
        <div className="person-page">
          {
            (token?.role.includes(UserRoles.Administrator) || token?.role.includes(UserRoles.SystemAdministrator) || token?.personId === params.id) &&
            <>
              <img className="person-pen"
                  src={Pen} 
                  alt="pen"
                  onClick={toggleEditMode}
              />              
              {
                editMode &&
                <EditPerson personId={params.id as string}
                            onClose={toggleEditMode}     
                />
              }              
            </>
          }
          <span className="person-title">{getFullName()}</span>
          <img className="person-poster"
               src={imgSrc}
               onError={onImgError}
               alt={getFullName()}
          />
          <div className="person-description">            
            <Section title="Biography">
              <span>{person.biography ?? "No data."}</span>
            </Section>
          </div>
          <div className="person-funfacts">            
            <Section title="Fun facts">
              <span>{person.funFacts ?? "No data."}</span>
            </Section>
          </div>
          <div className="person-life">
            <Section title="Life history">
              <Text label={`Birth date`} text={person.lifeHistory.birthDate?.substring(0, 10) ?? "unknown"} />
              <hr className="person-break" />
              <Text label={`Birth place`} text={person.lifeHistory.birthPlace ?? "unknown"} />
              {
                person.lifeHistory.deathDate &&
                <>
                  <hr className="person-break" />
                  <Text label={`Death date`} text={person.lifeHistory.deathDate.substring(0, 10)} />
                </>
              }
              {
                person.lifeHistory.deathPlace &&
                <>
                  <hr className="person-break" />
                  <Text label={`Death place`} text={person.lifeHistory.deathPlace} />
                </>
              }
              <hr className="person-break" />
              <Text label={`Career start`} text={person.lifeHistory.careerStart?.substring(0, 10) ?? "unknown"} />
              {
                person.lifeHistory.careerEnd &&
                <>
                  <hr className="person-break" />
                  <Text label={`Career end`} text={person.lifeHistory.careerEnd.substring(0, 10)} />
                </>
              }
              <hr className="person-break" />
            </Section>
          </div>
          <div className="person-pic">
            <Section title="Pictures">
              <div className="person-content">
              {
                person.contentIds.length > 0 ?
                <>
                  {
                    person.contentIds.map((id, index) => {
                      if (index < (moreContent ? person.contentIds.length : 4)) {
                        return (
                          <img className="person-img"
                              key={index}
                              src={CloudStore.getImageUrl(id)}
                              alt={getFullName()}
                          />
                        )
                      }
                    }) 
                  }                  
                  <Button className="person-more-btn"
                          label={moreContent ? "Show less" : "Show more"}
                          color="dark"
                          onClick={toggleMoreContent}
                  />                  
                </> :
                <span className="person-name">No data</span>
              }
              </div>
            </Section>
          </div>
          <div className="person-media">
            <Section title="Known for">
              <Suspense fallback={<Loading />}>
                <LazyPersonMedia personId={params.id as string}/>
              </Suspense>
            </Section>
          </div>
        </div>
      }
    </>
  )
}