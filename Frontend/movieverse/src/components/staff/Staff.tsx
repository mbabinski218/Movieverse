import { useStaff } from "../../hooks/useStaff";
import { CloudStore } from "../../CloudStore";
import "./Staff.css";
import Blank from "../../assets/blank.png";

export interface StaffProps {
  mediaId: string;
  className?: string;
}

const Staff: React.FC<StaffProps> = ({mediaId, className}) => {
  const [staff] = useStaff(mediaId);

  return (
    <div className={`staff-menu ${className ? className : ""}`}>
      {
        (staff && staff.length > 0) ?
        staff.map((person, index) => (
          <a className="staff-item"
             key={index}
             href={person.personId ? `/person/${person.personId}` : ""}
          >
            <img className={person.pictureId ? "staff-img" : "staff-no-img"}
                 src={person.pictureId ? CloudStore.getImageUrl(person.pictureId) : Blank}
                 alt={person.firstName}
            />
            <span className="staff-name">{`${person.firstName} ${person.lastName}`}</span>
            <span className="staff-role">{person.role}</span>
          </a>
        )) :
        <span className="staff-name">No data</span>
      }
    </div>
  )
}

export default Staff;