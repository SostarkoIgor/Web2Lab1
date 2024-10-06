import React, { useEffect, useState } from 'react';
import { useAuth0 } from '@auth0/auth0-react';
import axios from 'axios';

interface Ticket {
    id: string;
    vatin: string;
    firstName: string;
    lastName: string;
    createdAt: string;
}

const TicketInfo: React.FC<{ ticketId: string }> = ({ ticketId }) => {
    const { user, isAuthenticated, loginWithRedirect, getAccessTokenSilently  } = useAuth0();
    const [ticket, setTicket] = useState<Ticket | null>(null);
    const [error, setError] = useState<string | null>(null);

    useEffect(() => {
        const fetchTicketDetails = async () => {
            if (!isAuthenticated) {
                return;
            }

            try {
                const response = await axios.get(`https://localhost:7075/api/Ticket/ticketInfo/${ticketId}`, {
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
            <div>
                <h2>You need to be logged in to view ticket details</h2>
                <button onClick={() => loginWithRedirect()}>Login</button>
            </div>
        );
    }

    return (
        <div className="App">
            <p className='userName'>Current user: {user?.name}</p>
            {error && <div style={{ color: 'red' }}>{error}</div>}
            {ticket ? (
                <div>
                    <h2>Ticket Details</h2>
                    <p><strong>ID:</strong> {ticket.id}</p>
                    <p><strong>VATIN:</strong> {ticket.vatin}</p>
                    <p><strong>First Name:</strong> {ticket.firstName}</p>
                    <p><strong>Last Name:</strong> {ticket.lastName}</p>
                    <p><strong>Created At:</strong> {new Date(ticket.createdAt).toLocaleString()}</p>
                </div>
            ) : (
                <h2>Loading ticket details...</h2>
            )}
        </div>
    );
};

export default TicketInfo
