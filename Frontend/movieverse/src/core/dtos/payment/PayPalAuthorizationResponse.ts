import { ClientMetadata } from "./ClientMetadata"

export type PayPalAuthorizationResponse = {
  scope: string
  access_token: string
	token_type: string
	app_id: string
	expires_in: number
	supported_authn_schemes: string[]
	nonce: string
	client_metadata: ClientMetadata
}