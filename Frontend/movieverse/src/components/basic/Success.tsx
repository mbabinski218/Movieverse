import { useEffect, useState } from "react";
import "./Success.css";

interface SuccessProps {
  success: string[] | string;
} 

export const Success: React.FC<SuccessProps> = ({success}) => {
  const [successMessages, setSuccessMessages] = useState<string[]>([]);

  useEffect(() => {
    if (typeof success === "string") {
      setSuccessMessages([success]);
    } 
    else {
      setSuccessMessages(success);
    }
  }, [success]);

  return (
    <div className="success-container">
      {
        successMessages.map((s, index) => {
          return (
            <div key={index}>
              <span className="success-message">{s}</span>
              <hr className="success-line"/>
            </div>
          );
        })
      }
    </div>
  );
}