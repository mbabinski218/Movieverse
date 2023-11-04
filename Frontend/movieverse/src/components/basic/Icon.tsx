import { SyntheticEvent, useCallback } from "react";
import "./Icon.css";
import Blank from "../../assets/blank.png";

export interface IconProps {
  label?: string;
  src?: string;
  text?: string;
  className?: string;
  onClick?: () => void;
}

export const Icon: React.FC<IconProps> = (props) => {
  const onError = useCallback((e: SyntheticEvent<HTMLImageElement, Event>): void => {
		const img = e.target as HTMLImageElement;
		img.src = Blank;
	}, []);

  return (
    <div className={props.className ?? ""}>
      {
        props.src && props.text ?
        <div className={props.onClick ? "icon-comp-img icon-clickable" : "icon-comp-img"}
             onClick={props.onClick}
        >
          <span className="icon-label">{props.label ?? ""}</span>
          <img className="icon-img"
               src={props.src}
               alt={props.label ?? ""}
               onError={onError}
          />
          <span className="icon-text">{props.text ?? ""}</span>
        </div> :        
        <>
          {
            props.src ?
            <div className={props.onClick ? "icon-comp-only-img icon-clickable" : "icon-comp-only-img"}
                 onClick={props.onClick}
            >
              <span className="icon-label">{props.label ?? ""}</span>
              <img className="icon-img-center"
                   src={props.src}
                   style={{scale: "1.4"}}
                   alt={props.label ?? ""}
                   onError={onError}
            />
            </div> :
            <div className={props.onClick ? "icon-comp icon-clickable" : "icon-comp"}
                 onClick={props.onClick}
            >
              <span className="icon-label">{props.label ?? ""}</span>
              <span className="icon-text">{props.text ?? ""}</span>
            </div>
          }
        </>        
      }
    </div>
  );
}