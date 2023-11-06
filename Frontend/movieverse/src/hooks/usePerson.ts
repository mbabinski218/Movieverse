import { useEffect, useState } from "react";
import { Api } from "../Api";
import { PersonDto } from "../core/dtos/person/personDto";

export const usePerson = (personId: string) => {
  const [person, setPerson] = useState<PersonDto | null>(null);

  useEffect(() => {
    try {
      Api.getPerson(personId)
        .then(res => {
          if (res.ok) {
            return res.json();
          }
        })
        .then(data => data as PersonDto)
        .then(setPerson);          
		}
    catch {
      return;
    }
  }, []);

  return [person, setPerson] as [typeof person, typeof setPerson];
}