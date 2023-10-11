import { useCallback, useRef, useState } from "react";
import { Input } from "../basic/Input";
import { Button } from "../basic/Button";
import { LinkButton } from "../basic/LinkButton";
import { Error } from "../basic/Error";
import { CredentialResponse, GoogleLogin } from "@react-oauth/google";
import { RegisterContract } from "../../core/contracts/registerContract";
import { LoginContract } from "../../core/contracts/loginContract";
import "./Authentication.css"
import { Api } from "../../Api";
import { useLocalStorage } from "../../hooks/useLocalStorage";

//TODO localStorage
// Interfaces
interface AuthenticationProps {
  email: string;
  password: string;
  rePassword: string;
  username: string;
  age: string;
  firstName: string;
  lastName: string;
}

interface StateProps {
  error: boolean;
  errorMessages: string[] | string;
  success: boolean;
  successMessages: string[] | string;
}

// Empty props
const emptyAuthProps: AuthenticationProps = {
  email: "",
  password: "",
  rePassword: "",
  username: "",
  age: "",
  firstName: "",
  lastName: ""
};

const emptyAuthState: StateProps = {
  error: false,
  errorMessages: [],
  success: false,
  successMessages: []
};

export const Authentication: React.FC = () => {
  // States
  const [registerMode, setRegisterMode] = useState<boolean>(false);
  const [authenticationProps, setAuthenticationProps] = useState<AuthenticationProps>(emptyAuthProps);
  const [stateProps, setStateProps] = useState<StateProps>(emptyAuthState);

  // Refs
  const googleLoginButton = useRef<HTMLDivElement>(null);
  
  // Change mode between sign in and register
  const changeMode = useCallback(() => {
    setRegisterMode(!registerMode);
    setAuthenticationProps(emptyAuthProps);
    setStateProps(emptyAuthState);    
  }, [registerMode, emptyAuthProps, emptyAuthState]);

  // Setters for input fields
  const handleEmailChange = useCallback((e: React.ChangeEvent<HTMLInputElement>) => {
    setAuthenticationProps({...authenticationProps, email: e.target.value});
  }, [authenticationProps]);

  const handlePasswordChange = useCallback((e: React.ChangeEvent<HTMLInputElement>) => {
    setAuthenticationProps({...authenticationProps, password: e.target.value});
  }, [authenticationProps]);

  const handleRePasswordChange = useCallback((e: React.ChangeEvent<HTMLInputElement>) => {
    setAuthenticationProps({...authenticationProps, rePassword: e.target.value});
  }, [authenticationProps]);

  const handleUsernameChange = useCallback((e: React.ChangeEvent<HTMLInputElement>) => {
    setAuthenticationProps({...authenticationProps, username: e.target.value});
  }, [authenticationProps]);

  const handleAgeChange = useCallback((e: React.ChangeEvent<HTMLInputElement>) => {
    setAuthenticationProps({...authenticationProps, age: e.target.value});
  }, [authenticationProps]);

  const handleFirstNameChange = useCallback((e: React.ChangeEvent<HTMLInputElement>) => {
    setAuthenticationProps({...authenticationProps, firstName: e.target.value});
  }, [authenticationProps]);

  const handleLastNameChange = useCallback((e: React.ChangeEvent<HTMLInputElement>) => {
    setAuthenticationProps({...authenticationProps, lastName: e.target.value});
  }, [authenticationProps]);

  // Show error message
  const showError = (err: string) => {
    setStateProps({error: true, errorMessages: err, success: false, successMessages: []});
  };

  // Show success message
  const showSuccess = (msg: string) => {
    setStateProps({error: false, errorMessages: [], success: true, successMessages: msg});
  };

  // Sign in
  const onSignIn = useCallback(() => {
    if (authenticationProps.email === "" || authenticationProps.password === "") {
      showError("Email and password cannot be empty");
      return;
    }

    const loginContract: LoginContract = {
      grantType: "Password",
      email: authenticationProps.email,
      password: authenticationProps.password,
      refreshToken: null,
      idToken: null
    };
    
    Api.login(loginContract)
      .then((res) => {
        if (res) {
          showSuccess("Login successful");
        }
        else {
          showError("Login failed");
        }
      })
      .catch((err) => {
        showError(err);
      });
  }, [authenticationProps]);

  // Register
  const onRegister = useCallback(() => {
    if (authenticationProps.email === "" || 
        authenticationProps.username === "" || 
        authenticationProps.password === "" || 
        authenticationProps. rePassword === "" || 
        authenticationProps.age === "") {
      showError("Set all required fields");
      return;
    }
      
    if (authenticationProps.password !== authenticationProps.rePassword) {
      showError("Passwords do not match");
      return;
    }

    const registerContract: RegisterContract = {
      email: authenticationProps.email,
      username: authenticationProps.username,
      age: parseInt(authenticationProps.age),
      firstName: authenticationProps.firstName,
      lastName: authenticationProps.lastName,
      password: authenticationProps.password,
      confirmPassword: authenticationProps.rePassword
    };

    Api.register(registerContract)
      .then((res) => {
        if (res) {
          showSuccess("Registration successful");          
          changeMode();
        }
        else {
          showError("Registration failed");
        }
      })
      .catch((err) => {
        showError(err);
      });
  }, [authenticationProps]);

  // Sign in or register on enter key down
  const onEnterDown = useCallback((e: React.KeyboardEvent<HTMLInputElement>) => {
    if (e.key !== "Enter") {
      return;
    }
    registerMode ? onRegister() : onSignIn();
  }, [registerMode]);

  // Prevent entering e, E, +, -, , and . in age field
  const onNotAllowedKeyDown = useCallback((e: React.KeyboardEvent<HTMLInputElement>) => {
    if (e.key === "e" || e.key === "E" || e.key === "+" || e.key === "-" || e.key === "," || e.key === ".") {
      e.preventDefault();
    }
  }, []);

  // Google login
  const onGoogleLoginSuccess = useCallback((response: CredentialResponse) => {
    if (!response?.credential) {
      showError("Google login failed");
      return;
    }

    const loginContract: LoginContract = {
      grantType: "Google",
      email: null,
      password: null,
      refreshToken: null,
      idToken: response.credential
    };

    Api.login(loginContract)
      .then((res) => {
        if (res) {
          showSuccess("Login successful");
        }
        else {
          showError("Login failed");
        }
      })
      .catch((err) => {
        showError(err);
      });
  }, []);

  // Google login error
  const onGoogleLoginError = useCallback(() => {
    showError("Google login failed");
  }, []);

  // html
  return (
    <>
      <div className="auth">
        <div>
          <Input label={registerMode ? "*Email" : "Email"} 
                 onChange={handleEmailChange} 
                 value={authenticationProps.email}
          />
          {
            registerMode &&
            <>
              <Input label="*Username" 
                     onChange={handleUsernameChange} 
                     value={authenticationProps.username}
              />
              <Input label="*Age" onChange={handleAgeChange} 
                     value={authenticationProps.age} 
                     type="number" 
                     onKeyDown={onNotAllowedKeyDown}
              />
              <Input label="First name" 
                     onChange={handleFirstNameChange} 
                     value={authenticationProps.firstName} 
              />
              <Input label="Last name" 
                     onChange={handleLastNameChange} 
                     value={authenticationProps.lastName} 
              />
            </>
          }
          <Input label={registerMode ? "*Password" : "Password"} 
                 onChange={handlePasswordChange} 
                 onKeyDown={onEnterDown} 
                 value={authenticationProps.password}
            />
          {
            registerMode &&
            <Input label="*Re-enter password" 
                   onChange={handleRePasswordChange} 
                   onKeyDown={onEnterDown} 
                   value={authenticationProps.rePassword}
            />
          }
        </div>
        <div className="auth-buttons">
          <Button label={registerMode ? "Register" : "Sign in"} 
                  primary={true} 
                  onClick={registerMode ? onRegister : onSignIn}
          />
          <LinkButton label={registerMode ? "Already have an account? Sign in" : "Create a new account"} 
                      onClick={changeMode} 
          />
        </div>
        <div className="auth-external">
          <div className="auth-google-theme"    
               ref={googleLoginButton}
          >                  
            <GoogleLogin onSuccess={onGoogleLoginSuccess} 
                         onError={onGoogleLoginError} 
                         locale="en-US" 
                         width={googleLoginButton.current?.offsetWidth ?? 0} 
            />
          </div>
        </div>
      </div>
      <div className="auth-error">
      {
        stateProps.error &&
        <Error errors={stateProps.errorMessages} />
      }
      </div>
      <div className="auth-success">
      {
        stateProps.success &&
        <Error errors={stateProps.successMessages} />
      }
      </div>
    </>
  );
};