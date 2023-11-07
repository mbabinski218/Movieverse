import { useCallback } from "react";
import "./Button.css";

interface ButtonProps {
  className?: string;
  label?: string;
  imgSrc?: string;
  primary?: boolean;
  color?: "white" | "dark" | "gold"
  type?: "button" | "submit";
  disabled?: boolean;
  redirect?: string;
  onClick?: (() => void) | (() => Promise<void>);
}

export const Button: React.FC<ButtonProps> = ({className, label, imgSrc, primary, color, type, redirect, disabled, onClick}) => {  
  const colorPicker = useCallback(() => {
    switch (color) {
      case "white":
        return "#ffffff";
      case "dark":
        return "#2f2f2f";
      case "gold":
        return "#ffcb74";
      default:
        return primary ? "#ffcb74" : "#ffffff";
    }
  }, [color]);

  const fontColorPicker = useCallback(() => {
    switch (color) {
      case "dark":
        return "#ffffff";
      default:
        return "#2f2f2f";
    }
  }, [color]);
  
  return (
    <button className={(primary ? "primary-color " : "") + "button btn btn-block " + (className ?? "")} 
            type={type ?? "button"}
            style={{backgroundColor: colorPicker(), color: fontColorPicker()}}
            disabled={disabled}
            onClick={onClick}
    >
      <a className="button-link"
         href={redirect}
      />
      {
        imgSrc && <img className="button-img" src={imgSrc}/>
      }
      {label}
    </button>
  );
}