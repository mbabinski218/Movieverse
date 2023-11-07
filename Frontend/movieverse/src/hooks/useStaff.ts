import { useEffect, useState } from "react";
import { Api } from "../Api";
import { StaffDto } from "../core/dtos/staff/staffDto";

export const useStaff = (mediaId: string) => {
  const [staff, setStaff] = useState<StaffDto[] | null>(null);

  useEffect(() => {
    try {
      Api.getStaff(mediaId)
        .then(res => {
          if (res.ok) {
            return res.json();
          }
        })
        .then(data => data as StaffDto[])
        .then(setStaff);          
		}
    catch {
      return;
    }
  }, []);

  return [staff, setStaff] as [typeof staff, typeof setStaff];
}