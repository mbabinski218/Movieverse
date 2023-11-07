import { useEffect, useState } from "react";
import { ContentDto } from "../core/dtos/content/contentDto";
import { Api } from "../Api";

export const useContent = (mediaId: string) => {
  const [content, setContent] = useState<ContentDto[] | null>(null);

  useEffect(() => {
    try {
      Api.getContentPaths(mediaId)
			  .then(res => {
          if(res.ok) {
            res.json()
              .then((data: ContentDto[]) => {
                setContent(data);
              })
          }
        })
		}
    catch {
      console.error("Error while fetching latest media");
      return;
    }
  }, []);

  return [content, setContent] as [typeof content, typeof setContent];
}