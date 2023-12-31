import { Container, Nav, Navbar as NavbarBs, Row, Col } from "react-bootstrap";
import { useOutsideClickAlerter } from "../../hooks/useOutsideClickAlerter";
import { SearchBar } from "./SearchBar";
import { useCallback, useEffect, useRef, useState } from "react";
import { AddMediaMenu } from "../media/AddMediaMenu";
import { RoleEditor } from "../user/RoleEditor";
import { useUserToken } from "../../hooks/useUserToken";
import { UserRoles } from "../../UserRoles";
import { AddPersonMenu } from "../person/AddPersonMenu";
import Logo from "../../assets/logo.svg";
import Chart from "../../assets/chart.svg";
import Check from "../../assets/check.svg";
import Person from "../../assets/person.svg";
import Menu from "../../assets/bars.svg";
import MenuClose from "../../assets/bars-close.svg";
import "./Navbar.css";

export const Navbar: React.FC = () => {
  const [menuOpen, setMenuOpen] = useState<boolean>(false);
  const [searchBarOpen, setSearchBarOpen] = useState<boolean>(false);
  const [addMediaMenuOpen, setAddMediaMenuOpen] = useState<boolean>(false);
  const [addPersonMenuOpen, setAddPersonMenuOpen] = useState<boolean>(false);
  const [roleEditorOpen, setRoleEditorOpen] = useState<boolean>(false);
  const [user, setUser] = useState<string | null>(null);
  const [userToken] = useUserToken();

  useEffect(() => {
    if (userToken) {
      setUser(userToken.displayName);
    }
  }, [userToken]);

  const outsideSearchBarClick = useCallback(() => {
    setSearchBarOpen(false);
  }, []);

  const searchRef = useRef(null);
  useOutsideClickAlerter(searchRef, () => {
    outsideSearchBarClick();
  });

  const searchBarSelect = useCallback(() => {
    setSearchBarOpen(true);
    setMenuOpen(false);
  }, []);

  const openAddMediaMenu = useCallback(() => {
    setAddMediaMenuOpen(true);
    setMenuOpen(false);
  }, []);

  const colseAddMediaMenu = useCallback(() => {
    setAddMediaMenuOpen(false);
  }, []);

  const openAddPersonMenu = useCallback(() => {
    setAddPersonMenuOpen(true);
    setMenuOpen(false);
  }, []);

  const colseAddPersonMenu = useCallback(() => {
    setAddPersonMenuOpen(false);
  }, []);

  const openRoleEditor = useCallback(() => {
    setRoleEditorOpen(true);
    setMenuOpen(false);
  }, []);

  const closeRoleEditor = useCallback(() => {
    setRoleEditorOpen(false);
  }, []);

  const redirectToPersonality = useCallback(() => {
    window.location.href = `/person/${userToken?.personId}`;
  }, [userToken]);

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
              onSelect={searchBarSelect}
            />
            <a className="element navbar-button pro" href="/pro">
              <img src={Chart} alt="chart" className="chart" />
              <span>Pro</span>
            </a>
            <a className="element navbar-button watchlist" href="/chart/watchlist">
              <img src={Check} alt="check" className="check" />
              <span>Watchlist</span>
            </a>
            <a className="element navbar-button user" href={user ? "/account" : "/user"}>
              <img src={Person} alt="person" className="person" />
              <span>{user ? user : "Sign in"}</span>
            </a>
            <img src={menuOpen ? MenuClose : Menu} alt="menu" className="menu" onClick={() => {
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
              <Nav.Link href="/chart/movies/releaseCalendar" className="element-link">Release calendar</Nav.Link>
              <Nav.Link href="/chart/movies/top100" className="element-link">Top 100</Nav.Link>
              <Nav.Link href="/chart/movies/mostPopular" className="element-link">Most popular</Nav.Link>
            </Col>
            <Col>
              <div className="category">
                <span>Series</span>
              </div>
              <Nav.Link href="/chart/series/releaseCalendar" className="element-link">Release calendar</Nav.Link>
              <Nav.Link href="/chart/series/top100" className="element-link">Top 100</Nav.Link>
              <Nav.Link href="/chart/series/mostPopular" className="element-link">Most popular</Nav.Link>
            </Col>
            <Col>
              <div className="category">
                <span>People</span>
              </div>
              <Nav.Link href="/chart/people/bornToday" className="element-link">Born today</Nav.Link>
            </Col>
            {
              (userToken?.role.includes(UserRoles.Administrator) || userToken?.role.includes(UserRoles.SystemAdministrator)) &&
              <Col>
                <div className="category">
                  <span>Utility panel</span>
                </div>
                <span onClick={openAddMediaMenu} className="element-link">Add new media</span>
                <br />
                <span onClick={openAddPersonMenu} className="element-link">Add new person</span>
                <br />
                <span onClick={openRoleEditor} className="element-link">Role editor</span>
              </Col> ||
              userToken?.role.includes(UserRoles.Pro) &&
              <Col>
                <div className="category">
                  <span>Utility panel</span>
                </div>
                {
                  userToken.personId ?
                  <span onClick={redirectToPersonality} className="element-link">Open your own site</span> :
                  <span onClick={openAddPersonMenu} className="element-link">Create your own site</span>
                }
              </Col>
            }
          </Row>
        </Container>
      </div>
      {
        addMediaMenuOpen &&
        <AddMediaMenu onClose={colseAddMediaMenu}
                      onSuccessfulAdd={colseAddMediaMenu}
        />
      }
      {
        addPersonMenuOpen &&
        <AddPersonMenu forUser={!userToken?.role.includes(UserRoles.Administrator) && !userToken?.role.includes(UserRoles.SystemAdministrator)}
                       onClose={colseAddPersonMenu}
                       onSuccessfulAdd={colseAddPersonMenu}
        />
      }
      {
        roleEditorOpen &&
        <RoleEditor onClose={closeRoleEditor} />
      }
    </div>          
  )
};