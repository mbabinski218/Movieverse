import { useCallback, useEffect, useState } from "react";
import { useNavigate } from "react-router-dom";
import { SubscriptionResponse } from "../../core/dtos/payment/SubscriptionResponse";
import { StateProps, emptyState } from "../../common/stateProps";
import { Success } from "../basic/Success";
import { Error } from "../basic/Error";
import { Button } from "../basic/Button";
import { Api } from "../../Api";
import "./ManagementPanel.css";
import { Loading } from "../basic/Loading";

export const ManagementPanel: React.FC = () => {
  const [loading, setLoading] = useState<boolean>(true);
  const [subscription, setSubscription] = useState<SubscriptionResponse | null>(null);
  const [stateProps, setStateProps] = useState<StateProps>(emptyState);
  const navigate = useNavigate();

  useEffect(() => {
    Api.paypalGetSubscription()
      .then(res => {
        if (res.ok) {
          return res.json()
        }
        else {
          return;
        }
      })
      .then(res => res as SubscriptionResponse)
      .then(res => {
        setSubscription(res);
        setLoading(false);
      })
    .catch(() => showError("Error while loading the subscription. Please try again later."))    
  }, []);

  const showError = (err: string | string[]) => {
    setStateProps({error: true, errorMessages: err, success: false, successMessages: []});
  };
 
  const showSuccess = (msg: string | string[]) => {
    setStateProps({error: false, errorMessages: [], success: true, successMessages: msg});
  };

  const cancleSubscriptionHandler = useCallback(async () => {
    try {
      const auth = await Api.paypalAuthorization()
        .then(res => res.json())        
        .then(res => res as {accessToken: string})

      Api.paypalCancelSubscription(auth.accessToken)
        .then(res => {
          if (res.ok) {
            showSuccess("Subscription canceled successfully.");
            Api.refreshTokens()
              .then(() => window.location.reload());
          }
          else {
            showError("Error while canceling the subscription. Please try again later.");
          }
        })
    }
    catch {
      showError("Error while canceling the subscription. Please try again later.");
    }
  }, [])

  return (
    <div className="mp-page">
      <div className="mp-title">
        <span>Subscription panel</span>
      </div>
      {
        loading ? 
        <div className="up-menu-loading">
          <Loading />
        </div>:
        <>
          {
            subscription ?
            <div className="mp-menu">
              {
                subscription &&
                <>
                  <div className="up-menu-info">
                    <span className="up-menu-info-bold">Available functions:</span><br/>
                    <span className="up-menu-info-m">Access to advanced statistics such as BoxOffice</span><br/>
                    <span className="up-menu-info-m">Adding new movies and series</span><br/><br/>
                    <span className="up-menu-info-gold up-menu-info-m">Next billing: {(new Date(subscription.nextBillingTime ?? new Date())).toLocaleDateString()}</span>
                  </div>
                    <Button label="Cancel subscription"
                            onClick={cancleSubscriptionHandler}
                    />
                </>
              }
            </div> :
            <div className="mp-menu up-menu-info">          
              <span>Pro subscription granted for life by the administrator.</span>
            </div>            
          }
        </>
      }
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