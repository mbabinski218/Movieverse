import { useEffect } from "react"

export const Pro: React.FC = () => {
  useEffect(() => {
    document.title = "Pro - Movieverse"
  }, [])

  return <h1>Pro</h1>
}