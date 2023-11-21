import { useCallback, useEffect, useState } from "react";
import { useReview } from "../../hooks/useReview";
import { Button } from "../basic/Button";
import { Input } from "../basic/Input";
import { Api } from "../../Api";
import { ReviewDto } from "../../core/dtos/media/reviewDto";
import "./Reviews.css"
import { useUserRoles } from "../../hooks/useUserRoles";
import { UserRoles } from "../../UserRoles";

export interface ReviewsProps {
  mediaId: string;
  className?: string;
}

const Reviews: React.FC<ReviewsProps> = ({mediaId, className}) => {
  const [reviews, setReviews] = useReview(mediaId);
  const [showReviews, setShowReviews] = useState<boolean>(false);
  const [text, setText] = useState<string>("");
  const [reload, setReload] = useState<boolean>(false);
  const [userRoles] = useUserRoles();

  useEffect(() => {
    if (reload) {
      Api.getReview(mediaId)
      .then(res => {
        if (res.ok) {
          return res.json();
        }
      })
      .then(data => data as ReviewDto[])
      .then(setReviews);
    }
  }, [reload]);

  const toggleReviews = useCallback(() => {
    setShowReviews(!showReviews);
  }, [showReviews]);

  const handleTextChange = useCallback((event: React.ChangeEvent<HTMLInputElement>) => {
    setText(event.target.value);
  }, []);

  const handleReviewSend = useCallback(() => {
    Api.addReview(mediaId, text)
      .then(res => {
        if (res.ok) {
          res.json()
            .then((review: ReviewDto) => {
              setText("");
      
              if (!reviews) {
                setReviews([review])
              }
              else {
                reviews.unshift(review);
              }
            });
        }
        else {
          alert("You are banned by administator.");
        }
      })
  }, [text, reviews]);

  const handleUserBan = useCallback((userId: string) => {
    Api.banUser(userId)
      .then(res => {
        if (res.ok) {
          setReload(true);
        }
        else {
          res.json().then(res => alert(res));
        }
    });
  }, []);

  return (
    <div className={`review-menu ${className ? className : ""}`}>
      <div className="review-input">
        <Input label="Add new comment" 
              value={text}
              onChange={handleTextChange}
        />
      </div>
      <Button className="review-send-button"
              label="Send"
              color="gold"
              disabled={text.length === 0}
              onClick={handleReviewSend}
      />
      <div className="review-show">
        {
          (reviews && reviews.length > 0) ?        
          <>
            <div>
              {
                showReviews &&
                reviews.map((review, index) => (
                  <div className="review-grid" key={index}>
                    <div className="review-name">{review.userName}</div>
                    <div className="review-date">{review.date.substring(0, 10)}</div>
                    <div className="review-text">{review.text}</div>
                    {
                      (userRoles?.includes(UserRoles.Administrator) || userRoles?.includes(UserRoles.SystemAdministrator)) &&
                      <Button className="review-util"
                              label="Ban user"
                              color="dark"
                              onClick={() => handleUserBan(review.userId)}
                      />
                    }
                    {
                      index !== reviews.length - 1 &&
                      <hr className="review-break" />
                    }
                  </div>
                ))
              }
            </div>
            <Button label={showReviews ? "Hide reviews" : "Show reviews"}
                    color="dark"
                    onClick={toggleReviews}
            />
          </> :
          <span className="staff-name">No reviews</span>
        }
      </div>
    </div>
  )
}

export default Reviews;