import "./Button.css";

interface ButtonProps {
  label?: string | undefined;
  primary?: boolean | undefined;
  onClick?: () => void | undefined;
}

export const Button: React.FC<ButtonProps> = ({label, primary, onClick}) => {  
  return (
    <button className={(primary ? "primary-color " : "") + "button btn btn-block"} 
            onClick={onClick}
    >{label}</button>
  );
}