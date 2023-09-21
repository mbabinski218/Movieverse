import { Container, Nav, Navbar as NavbarBs } from "react-bootstrap";
import Logo from "../../assets/logo.svg";
import Check from "../../assets/check.svg";
import User from "../../assets/user.svg";
import Menu from "../../assets/bars.svg";
import { SearchBar } from "./SearchBar";
import "./Navbar.css";

export const Navbar: React.FC = () => {
  return (
    <NavbarBs>
      <Container className="content">
          <Nav.Link href="/"> 
            <img src={Logo} className="logo" />  
          </Nav.Link>
          <SearchBar />
          <div className="element button">
            <img src={Check} className="check" />
            <span>Watchlist</span>
          </div>
          <div className="element button">
            <img src={User} className="user" />
            <span>Sign in</span>
          </div>          
          <img src={Menu} className="menu" />
      </Container>
    </NavbarBs>
  )
};