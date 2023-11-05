import { useEffect, useState } from "react";
import { Api } from "../Api";
import { PlatformInfoDto } from "../core/dtos/platform/platformInfoDto";

export const usePlatform = (mediaId: string) => {
  const [platform, setPlatform] = useState<PlatformInfoDto[] | null>(null);

  useEffect(() => {
    try {
      Api.getPlatform(mediaId)
        .then(res => {
          if (res.ok) {
            return res.json();
          }
        })
        .then(data => data as PlatformInfoDto[])
        .then(setPlatform);          
		}
    catch {
      return;
    }
  }, []);

  return [platform, setPlatform] as [typeof platform, typeof setPlatform];
}