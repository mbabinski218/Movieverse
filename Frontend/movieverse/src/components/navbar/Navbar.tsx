import { Container, Nav, Navbar as NavbarBs, Row, Col } from "react-bootstrap";
import { useEffect, useState } from 'react';
import Logo from "../../assets/logo.svg";
import Chart from "../../assets/chart.svg";
import Check from "../../assets/check.svg";
import User from "../../assets/user.svg";
import Menu from "../../assets/bars.svg";
import { SearchBar } from "./SearchBar";
import "./Navbar.css";

export const Navbar: React.FC = () => {
  useEffect(() => {
    document.title = "Movieverse"
  })

  const [menuOpen, setMenuOpen] = useState(false);

  return (
    <>
      <NavbarBs>
        <Container className="content">
            <Nav.Link href="/"> 
              <img src={Logo} className="logo" />  
            </Nav.Link>
            <SearchBar />
            <div className="element button">
              <img src={Chart} className="chart" />
              <span>Pro</span>
            </div>
            <div className="element button">
              <img src={Check} className="check" />
              <span>Watchlist</span>
            </div>
            <div className="element button">
              <img src={User} className="user" />
              <span>Sign in</span>
            </div>
            <img src={Menu} className="menu" onClick={() => {
              setMenuOpen(!menuOpen);
            }}/>
        </Container>
      </NavbarBs>
      <div className={menuOpen ? "menu-open selected" : "menu-close"}>
          <Container className="center">
            <Row>
              <Col>
                <div className="category">
                  <span>Movies</span>
                </div>
                <div className="element button">
                  <span>Release calendar</span>
                </div>
                <div className="element button">
                  <span>Top 100</span>
                </div>
                <div className="element button">
                  <span>Most popular</span>
                </div>
                <div className="element button">
                  <span>For you</span>
                </div>
              </Col>
              <Col>
                <div className="category">
                  <span>Series</span>
                </div>
                <div className="element button">
                  <span>Release calendar</span>
                </div>
                <div className="element button">
                  <span>Top 100</span>
                </div>
                <div className="element button">
                  <span>Most popular</span>
                </div>
                <div className="element button">
                  <span>For you</span>
                </div>
              </Col>
              <Col>
                <div className="category">
                  <span>Actors</span>
                </div>
                <div className="element button">
                  <span>Most popular</span>
                </div>
                <div className="element button">
                  <span>Born today</span>
                </div>
              </Col>
            </Row>
          </Container>
      </div>
    </>          
  )
};