import { PageBlur } from "../basic/PageBlur";
import { useCallback, useState } from "react";
import Rating from "@mui/material/Rating/Rating";
import "./RateMediaPopup.css";
import Close from "../../assets/bars-close.svg";

export interface RateMediaPopupProps {
  mediaId: string;
  rating?: number;
  onClose?: () => void;
  onRatingChange?: ((event: React.SyntheticEvent<Element, Event>, value: number | null) => void);
}

export const RateMediaPopup: React.FC<RateMediaPopupProps> = (props) => {
  const [rating, setRating] = useState<number>(props.rating ?? 0);
  const [hover, setHover] = useState<number>(-1);

  const onChangeActive = useCallback((event: React.SyntheticEvent<Element, Event>, value: number) => {
    setHover(value);
  }, []);

  const onChange = useCallback((event: React.SyntheticEvent<Element, Event>, value: number | null) => {
    setRating(value ?? 0);
    if (props.onRatingChange) {
      props.onRatingChange(event, value);
    }
    if (props.onClose) {
      props.onClose();
    }
  }, []);

  return (
    <>
      <PageBlur />
      <div className="rate-window">
        <div className="rate-menu">
          <span className="rate-title">Rate</span>
          <img className="rate-close" 
               src={Close} 
               onClick={props.onClose} 
               alt="close" 
          />
          <Rating className="rate-stars"
                  value={rating}
                  defaultValue={0} 
                  max={10} 
                  size="large"
                  onChangeActive={onChangeActive}
                  onChange={onChange}                  
          />
          <span className="rate-value">{`${hover !== -1 ? hover : rating}/10`}</span>
        </div>
      </div>
    </>
  )
}