import "./Text.css"

export interface TextProps {
  label: string;
  text: string;
}

export const Text: React.FC<TextProps> = ({label, text}) => {
  return (
    <div className="text">
      <div className="text-label">{label}</div>
      <div className="text-text">{text}</div>
    </div>
  )
}