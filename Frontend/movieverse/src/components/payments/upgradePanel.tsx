import { useCallback, useEffect, useState } from "react"
import { PayPalScriptProvider, PayPalButtons, ReactPayPalScriptOptions } from "@paypal/react-paypal-js";
import { CreateSubscriptionActions, OnApproveData, OnApproveActions, OnCancelledActions } from "@paypal/paypal-js/types/components/buttons";
import { StateProps, emptyState } from "../../common/stateProps";
import { useNavigate } from "react-router-dom";
import { Success } from "../basic/Success";
import { Error } from "../basic/Error";
import { Api } from "../../Api";
import { PlanResponse } from "../../core/dtos/payment/PlanResponse";
import { environment } from "../../common/environment";
import "./UpgradePanel.css"

export const UpgradePanel: React.FC = () => {
  const [plan, setPlan] = useState<PlanResponse | null>(null);
  const [stateProps, setStateProps] = useState<StateProps>(emptyState);
  const navigate = useNavigate();

  const initialOptions: ReactPayPalScriptOptions = {
    clientId: environment.paypalClientId,
    currency: "USD",
    enableFunding: "paypal",
    disableFunding: "paylater",
    dataSdkIntegrationSource: "integrationbuilder_sc",
    locale: "en_US",
    vault: true,
    intent: "subscription"
  };

  useEffect(() => {
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

  const createSubscriptionHandler = useCallback(async (data: Record<string, unknown>, actions: CreateSubscriptionActions): Promise<string> => {   
    try {
      const response = await Api.paypalAuthorization()
        .then(res => res.json())        
        .then(res => res as {accessToken: string})

      const subId = await Api.paypalSubscription(response.accessToken)
        .then(res => res.text());

      return subId;
    }
    catch {
      showError("Error while subscribing to the plan. Please try again.");
    }

    return "error";
  }, []);

  const onApproveHandler = useCallback(async (data: OnApproveData, actions: OnApproveActions): Promise<void> => {
    if (data.subscriptionID) {
      showSuccess("You have successfully subscribed to the plan.");      
      Api.paypalStartSubscription(data.subscriptionID)
        .then(() => {
          Api.refreshTokens()
            .then(() => window.location.reload());
        })
    }
    else {
      showError("Error while subscribing to the plan. Please try again.");
    }

    return Promise.resolve();
  }, []);

  const onCancelHandler = useCallback((data: Record<string, unknown>, actions: OnCancelledActions): void => {
    showError("Transaction cancelled.");
  }, []);

  const onErrorHandler = useCallback((err: Record<string, unknown>): void => {
    showError("Transaction error. Please try again later.");
  }, []);

  return (
    <div className="up-page">
      <div className="up-title">
        <span>Upgrade to pro</span>
      </div>
      <div className="up-menu">
        {
          plan &&
          <div className="up-menu-info">
            <span className="up-menu-info-bold">{plan?.name}</span>
            <br/>
            <span className="up-menu-info-m">{plan?.description}</span>
            <br/><br/>
            <div className="up-menu-info-gold up-menu-info-m">
              {
                plan && 
                <>
                  {
                    plan.freeTrial &&
                    <>
                      <span>Start your 30-day free trial!</span>
                      <br/>
                      <span>{`Then only ${plan?.price} ${plan?.currency} per month`}</span>
                    </> ||
                    <span>{`Only ${plan?.price} ${plan?.currency} per month`}</span>
                  }
                </>
              }           
            </div>
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