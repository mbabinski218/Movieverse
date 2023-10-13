import { Container, Nav, Navbar as NavbarBs, Row, Col } from "react-bootstrap";
import { useOutsideClickAlerter } from "../../hooks/useOutsideClickAlerter";
import { SearchBar } from "./SearchBar";
import { useCallback, useEffect, useRef, useState } from "react";
import { AccessToken, LocalStorage } from "../../hooks/useLocalStorage";
import jwtDecode from "jwt-decode";
import Logo from "../../assets/logo.svg";
import Chart from "../../assets/chart.svg";
import Check from "../../assets/check.svg";
import Person from "../../assets/person.svg";
import Menu from "../../assets/bars.svg";
import "./Navbar.css";

export const Navbar: React.FC = () => {
  const [menuOpen, setMenuOpen] = useState<boolean>(false);
  const [searchBarOpen, setSearchBarOpen] = useState<boolean>(false);
  const [user, setUser] = useState<string | null>(null);

  const outsideSearchBarClick = useCallback(() => {
    setSearchBarOpen(false);
  }, [setSearchBarOpen]);

  const searchRef = useRef(null);
  useOutsideClickAlerter(searchRef, () => {
    outsideSearchBarClick();
  });

  const searchBarClick = useCallback(() => {
    setSearchBarOpen(true);
    setMenuOpen(false);
  }, [setSearchBarOpen, setMenuOpen]);

  useEffect(() => { 
    const accessToken = LocalStorage.accessToken;

    if (!accessToken) {
      setUser(null);
      return;
    }

    try {
      const decodedToken = jwtDecode(accessToken) as AccessToken;
      setUser(decodedToken.displayName);
    }
    catch {
      setUser(null);
    }
  });

  return (
    <div className="header">
      <NavbarBs className={menuOpen ? "background-dark" : ""}>
        <Container className="content">
            <Nav.Link href="/"> 
              <img src={Logo} alt="logo" className="logo" />  
            </Nav.Link>
            <SearchBar
              searchRef={searchRef}
              searchBarOpen={searchBarOpen} 
              onClick={searchBarClick}
            />
            <a className="element navbar-button pro" href="/pro">
              <img src={Chart} alt="chart" className="chart" />
              <span>Pro</span>
            </a>
            <a className="element navbar-button watchlist" href="/watchlist">
              <img src={Check} alt="check" className="check" />
              <span>Watchlist</span>
            </a>
            <a className="element navbar-button user" href={user ? "/account" : "/user"}>
              <img src={Person} alt="person" className="person" />
              <span>{user ? user : "Sign in"}</span>
            </a>
            <img src={Menu} alt="menu" className="menu" onClick={() => {
              setMenuOpen(!menuOpen);
            }}/>
        </Container>
      </NavbarBs>
      <div className={menuOpen ? "menu-open selected" : "menu-close"}>
        <Container>
          <Row>
            <Col>
              <div className="category">
                <span>Movies</span>
              </div>
              <Nav.Link href="/chart" className="element-link">Release calendar</Nav.Link>
              <Nav.Link href="/chart" className="element-link">Top 100</Nav.Link>
              <Nav.Link href="/chart" className="element-link">Most popular</Nav.Link>
              <Nav.Link href="/chart" className="element-link">For you</Nav.Link>
            </Col>
            <Col>
              <div className="category">
                <span>Series</span>
              </div>
              <Nav.Link href="/chart" className="element-link">Release calendar</Nav.Link>
              <Nav.Link href="/chart" className="element-link">Top 100</Nav.Link>
              <Nav.Link href="/chart" className="element-link">Most popular</Nav.Link>
              <Nav.Link href="/chart" className="element-link">For you</Nav.Link>
            </Col>
            <Col>
              <div className="category">
                <span>Actors</span>
              </div>
              <Nav.Link href="/chart" className="element-link">Most popular</Nav.Link>
              <Nav.Link href="/chart" className="element-link">Born today</Nav.Link>
            </Col>
          </Row>
        </Container>
      </div>
    </div>          
  )
};