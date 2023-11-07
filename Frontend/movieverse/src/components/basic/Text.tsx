import "./Text.css"

export interface TextProps {
  label: string;
  text?: string;
  className?: string;
}

export const Text: React.FC<TextProps> = ({label, text, className}) => {
  return (
    <div className={`text ${className ? className : ""}`}>
      <div className="text-label">{label}</div>
      <div className="text-text">{text}</div>
    </div>
  )
}