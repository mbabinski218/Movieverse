import { useCallback, useState } from "react";
import { Api } from "../../Api";
import { Button } from "../basic/Button";
import { StateProps, emptyState } from "../../common/stateProps";
import { Success } from "../basic/Success";
import { Error } from "../basic/Error";
import { useNavigate } from "react-router-dom";
import { PageBlur } from "../basic/PageBlur";
import "./AddPersonMenu.css";
import Close from "../../assets/bars-close.svg";

export interface AddPersonMenuProps {
  forUser: boolean;
  onClose?: () => void;
  onSuccessfulAdd?: () => void;
}

export const AddPersonMenu: React.FC<AddPersonMenuProps> = (props) => {
  const [stateProps, setStateProps] = useState<StateProps>(emptyState);
  const navigate = useNavigate();

  const showError = (err: string | string[]) => {
    setStateProps({error: true, errorMessages: err, success: false, successMessages: []});
  };

  const showSuccess = (msg: string | string[]) => {
    setStateProps({error: false, errorMessages: [], success: true, successMessages: msg});
  };

  const handleAddButtonClick = useCallback(() => {
    Api.addPerson(props.forUser)
      .then((res) => {
        if (res.ok) {
          res.text().then((id: string) => {
            showSuccess("Person added successfully!");
            if (props.onSuccessfulAdd) {
              props.onSuccessfulAdd();
            }

            if (props.forUser) {
              Api.refreshTokens();
            }

            navigate(`/person/${id}`);
          })
        }
        else {
          res.json().then((errors: string[]) => {
            showError(errors);
          })
          .catch(() => {
            showError("Adding failed.");
          });
        }
      })
      .catch((err) => {
        showError(err);
      });
  }, []);

  return (
    <>
      <PageBlur />
      <div className="person-window">
        <div className="person-add-menu">
          <span className="person-add-title">Add new person</span>
          <img src={Close} onClick={props.onClose} alt="close" className="person-add-close" />
          <Button className="person-button"
                  label="Add"              
                  primary={true}
                  onClick={handleAddButtonClick}
          />
        </div>
        <div className="person-error">
          {
            stateProps.error &&
            <Error errors={stateProps.errorMessages} />
          }
        </div>
        <div className="person-success">
          {
            stateProps.success &&
            <Success success={stateProps.successMessages} />
          }
        </div>
      </div>
    </>
  );
}