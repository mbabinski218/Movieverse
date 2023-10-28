import "./Button.css";

interface ButtonProps {
  className?: string;
  label?: string;
  primary?: boolean;
  type?: "button" | "submit";
  disabled?: boolean;
  onClick?: (() => void) | (() => Promise<void>);
}

export const Button: React.FC<ButtonProps> = ({className, label, primary, type, disabled, onClick}) => {  
  return (
    <button className={(primary ? "primary-color " : "") + "button btn btn-block " + (className ?? "")} 
            type={type ?? "button"}
            disabled={disabled}
            onClick={onClick}
    >
      {label}
    </button>
  );
}