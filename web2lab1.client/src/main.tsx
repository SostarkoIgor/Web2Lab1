import { StrictMode } from 'react'
import { createRoot } from 'react-dom/client'
import App from './App.tsx'
import './index.css'
import { Auth0Provider } from '@auth0/auth0-react'
import { Routes, Route, BrowserRouter } from 'react-router-dom';
import TicketInfo from './Pages/TicketInfo.tsx'

const domain="dev-sx10l5srw3t5xwff.us.auth0.com"
const clientId="FwK0bpM82iAZTgkZeRZQx0koeuXTPELF"

createRoot(document.getElementById('root')!).render(
  <Auth0Provider domain={domain} clientId={clientId}
    authorizationParams={{ redirect_uri: window.location.origin, response_type: 'code', audience: "https://ticketapi" }}>
    <StrictMode>
      <BrowserRouter>
        <Routes>
          <Route path="/" element={<App />} />
          <Route path="/ticket/:ticketId" element={<TicketInfo ticketId={window.location.pathname.split("/")[2]}/>} />
          <Route path="*" element={<App />} />
        </Routes>
      </BrowserRouter>
    </StrictMode>
  </Auth0Provider>
)
