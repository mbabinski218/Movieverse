import { useEffect } from "react"

export const Watchlist: React.FC = () => {
  useEffect(() => {
    document.title = "Watchlist - Movieverse"
  }, [])

  return <h1>Watchlist</h1>
}