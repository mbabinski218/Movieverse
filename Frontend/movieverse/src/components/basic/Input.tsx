import "./Input.css";

interface InputProps {
  label?: string | undefined;
  value?: string | undefined;
  type?: string | undefined;
  onClick?: () => void | undefined;
  onChange?: (e: React.ChangeEvent<HTMLInputElement>) => void | undefined;
  onKeyDown?: (e: React.KeyboardEvent<HTMLInputElement>) => void | undefined;
}

export const Input: React.FC<InputProps> = ({label, value, type, onClick, onChange, onKeyDown}) => {
  return (
    <div className="input" id="input"> 
      <div className="input-bar">
        <input className="input-text" 
               placeholder={label ?? ""} 
               value={value ?? ""} 
               type={type ?? "text"}   
               onClick={onClick} 
               onChange={onChange} 
               onKeyDown={onKeyDown} 
        /> 
      </div>
    </div>
  )
};