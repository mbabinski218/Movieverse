import { BrowserRouter, Route, Routes } from "react-router-dom";
import { Navbar } from "./components/navbar/Navbar";
import "./styles/variables.css";
import { Home } from "./pages/Home";
import { Watchlist } from "./pages/Watchlist";
import { Pro } from "./pages/Pro";
import { Chart } from "./pages/Chart";
import { User } from "./pages/User";
import { Find } from "./pages/Find";
import { Media } from "./pages/Media";
import { useEffect } from "react";

const App: React.FC = () => {
  useEffect(() => {
    document.title = "Movieverse"
  })

  return (
    <>      
      <Navbar />
      <BrowserRouter>
        <Routes>
          <Route path="/" element={<Home />} />
          <Route path="/media" element={<Media />} />
          <Route path="/pro" element={<Pro />} />
          <Route path="/watchlist" element={<Watchlist />} />
          <Route path="/user" element={<User />} />
          <Route path="/chart" element={<Chart />} />
          <Route path="/find" element={<Find />} />
        </Routes>
      </BrowserRouter>
    </>
  );
}

export default App;
