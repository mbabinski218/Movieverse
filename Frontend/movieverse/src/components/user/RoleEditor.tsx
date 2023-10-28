import { useCallback, useState } from "react";
import { PageBlur } from "../basic/PageBlur";
import { Input } from "../basic/Input";
import { Button } from "../basic/Button";
import { Error } from "../basic/Error";
import { Success } from "../basic/Success";
import { StateProps, emptyState } from "../../common/stateProps";
import { Api } from "../../Api";
import "./RoleEditor.css"
import Close from "../../assets/bars-close.svg";
import { UpdateRolesContract } from "../../core/contracts/updateRolesContract";
import { Checkbox } from "../basic/Checkbox";
import { UserRoles } from "../../UserRoles";

export interface RoleEditorProps {
  onClose?: () => void;
}

export const RoleEditor: React.FC<RoleEditorProps> = (props) => {
  const [email, setEmail] = useState<string | null>(null);
  const [roles, setRoles] = useState<string[]>([]);
  const [stateProps, setStateProps] = useState<StateProps>(emptyState);

  const handleEmailChange = useCallback((e: React.ChangeEvent<HTMLInputElement>) => {
    setEmail(e.target.value);
  }, []);

  const handleCheckboxChange = useCallback((e: React.ChangeEvent<HTMLInputElement>) => {
    const value = e.target.value;
    const checked = e.target.checked;
    console.log(value, checked);
    if (checked) {
      setRoles([...roles, value]);
    } 
    else {      
      setRoles(roles.filter((role) => role !== value));
    }
  }, [roles]);

  const handleUpdateRolesButtonClick = useCallback(() => {
    if (!email?.isValid()) {
      showError("Email is required!");
      return;
    }

    if (!email.isValidEmailFormat()) {
      showError("Wrong email format!");
      return;
    }

    const body: UpdateRolesContract = {
      email: email,
      roles: roles
    }

    Api.updateRoles(body)
      .then((res) => {
        if (res.ok) {
          setEmail(null);
          showSuccess("Roles updated successfully!");
        } else {
          res.json()
          .then((err) => showError(err))
          .catch(() => showError("Something went wrong! Try again later."))
        }
      })
      .catch(() => {
        showError("Something went wrong! Try again later.");
      });
  }, [email, roles]);

  const showError = (err: string | string[]) => {
    setStateProps({error: true, errorMessages: err, success: false, successMessages: []});
  };

  const showSuccess = (msg: string | string[]) => {
    setStateProps({error: false, errorMessages: [], success: true, successMessages: msg});
  };

  return (
    <>
      <PageBlur />
      <div className="roleEditor-window">
        <div className="roleEditor-menu">
          <span className="roleEditor-title">Role editor</span>
          <img src={Close} onClick={props.onClose} alt="close" className="roleEditor-close" />
          <div className="roleEditor-inputs">
            <Input label="User email"
                   value={email ?? ""}
                   onChange={handleEmailChange}
            />
            
          </div>
          <div className="roleEditor-checkboxes">
            <Checkbox label={UserRoles.Administrator} 
                      checked={roles.includes(UserRoles.Administrator)}
                      onChange={handleCheckboxChange}
            />
            <Checkbox label={UserRoles.Critic} 
                      checked={roles.includes(UserRoles.Critic)}
                      onChange={handleCheckboxChange}
            />
            <Checkbox label={UserRoles.Pro} 
                      checked={roles.includes(UserRoles.Pro)}
                      onChange={handleCheckboxChange}
            />
          </div>
          <Button className="roleEditor-button"
                  label="Update roles"              
                  primary={true}
                  disabled={!email?.isValid()}
                  onClick={handleUpdateRolesButtonClick}
          />
        </div>
        <div className="roleEditor-error">
          {
            stateProps.error &&
            <Error errors={stateProps.errorMessages} />
          }
        </div>
        <div className="roleEditor-success">
          {
            stateProps.success &&
            <Success success={stateProps.successMessages} />
          }
        </div>
      </div>
    </>
  )
}