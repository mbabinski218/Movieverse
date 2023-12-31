import "./Input.css";

interface InputProps {
  label?: string;
  value?: string;
  type?: "text" | "number" | "password";
  accept?: string;
  min?: number;
  max?: number;
  onClick?: () => void;
  onChange?: (e: React.ChangeEvent<HTMLInputElement>) => void;
  onSubmit?: (e: React.KeyboardEvent<HTMLInputElement>) => void;
  onKeyDown?: (e: React.KeyboardEvent<HTMLInputElement>) => void;
}

export const Input: React.FC<InputProps> = ({label, value, type, accept, min, max, onClick, onChange, onSubmit, onKeyDown}) => {
  return (
    <div className="input"> 
      <div className="input-bar">
        <input className="input-text"
               placeholder={label ?? ""} 
               value={value ?? ""} 
               type={type ?? "text"}   
               accept={accept ?? ""}
               min={min ?? 0}
               max={max ?? Number.MAX_VALUE}
               onClick={onClick} 
               onChange={onChange} 
               onSubmit={onSubmit}
               onKeyDown={onKeyDown}
        /> 
      </div>
    </div>
  )
};

interface InputFileProps {
  label?: string | undefined;
  fileName?: string | undefined;
  accept?: string | undefined;
  onChange?: (e: React.ChangeEvent<HTMLInputElement>) => void | undefined;
}

export const InputFile: React.FC<InputFileProps> = ({label, fileName, accept, onChange}) => {
  return (
    <div className="input"> 
      <div className="input-bar">
        <label htmlFor="file-tag">{label ?? ""}</label>
        <input className="file"
               type="file"
               accept={accept ?? ""}
               onChange={onChange}
               id="file-tag"
        />
        <span className="file-text">{fileName ?? ""}</span>
      </div>
    </div>
  )
};