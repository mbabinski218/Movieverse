import { useCallback, useEffect, useState } from "react"
import { PayPalScriptProvider, PayPalButtons, ReactPayPalScriptOptions } from "@paypal/react-paypal-js";
import { CreateSubscriptionActions, OnApproveData, OnApproveActions, OnCancelledActions } from "@paypal/paypal-js/types/components/buttons";
import { StateProps, emptyState } from "../common/stateProps";
import { LocalStorage } from "../hooks/useLocalStorage";
import { useNavigate } from "react-router-dom";
import { Success } from "../components/basic/Success";
import { Error } from "../components/basic/Error";
import "./Pro.css"

export const Pro: React.FC = () => {
  const [stateProps, setStateProps] = useState<StateProps>(emptyState);
  const navigate = useNavigate();

  const initialOptions: ReactPayPalScriptOptions = {
    clientId: "test",
    currency: "USD",
    enableFunding: "paypal",
    disableFunding: "paylater",
    dataSdkIntegrationSource: "integrationbuilder_sc",
    locale: "en_US",
    vault: true,
    intent: "subscription",
  };

  useEffect(() => {
    if (!LocalStorage.getAccessToken()) {
      navigate("/user");
    }
    document.title = "Pro - Movieverse"
  }, [])

  const showError = (err: string | string[]) => {
    setStateProps({error: true, errorMessages: err, success: false, successMessages: []});
  };
 
  const showSuccess = (msg: string | string[]) => {
    setStateProps({error: false, errorMessages: [], success: true, successMessages: msg});
  };

  const createSubscriptionHandler = useCallback((data: Record<string, unknown>, actions: CreateSubscriptionActions): Promise<string> => {
    
  }, []);

  const onApproveHandler = useCallback((data: OnApproveData, actions: OnApproveActions): Promise<void> => {
    
  }, []);

  const onCancelHandler = useCallback((data: Record<string, unknown>, actions: OnCancelledActions): void => {
    
  }, []);

  const onErrorHandler = useCallback((err: Record<string, unknown>): void => {
    showError(err.toString());
  }, []);

  return (
    <div className="pro-page">
      <div className="pro-title">
        <span>Upgrade to pro</span>
      </div>
      <div className="pro-menu">
        <div className="pro-menu-info">

        </div>
        <PayPalScriptProvider options={initialOptions}>
          <PayPalButtons style={{ label: "subscribe", shape: "pill", }}
                         createSubscription={createSubscriptionHandler}
                         onApprove={onApproveHandler}
                         onCancel={onCancelHandler}
                         onError={onErrorHandler}
          />
        </PayPalScriptProvider>
      </div>
      <div className="media-error">
          {
            stateProps.error &&
            <Error errors={stateProps.errorMessages} />
          }
        </div>
        <div className="media-success">
          {
            stateProps.success &&
            <Success success={stateProps.successMessages} />
          }
        </div>
    </div>
  )
}