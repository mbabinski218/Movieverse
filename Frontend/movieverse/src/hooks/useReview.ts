import { useEffect, useState } from "react";
import { Api } from "../Api";
import { ReviewDto } from "../core/dtos/media/reviewDto";

export const useReview = (mediaId: string) => {
  const [review, setReview] = useState<ReviewDto[] | null>(null);

  useEffect(() => {
    try {
      Api.getReview(mediaId)
        .then(res => {
          if (res.ok) {
            return res.json();
          }
        })
        .then(data => data as ReviewDto[])
        .then(setReview);          
		}
    catch {
      return;
    }
  }, []);

  return [review, setReview] as [typeof review, typeof setReview];
}