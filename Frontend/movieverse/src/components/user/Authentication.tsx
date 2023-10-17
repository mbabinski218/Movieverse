import { useCallback, useEffect, useRef, useState } from "react";
import { Input } from "../basic/Input";
import { Button } from "../basic/Button";
import { LinkButton } from "../basic/LinkButton";
import { Error } from "../basic/Error";
import { CredentialResponse, GoogleLogin } from "@react-oauth/google";
import { RegisterContract } from "../../core/contracts/registerContract";
import { LoginContract } from "../../core/contracts/loginContract";
import { Api } from "../../Api";
import { LocalStorage, useLocalStorage } from "../../hooks/useLocalStorage";
import { Success } from "../basic/Success";
import { TokensDto } from "../../core/dtos/user/tokensDto";
import { useNavigate } from "react-router-dom";
import { StateProps, emptyAuthState } from "../../common/stateProps";
import "./Authentication.css"

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

export const Authentication: React.FC = () => {
  // Local storage
  const [accessToken, setAccessToken] = useLocalStorage<string | null>(LocalStorage.accessTokenKey, LocalStorage.getAccessToken());
  const [refreshToken, setRefreshToken] = useLocalStorage<string | null>(LocalStorage.refreshTokenKey, LocalStorage.getRefreshToken());

  // States
  const [registerMode, setRegisterMode] = useState<boolean>(false);
  const [authenticationProps, setAuthenticationProps] = useState<AuthenticationProps>(emptyAuthProps);
  const [stateProps, setStateProps] = useState<StateProps>(emptyAuthState);

  // Refs
  const googleLoginButton = useRef<HTMLDivElement>(null);

  // Navigate
  const navigate = useNavigate();

  useEffect(() => {
    if (accessToken) {
      navigate(-1);
    }
  }, []);
  
  // Change mode between sign in and register
  const changeMode = useCallback(() => {
    setRegisterMode(!registerMode);
    setAuthenticationProps(emptyAuthProps);
    setStateProps(emptyAuthState);    
  }, [registerMode, emptyAuthProps, emptyAuthState]);

  // Handlers for input fields
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
  const showError = (err: string | string[]) => {
    setStateProps({error: true, errorMessages: err, success: false, successMessages: []});
  };

  // Show success message
  const showSuccess = (msg: string | string[]) => {
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
      password: authenticationProps.password
    };
    
    Api.login(loginContract)
      .then((res: Response) => {
        if (res.ok) {                    
          res.json().then((tokens: TokensDto) => {
            setAccessToken(tokens.accessToken);
            setRefreshToken(tokens.refreshToken);
            navigate(-1);
          })
          .catch(() => {
            showError("Login failed.");
          });
        }
        else {
          res.json().then((errors: string[]) => {
            showError(errors);
          })
          .catch(() => {
            showError("Login failed.");
          });
        }     
    });
  }, [authenticationProps]);

  // Register
  const onRegister = useCallback(() => {
    if (authenticationProps.email === "" || 
        authenticationProps.username === "" || 
        authenticationProps.password === "" || 
        authenticationProps.rePassword === "" || 
        authenticationProps.age === "") {
      showError("Set all required fields.");
      return;
    }
      
    if (authenticationProps.password !== authenticationProps.rePassword) {
      showError("Passwords do not match.");
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
      .then((res: Response) => {
        if (res.ok) {    
            changeMode();                
            showSuccess("Registration successful. Check your email for confirmation.");
        }
        else {
          res.json().then((errors: string[]) => {
            showError(errors);
          })
          .catch(() => {
            showError("Registration failed.");
          });
        }     
    });
  }, [authenticationProps]);

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

  // Google login
  const onGoogleLoginSuccess = useCallback((response: CredentialResponse) => {
    if (!response?.credential) {
      showError("Google login failed");
      return;
    }

    const loginContract: LoginContract = {
      grantType: "Google",
      idToken: response.credential
    };

    Api.login(loginContract)
      .then((res: Response) => {
        if (res.ok) {                    
          res.json().then((tokens: TokensDto) => {
            setAccessToken(tokens.accessToken);
            setRefreshToken(tokens.refreshToken);
            navigate(-1);
          })
          .catch(() => {
            showError("Google login failed");
          });
        }
        else {
          res.json().then((errors: string[]) => {
            showError(errors);
          })
          .catch(() => {
            showError("Google login failed");
          });
        }     
    });
  }, []);

  // Google login error
  const onGoogleLoginError = useCallback(() => {
    showError("Google login failed");
  }, []);

  // html
  if (accessToken) {
    return (
      <div className="auth">
          <Success success="Successfully logged in." />
      </div>
    );
  }
  else {
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
                       min={1}
                       max={200} 
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
                   type="password"
                   value={authenticationProps.password}
              />
            {
              registerMode &&
              <Input label="*Re-enter password" 
                     onChange={handleRePasswordChange}
                     type="password"
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
          <Success success={stateProps.successMessages} />
        }
        </div>
      </>
    );
  }
};