import { Container, Nav, Navbar as NavbarBs } from "react-bootstrap";
import { ReactComponent as Logo } from "../../assets/logo.svg";
import "./Navbar.css";

export const Navbar: React.FC = () => {
  return (
    <NavbarBs>
      <Container>
          <Logo className="svg"/>
      </Container>
    </NavbarBs>
  )
};