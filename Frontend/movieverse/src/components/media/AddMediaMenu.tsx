import { useCallback, useState } from "react";
import { Api } from "../../Api";
import { Button } from "../basic/Button";
import { Input } from "../basic/Input";
import { AddMediaContract } from "../../core/contracts/addMediaContract";
import { StateProps, emptyState } from "../../common/stateProps";
import { Success } from "../basic/Success";
import { Error } from "../basic/Error";
import { useNavigate } from "react-router-dom";
import { PageBlur } from "../basic/PageBlur";
import "./AddMediaMenu.css";
import Close from "../../assets/bars-close.svg";

export interface AddMediaMenuProps {
  onClose?: () => void;
  onSuccessfulAdd?: () => void;
}

export const AddMediaMenu: React.FC<AddMediaMenuProps> = (props) => {
  const [type, setType] = useState<"Movie" | "Series">("Movie")
  const [title, setTitle] = useState<string | null>(null)
  const [stateProps, setStateProps] = useState<StateProps>(emptyState);
  const navigate = useNavigate();

  const handleTitleChange = useCallback((e: React.ChangeEvent<HTMLInputElement>) => {
    setTitle(e.target.value);
  }, []);

  const selectTypeHandler = useCallback((e: React.ChangeEvent<HTMLSelectElement>) => {
    setType(e.target.value as "Movie" | "Series");
  }, []);

  const showError = (err: string | string[]) => {
    setStateProps({error: true, errorMessages: err, success: false, successMessages: []});
  };

  const showSuccess = (msg: string | string[]) => {
    setStateProps({error: false, errorMessages: [], success: true, successMessages: msg});
  };

  const handleAddButtonClick = useCallback(() => {
    if (title === null || title.length === 0) {
      showError("Title is required!")
      return;
    }

    const body: AddMediaContract = {
      title: title,
      type: type
    }

    Api.addMedia(body)
      .then((res) => {
        if (res.ok) {
          res.text().then((id: string) => {
            setTitle(null);
            setType("Movie");
            showSuccess("Media added successfully!");
            if (props.onSuccessfulAdd) {
              props.onSuccessfulAdd();
            }
            
            navigate(`/media/${id}`);
            window.location.reload();
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
  }, [title, type]);

  return (
    <>
      <PageBlur />
      <div className="media-window">
        <div className="media-add-menu">
          <span className="media-add-title">Add new media</span>
          <img src={Close} onClick={props.onClose} alt="close" className="media-add-close" />
          <div className="media-inputs">
            <select className="media-select" 
                    title="Type" 
                    onChange={selectTypeHandler}
            >
              <option value="Movie">Movie</option>
              <option value="Series">Series</option>
            </select>
            <Input label="Title"
                  value={title ?? ""}
                  onChange={handleTitleChange}
            />
          </div>
          <Button className="media-button"
                  label="Add"              
                  primary={true}
                  disabled={title === null || title.length === 0}
                  onClick={handleAddButtonClick}
          />
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
    </>
  );
}