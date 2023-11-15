import { useEffect, useState } from "react";
import { Api } from "../Api";
import { PlatformDto } from "../core/dtos/platform/platformDto";

export const usePlatforms = () => {
  const [platforms, setPlatforms] = useState<PlatformDto[] | null>(null);

  useEffect(() => {
    try {
      Api.getPlatforms()
        .then(res => {
          if (res.ok) {
            return res.json();
          }
        })
        .then(data => data as PlatformDto[])
        .then(setPlatforms);          
		}
    catch {
      return;
    }
  }, []);

  return [platforms, setPlatforms] as [typeof platforms, typeof setPlatforms];
}