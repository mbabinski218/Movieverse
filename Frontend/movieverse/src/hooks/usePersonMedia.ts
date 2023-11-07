import { useEffect, useState } from "react";
import { Api } from "../Api";
import { MediaSectionDto } from "../core/dtos/media/mediaSectionDto";

export const usePersonMedia = (personId: string) => {
  const [personMedia, setPersonMedia] = useState<MediaSectionDto[] | null>(null);

  useEffect(() => {
    try {
      Api.getPersonMedia(personId)
        .then(res => {
          if (res.ok) {
            return res.json();
          }
        })
        .then(data => data as MediaSectionDto[])
        .then(setPersonMedia);          
		}
    catch {
      return;
    }
  }, []);

  return [personMedia, setPersonMedia] as [typeof personMedia, typeof setPersonMedia];
}