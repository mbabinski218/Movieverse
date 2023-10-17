import "./Button.css";

interface ButtonProps {
  className?: string | undefined;
  label?: string | undefined;
  primary?: boolean | undefined;
  type?: "button" | "submit" | undefined;
  disabled?: boolean | undefined;
  onClick?: () => void | undefined;
}

export const Button: React.FC<ButtonProps> = ({className, label, primary, type, disabled, onClick}) => {  
  return (
    <button className={(primary ? "primary-color " : "") + "button btn btn-block " + (className ?? "")} 
            type={type ?? "button"}
            disabled={disabled}
            onClick={onClick}
    >{label}</button>
  );
}