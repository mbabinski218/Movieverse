import { useEffect, useState } from "react";
import "./Error.css";

interface ErrorProps {
  errors: string[] | string;
} 

export const Error: React.FC<ErrorProps> = ({errors}) => {
  const [errorMessages, setErrorMessages] = useState<string[]>([]);

  useEffect(() => {
    try {
      if (typeof errors === "string") {
        setErrorMessages([errors]);
      } 
      else {
        setErrorMessages(errors);
      }
    }
    catch (err) {
      setErrorMessages(["Fatal error. Try again later."]);
    }
  }, [errors]);

  return (
    <div className="error-container">
      {
        errorMessages.map((err, index) => {
          return (
            <div key={index}>
              <span className="error-message">{err}</span>
              <hr className="error-line"/>
            </div>
          );
        })
      }
    </div>
  );
}