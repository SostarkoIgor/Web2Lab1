import React, { useEffect, useState } from 'react';
import { useAuth0 } from '@auth0/auth0-react';
import { useNavigate } from 'react-router-dom';
import axios from 'axios';

interface Ticket {
    id: string;
    vatin: string;
    firstName: string;
    lastName: string;
    createdDate: string;
}


const source="https://web2lab1-0aid.onrender.com"
const TicketInfo: React.FC<{ ticketId: string }> = ({ ticketId }) => {
    const { user, isAuthenticated, loginWithRedirect, getAccessTokenSilently  } = useAuth0();
    const [ticket, setTicket] = useState<Ticket | null>(null);
    const [error, setError] = useState<string | null>(null);

    const navigate = useNavigate();

    useEffect(() => {
        const fetchTicketDetails = async () => {
            if (!isAuthenticated) {
                return;
            }

            try {
                const response = await axios.get(source+`/api/Ticket/ticketInfo/${ticketId}`, {
                    headers: {
                        Authorization: `Bearer ${await getAccessTokenSilently()}`,
                    },
                })
                setTicket(response.data);
            } catch (err) {
                setError('Error fetching ticket details');
                console.error(err);
            }
        };

        fetchTicketDetails();
    }, [isAuthenticated, ticketId]);

    if (!isAuthenticated) {
        return (
            <div className='app'>
                <h2>You need to be logged in to view ticket details</h2>
                <p>If logged in wait a bit for this page to load</p>
                <button onClick={() => loginWithRedirect()}>Login</button>
            </div>
        );
    }

    return (
        <div className="app">
            <p className='userName'>Current user: {user?.name}</p>
            {error && <div style={{ color: 'red' }}>{error}</div>}
            {ticket ? (
                <div className="ticketDetails">
                    <h2>Ticket Details</h2>
                    <p><strong>ID:</strong> {ticket.id}</p>
                    <p><strong>VATIN:</strong> {ticket.vatin}</p>
                    <p><strong>First Name:</strong> {ticket.firstName}</p>
                    <p><strong>Last Name:</strong> {ticket.lastName}</p>
                    <p><strong>Created At:</strong> {new Date(ticket.createdDate).toLocaleString()}</p>
                    <button onClick={() => navigate('/')}>Start page</button>
                </div>
            ) : (
                <h2>Loading ticket details...</h2>
            )}
        </div>
    );
};

export default TicketInfo
