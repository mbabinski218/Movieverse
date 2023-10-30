import { useEffect, useState } from "react";
import { Api } from "../Api";
import { MediaInfoDto } from "../core/dtos/user/mediaInfoDto";

export const useUserMediaInfo = (mediaId: string) => {
  const [mediaInfo, setMediaInfo] = useState<MediaInfoDto | null>(null);

  useEffect(() => {
    try {
      Api.getMediaInfo(mediaId)
        .then(res => {
          if (res.ok) {
            return res.json();
          }
        })
        .then(data => data as MediaInfoDto)
        .then(setMediaInfo);
		}
    catch {
      return;
    }
  }, []);

  return [mediaInfo, setMediaInfo] as [typeof mediaInfo, typeof setMediaInfo];
}