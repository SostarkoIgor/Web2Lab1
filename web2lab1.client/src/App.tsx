import React from 'react';
import { useAuth0 } from '@auth0/auth0-react'
import GenerateTicket from './Components/GenerateTicket'
import axios from 'axios'

const urlStart:string="https://web2lab1-0aid.onrender.com"
const App: React.FC = () => {
  const { loginWithRedirect, logout, isAuthenticated, user } = useAuth0();

  const [numberOfTickets, setNumberOfTickets] = React.useState<number>(0)

  React.useEffect(() => {
    async function fetchData() {
      try {
            let response = await axios.get(urlStart + '/api/Ticket/numberOfTickets')
            setNumberOfTickets(response.data)
        }
        catch (err) {
            console.log(err)
        }
    }
    fetchData()
  },[])

  return (
    <div className="app">
        {isAuthenticated ? (
          <div className="user">
            <p>Hello, <a className='userName'>{user?.name}</a>!</p>
            <button onClick={() => logout({ logoutParams: { returnTo: window.location.origin } })}>
              Log out
            </button>
          </div>
        ) : (
          <button onClick={() => loginWithRedirect()}>Sign in</button>
        )}

        <p className='ticketCount'>Number of tickets: <a className='numberOfTickets'>{numberOfTickets}</a></p>
        <GenerateTicket incrementNumberOfTickets={() => setNumberOfTickets(numberOfTickets + 1)} />
    </div>
  )
}

export default App
