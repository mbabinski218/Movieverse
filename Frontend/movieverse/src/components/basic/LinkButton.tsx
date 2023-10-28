import "./LinkButton.css"

interface LinkButtonProps {
  label?: string;
  onClick?: () => void;
}

export const LinkButton: React.FC<LinkButtonProps> = ({label, onClick}) => {
  return (
    <span className="link-button" onClick={onClick}>{label}</span>
  )
}