import "./Checkbox.css";

export interface CheckboxProps {
  label: string;
  value?: string;
  checked?: boolean;
  onChange?: (e: React.ChangeEvent<HTMLInputElement>) => void;
}

export const Checkbox: React.FC<CheckboxProps> = ({label, value, checked, onChange}) => {
  return (
    <div className="checkbox-main">
        <input className="checkbox-input"
               type="checkbox" 
               id={label} 
               name={label} 
               value={value ?? label} 
               onChange={onChange}
               checked={checked}
        />
        <label htmlFor={label}>{label}</label>
        <br />
    </div>
  )
}