import React, { Suspense, SyntheticEvent, useCallback, useEffect, useState } from "react";
import { useParams } from "react-router-dom";
import { usePerson } from "../hooks/usePerson";
import { Loading } from "../components/basic/Loading";
import { NotFound } from "../components/basic/NotFound";
import { CloudStore } from "../CloudStore";
import { Section } from "../components/basic/Section";
import { Text } from "../components/basic/Text";
import "./Person.css";
import Blank from "../assets/blank.png";

// Dynamic imports
const LazyPersonMedia = React.lazy(() => import("../components/person/PersonMedia"));

export const Person: React.FC = () => {
  const params = useParams();
  const [loading, setLoading] = useState<boolean>(true);
  const [person] = usePerson(params.id ?? "");
  const [imgSrc, setImgSrc] = useState<string>("");

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

  return (
    <>      
      {
        loading ? <Loading /> :
        !person ? <NotFound /> : 
        <div className="person-page">
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
              <hr className="person-break" />
              <Text label={`Death date`} text={person.lifeHistory.deathDate?.substring(0, 10) ?? "unknown"} />
              <hr className="person-break" />
              <Text label={`Death place`} text={person.lifeHistory.deathPlace ?? "unknown"} />
              <hr className="person-break" />
              <Text label={`Career start`} text={person.lifeHistory.careerStart?.substring(0, 10) ?? "unknown"} />
              <hr className="person-break" />
              <Text label={`Career end`} text={person.lifeHistory.careerEnd?.substring(0, 10) ?? "unknown"} />
              <hr className="person-break" />
            </Section>
          </div>
          <div className="person-pic">
            <Section title="Pictures">

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