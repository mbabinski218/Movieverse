
import "./Section.css";
import SectionBar from "../../assets/section.svg";

export interface SectionProps {
  title?: string | null;
  children?: React.ReactNode
}

export const Section: React.FC<SectionProps> = ({title, children}) => {

  return (
    <>
      <div className="section-bar">
        <img className="section-img" 
              src={SectionBar} 
              style={{rotate:"90deg"}}
              alt="Section bar"
        />
        <span className="section-title">{title}</span>
      </div>
      {children}
    </>
  )
}