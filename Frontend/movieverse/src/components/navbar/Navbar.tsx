import { Container, Nav, Navbar as NavbarBs, Row, Col } from "react-bootstrap";
import { useEffect, useState } from 'react';
import Logo from "../../assets/logo.svg";
import Chart from "../../assets/chart.svg";
import Check from "../../assets/check.svg";
import Person from "../../assets/person.svg";
import Menu from "../../assets/bars.svg";
import { SearchBar } from "./SearchBar";
import "./Navbar.css";

export const Navbar: React.FC = () => {
  useEffect(() => {
    document.title = "Movieverse"
  })

  const [menuOpen, setMenuOpen] = useState(false);
  const [searchBarOpen, setSearchBarOpen] = useState(false);

  return (
    <>
      <NavbarBs className={menuOpen ? "background-dark" : ""}>
        <Container className="content">
            <Nav.Link href="/"> 
              <img src={Logo} alt="logo" className="logo" />  
            </Nav.Link>
            <SearchBar onClick={() => {
              setSearchBarOpen(!searchBarOpen);
              setMenuOpen(false);
            }}/>
            <a className="element button pro" href="/pro">
              <img src={Chart} alt="chart" className="chart" />
              <span>Pro</span>
            </a>
            <a className="element button watchlist" href="/watchlist">
              <img src={Check} alt="check" className="check" />
              <span>Watchlist</span>
            </a>
            <a className="element button user" href="/user">
              <img src={Person} alt="person" className="person" />
              <span>Sign in</span>
            </a>
            <img src={Menu} alt="menu" className="menu" onClick={() => {
              setMenuOpen(!menuOpen);
              setSearchBarOpen(false);
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
    </>          
  )
};