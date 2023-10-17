export interface StateProps {
  error: boolean;
  errorMessages: string[] | string;
  success: boolean;
  successMessages: string[] | string;
}

export const emptyAuthState: StateProps = {
  error: false,
  errorMessages: [],
  success: false,
  successMessages: []
};
