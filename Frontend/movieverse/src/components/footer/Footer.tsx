import { Container } from "react-bootstrap";
import "./Footer.css";

export const Footer: React.FC = () => {
	return (
		<Container className="footer">
				<hr className="break"/>
				<div className="footer-items">
					<span>© 2023 Movieverse</span>
					<div className="shortcuts">	
						<a className="shortcuts-item">Privacy Policy</a>
						<a className="shortcuts-item">Terms of Service</a>
						<a className="shortcuts-item">Legal</a>
					</div>
				</div>
		</Container>
	)
}