import { BrowserRouter, Route, Routes } from "react-router-dom";
import { Navbar } from "./components/navbar/Navbar";
import { Footer } from "./components/footer/Footer";
import { Home } from "./pages/Home";
import { Pro } from "./pages/Pro";
import { Chart } from "./pages/Chart";
import { Media } from "./pages/Media";
import { Episodes } from "./pages/Episodes";
import { Person } from "./pages/Person";
import { User } from "./pages/User";
import { Account } from "./pages/Account";
import { Find } from "./pages/Find";
import { NotFound } from "./pages/NotFound";
import "./styles/variables.css";
import "./App.css";

const App: React.FC = () => {
  return (
    <div className="app">      
      <BrowserRouter>
        <Navbar />
        <Routes>
          <Route path="/" element={<Home />} />
          <Route path="/media/:id" element={<Media />} />
          <Route path="/media/:id/episodes" element={<Episodes />} />
          <Route path="/person/:id" element={<Person />} />
          <Route path="/pro" element={<Pro />} />
          <Route path="/user" element={<User />} />
          <Route path="/account" element={<Account />} />
          <Route path="/chart/:type/:category?" element={<Chart />} />
          <Route path="/find" element={<Find />} />
          <Route path="/not-found" element={<NotFound />} />
        </Routes>
        <Footer />
      </BrowserRouter>
    </div>
  );
}

export default App;