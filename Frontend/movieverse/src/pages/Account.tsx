import { useCallback, useEffect, useState } from "react"
import { useNavigate } from "react-router-dom";
import { LocalStorage } from "../hooks/useLocalStorage";
import { UpdateUserContract } from "../core/contracts/updateUserContract";
import { StateProps, emptyAuthState } from "../common/stateProps";
import { Success } from "../components/basic/Success";
import { Error } from "../components/basic/Error";
import { Input, InputFile } from "../components/basic/Input";
import { Button } from "../components/basic/Button";
import { UserDto } from "../core/dtos/user/userDto";
import { Api } from "../Api";
import "./Account.css";

// Interfaces
interface AccountProps {
  email: string;
  username: string;
  age: string;
  firstName: string;
  lastName: string;
  avatar: File | null;
}

// Empty props
const emptyAccountProps: AccountProps = {
  email: "",
  username: "",
  age: "",
  firstName: "",
  lastName: "",
  avatar: null
};

export const Account: React.FC = () => {
  const [accountProps, setAccountProps] = useState<AccountProps>(emptyAccountProps);
  const [accountCurrentProps, setAccountCurrentProps] = useState<AccountProps>(emptyAccountProps);
  const [stateProps, setStateProps] = useState<StateProps>(emptyAuthState);
  const navigate = useNavigate();

  useEffect(() => {
    if (!LocalStorage.getAccessToken()) {
      navigate("/user", { replace: true });
    }

    document.title = "Account - Movieverse"

    // Get user data
    Api.getUserData()
      .then(res => res.json())
      .then(data => data as UserDto)
      .then(user => {
        const accountProps: AccountProps = {
          email: user.email,
          username: user.userName,
          age: user.information.age,
          firstName: user.information.firstName ?? "",
          lastName: user.information.lastName ?? "",
          avatar: new File([], "")
        };        
        setAccountCurrentProps(accountProps);
      })
  }, []);

  // Show error message
  const showError = (err: string | string[]) => {
    setStateProps({error: true, errorMessages: err, success: false, successMessages: []});
  };

  // Show success message
  const showSuccess = (msg: string | string[]) => {
    setStateProps({error: false, errorMessages: [], success: true, successMessages: msg});
  };

  // Handlers for input fields
  const handleEmailChange = useCallback((e: React.ChangeEvent<HTMLInputElement>) => {
    setAccountProps({...accountProps, email: e.target.value});
  }, [accountProps]);

  const handleUsernameChange = useCallback((e: React.ChangeEvent<HTMLInputElement>) => {
    setAccountProps({...accountProps, username: e.target.value});
  }, [accountProps]);

  const handleAgeChange = useCallback((e: React.ChangeEvent<HTMLInputElement>) => {
    setAccountProps({...accountProps, age: e.target.value});
  }, [accountProps]);

  const handleFirstNameChange = useCallback((e: React.ChangeEvent<HTMLInputElement>) => {
    setAccountProps({...accountProps, firstName: e.target.value});
  }, [accountProps]);

  const handleLastNameChange = useCallback((e: React.ChangeEvent<HTMLInputElement>) => {
    setAccountProps({...accountProps, lastName: e.target.value});
  }, [accountProps]);

  const handleFileChange = useCallback((e: React.ChangeEvent<HTMLInputElement>) => {
    const file = e.target.files?.[0];
    if (file) {
      setAccountProps({...accountProps, avatar: file});
    }
  }, []);

  // Prevent entering e, E, +, -, , and . in age field
  const onNotAllowedKeyDown = useCallback((e: React.KeyboardEvent<HTMLInputElement>) => {
    if (e.key === "e" || 
        e.key === "E" || 
        e.key === "+" || 
        e.key === "-" || 
        e.key === "," || 
        e.key === ".") {
      e.preventDefault();
    }
  }, []);

  // Can update
 const canUpdate = useCallback(() => {
  return accountProps.email !== "" ||
         accountProps.username !== "" ||
         accountProps.age !== "" ||
         accountProps.firstName !== "" ||
         accountProps.lastName !== "" ||
         accountProps.avatar !== null;
 }, [accountProps, accountCurrentProps]);

  // Update handler
  const handleSubmit = useCallback(() => {
    const data: UpdateUserContract = {
      email: accountProps.email ? accountProps.email : undefined,
      userName: accountProps.username ? accountProps.username : undefined,
      information: {
        age: accountProps.age ? parseInt(accountProps.age) : undefined,
        firstName: accountProps.firstName ? accountProps.firstName : undefined,
        lastName: accountProps.lastName ? accountProps.lastName : undefined
      },
      avatar: accountProps.avatar ? accountProps.avatar : undefined
    };

    Api.updateUserData(data)
      .then(res => {
        if (res.ok) {
          showSuccess("User data updated successfully.");
          setAccountCurrentProps({
            email: accountProps.email ? accountProps.email : accountCurrentProps.email,
            username: accountProps.username ? accountProps.username : accountCurrentProps.username,
            age: accountProps.age ? accountProps.age : accountCurrentProps.age,
            firstName: accountProps.firstName ? accountProps.firstName : accountCurrentProps.firstName,
            lastName: accountProps.lastName ? accountProps.lastName : accountCurrentProps.lastName,
            avatar: null
          });
          setAccountProps(emptyAccountProps)
        }
        else {
          res.json().then((errors: string[]) => {
            showError(errors);
          })
          .catch(() => {
            showError("Something went wrong. Please try again.");
          });
        }
      })
      .catch(() => {
        showError("Something went wrong. Please try again.");
      });
  }, [accountProps]);

  // Logout handler
  const handleLogout = useCallback(() => {
    Api.logout()
      .then(() => {
        LocalStorage.clear();
        navigate("/user");
        window.location.reload();
      })
      .catch(err => {
        console.log(err);
        showError("Something went wrong. Please try again.");
      });
  }, []);

  return (
    <div className="account-page">
      <div className="account-form">
        <Input label={`Email: ${accountCurrentProps.email}`}
               onChange={handleEmailChange} 
               value={accountProps.email}
        />
        <Input label={`Username: ${accountCurrentProps.username}`}
               onChange={handleUsernameChange} 
               value={accountProps.username}
        />
        <Input label={`Age: ${accountCurrentProps.age}`}
               onChange={handleAgeChange} 
               type="number" 
               min={1}
               max={200}
               onKeyDown={onNotAllowedKeyDown}
               value={accountProps.age}
        />
        <Input label={`First name: ${accountCurrentProps.firstName}`}
               onChange={handleFirstNameChange} 
               value={accountProps.firstName}
        />
        <Input label={`Last name: ${accountCurrentProps.lastName}`}
               onChange={handleLastNameChange} 
               value={accountProps.lastName}
        />
        <InputFile label="Upload profile picture..."
                   fileName={accountProps.avatar?.name}
                   accept="image/png, image/jpeg, image/jpg, image/svg"
                   onChange={handleFileChange}
        />
        <div className="account-space account-buttons">
          <Button label="Update data"
                  primary={true}
                  disabled={!canUpdate()} 
                  onClick={handleSubmit} 
          />
        </div>    
      </div>
      <div className="account-form account-logout">
        <div className="account-buttons">
          <Button label="Logout"
                  onClick={handleLogout}
          />
        </div>
      </div>
      <div className="account-error">
      {
        stateProps.error && 
        <Error errors={stateProps.errorMessages} />
      }
      </div>
      <div className="account-success">
      {
        stateProps.success && 
        <Success success={stateProps.successMessages} />
      }
      </div>
    </div>
  )
}