import { useCallback, useEffect, useState } from "react"
import { PayPalScriptProvider, PayPalButtons, ReactPayPalScriptOptions } from "@paypal/react-paypal-js";
import { CreateSubscriptionActions, OnApproveData, OnApproveActions, OnCancelledActions } from "@paypal/paypal-js/types/components/buttons";
import { StateProps, emptyState } from "../common/stateProps";
import { LocalStorage } from "../hooks/useLocalStorage";
import { useNavigate } from "react-router-dom";
import { Success } from "../components/basic/Success";
import { Error } from "../components/basic/Error";
import { Api } from "../Api";
import { PlanResponse } from "../core/dtos/payment/PlanResponse";
import "./Pro.css"

export const Pro: React.FC = () => {
  const [plan, setPlan] = useState<PlanResponse | null>(null);
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
    document.title = "Pro - Movieverse";

    Api.paypalAuthorization()
    .then(res => {
      if(res.ok) {
        res.json()
          .then(res => res as {accessToken: string})
          .then(res => Api.paypalPlan(res.accessToken))
          .then(res => {
            if(res.ok) {
              res.json()
                .then(res => res as PlanResponse)
                .then(setPlan)
            }
            else {
              showError("Error while loading the plan. Please try again later.");
            }
          })
      }
      else {
        showError("Error with the authorization. Please try again later.");
      }
    })
    .catch(() => {
      showError("Fatal error. Please try again later.");
    })
  }, [])

  const showError = (err: string | string[]) => {
    setStateProps({error: true, errorMessages: err, success: false, successMessages: []});
  };
 
  const showSuccess = (msg: string | string[]) => {
    setStateProps({error: false, errorMessages: [], success: true, successMessages: msg});
  };

  const createSubscriptionHandler = useCallback((data: Record<string, unknown>, actions: CreateSubscriptionActions): Promise<string> => {
    return Promise.resolve("Subscription created");
  }, []);

  const onApproveHandler = useCallback((data: OnApproveData, actions: OnApproveActions): Promise<void> => {
    return Promise.resolve();
  }, []);

  const onCancelHandler = useCallback((data: Record<string, unknown>, actions: OnCancelledActions): void => {
    
  }, []);

  const onErrorHandler = useCallback((err: Record<string, unknown>): void => {
    showError("Error while creating the subscription. Please try again later.");
  }, []);

  return (
    <div className="pro-page">
      <div className="pro-title">
        <span>Upgrade to pro</span>
      </div>
      <div className="pro-menu">
        {
          plan &&
          <div className="pro-menu-info">
            <span className="pro-menu-info-bold">{plan?.name}</span>
            <br/>
            <span className="pro-menu-info-m">{plan?.description}</span>
            <br/><br/>
            <span className="pro-menu-info-gold pro-menu-info-m">{plan ? `${plan?.price} ${plan?.currency} per month` : ""}</span>
          </div>          
        }
        <PayPalScriptProvider options={initialOptions}>
          <PayPalButtons style={{ color:"white", label: "subscribe", shape: "pill", }}
                         createSubscription={createSubscriptionHandler}
                         onApprove={onApproveHandler}
                         onCancel={onCancelHandler}
                         onError={onErrorHandler}
                         disabled={plan === null}
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