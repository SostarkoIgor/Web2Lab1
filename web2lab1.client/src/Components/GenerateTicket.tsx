import React, { useState } from 'react'
import axios from 'axios'
import { useAuth0 } from '@auth0/auth0-react'


const urlStart:string="https://localhost:7075"
const TicketForm = ({incrementNumberOfTickets} : {incrementNumberOfTickets: () => void}) => {
  const [vatin, setVatin] = useState<string>('')
  const [firstName, setFirstName] = useState<string>('')
  const [lastName, setLastName] = useState<string>('')
  const [qrCode, setQrCode] = useState<string | null>(null)
  const [error, setError] = useState<string>('')

  const { user, isAuthenticated, loginWithRedirect, getAccessTokenSilently  } = useAuth0()

  const handleSubmit = async (e: React.FormEvent) => {
    e.preventDefault()
    setError('')
    setQrCode(null)

    try {
      const response = await axios.post(urlStart + '/api/Ticket/createTicket', {
        vatin,
        firstName,
        lastName
      }, {
        headers: {
            Authorization: `Bearer ${await getAccessTokenSilently()}`
        },
        responseType: 'blob'
      })

      const qrCodeUrl = URL.createObjectURL(response.data)
      setQrCode(qrCodeUrl)
      incrementNumberOfTickets()
      setFirstName('')
      setLastName('')
      setVatin('')
    } catch (err) {
      setError('Error generating ticket')
    }
  }

  return (
    <div className='genTicket'>
      <p className='genTicketTitle'>Generate ticket</p>
      <form onSubmit={handleSubmit}>
        <div>
          <label>Vatin:</label>
          <input
            type="text"
            value={vatin}
            onChange={(e) => setVatin(e.target.value)}
            required
          />
        </div>
        <div>
          <label>First name:</label>
          <input
            type="text"
            value={firstName}
            onChange={(e) => setFirstName(e.target.value)}
            required
          />
        </div>
        <div>
          <label>Last name:</label>
          <input
            type="text"
            value={lastName}
            onChange={(e) => setLastName(e.target.value)}
            required
          />
        </div>
        <button type="submit">Generate</button>
      </form>

      {error && <p style={{ color: 'red' }}>{error}</p>}

      {qrCode && (
        <div className='qrCodeContainer'>
          <p className='qrCodeTitle'>QR Code</p>
          <img className='qrCode' src={qrCode} alt="QR Code" />
        </div>
      )}
    </div>
  )
}

export default TicketForm
