import { ChangePasswordContract } from "../../core/contracts/changePasswordContract";
import { Input } from "../basic/Input";
import { Button } from "../basic/Button";
import { useCallback, useState } from "react";
import { Api } from "../../Api";
import "./ChangePassword.css";

export interface ChangePasswordProps {
  onError: (error: string | string[]) => void;
  onSuccess: (success: string | string[]) => void;
}

export const ChangePassword: React.FC<ChangePasswordProps> = (props) => {
  const [passwordProps, setPasswordProps] = useState<ChangePasswordContract>({
    currentPassword: "",
    newPassword: "",
    confirmNewPassword: ""
  });

  const canUpdate = useCallback(() => {
    return passwordProps.currentPassword !== "" && passwordProps.newPassword !== "" && passwordProps.confirmNewPassword !== "";
  }, [passwordProps]);

  const handleCurrentPasswordChange = useCallback((e: React.ChangeEvent<HTMLInputElement>) => {
    setPasswordProps({...passwordProps, currentPassword: e.target.value});
  }, [passwordProps]);

  const handleNewPasswordChange = useCallback((e: React.ChangeEvent<HTMLInputElement>) => {
    setPasswordProps({...passwordProps, newPassword: e.target.value});
  }, [passwordProps]);

  const handleConfirmNewPasswordChange = useCallback((e: React.ChangeEvent<HTMLInputElement>) => {
    setPasswordProps({...passwordProps, confirmNewPassword: e.target.value});
  }, [passwordProps]);

  const handleSubmit = useCallback(() => {
    if (passwordProps.newPassword !== passwordProps.confirmNewPassword) {
      props.onError("Passwords do not match");
      return;
    }

    Api.changePassword(passwordProps)
      .then((res) => {
        if (res.ok) {
          setPasswordProps({
            currentPassword: "",
            newPassword: "",
            confirmNewPassword: ""
          });
          props.onSuccess("Password updated successfully");
        } else {
          res.json().then((data: string[]) => {
            console.log(data);
            props.onError(data[0]);
          });
        }
      });
      
  }, [passwordProps, props]);

  return (
    <form className="change-pass-menu">
      <Input label={`Current password`}
             type="password"
             onChange={handleCurrentPasswordChange} 
             value={passwordProps.currentPassword}
      />
      <Input label={`New password`}
             type="password"
             onChange={handleNewPasswordChange} 
             value={passwordProps.newPassword}
      />
      <Input label={`Confirm new password`}
             type="password"
             onChange={handleConfirmNewPasswordChange} 
             value={passwordProps.confirmNewPassword}
      />
      <Button className="change-pass-button"
              label="Update password"
              primary={true}
              disabled={!canUpdate()}
              onClick={handleSubmit}
      />
    </form>
  )
};