# Prva labaratorijska vježba iz kolegija Napredni razvoj programske potpore za web
Projekt za Generiranje QR Kodova s Autentifikacijom i Autorizacijom

Ovaj projekt implementira aplikaciju za generiranje QR kodova i prikaz podataka o ulaznicama.
## Tehnologije

- **Frontend**: Vite + React
- **Backend**: ASP.NET Core
- **Baza podataka**: PostgreSQL (preporuča se na Renderu)
- **Autentifikacija i autorizacija**: OAuth2 i OpenId Connect (OIDC) putem Auth0 servisa

##Aplikacija je postavljena u oblak na Renderu
Glavna stranica: https://web2lab1-frontend.onrender.com/
Endpoint za stvaranje ulaznice: https://web2lab1-0aid.onrender.com/api/Ticket/createTicket
Auth0 endpoint za token: https://dev-sx10l5srw3t5xwff.us.auth0.com/oauth/token

Tijelo potrebno dati u zahtjevu za token:
{
	"client_id": "MTttxcYpTTg6QskWvcnSVXOjL9qyDFkY",
	"client_secret": "vSRbwJ1YgjTvc6Bpvr19NDzztpEu2W2XEsUkteSAfbfg5eWC3u6Ej9MAonClre6Z",
   "audience": "https://ticketapi",
   "grant_type": "client_credentials"
}

Pošto se koristi besplatna verzija Rendera za bazu podataka, moguće da je ta baza izbrisana u trenutku čitanja ovog teksta, te tada aplikacija neće dobro raditi.

# Tekst zadatka:

Cilj ovog projekta je demonstrirati prethodno predznanje vezano za izradu web-aplikacije koja komunicira s bazom podataka, omogućiti isporuku aplikacije u oblak, a zatim u nju ugraditi autentifikacijske i autorizacijske mehanizme iz prvog bloka predavanja.

Aplikacija će služiti za generiranje QR kodova za određenu namjenu (npr. ulaznice, pokloni paketi i slično, u daljnjem tekstu ulaznice) te za prikaz informacija pohranjenih u bazi podataka vezanih uz pojedinu ulaznicu.

Rješenje mora imati sljedeće funkcionalnosti:

1. Javno dostupna početna stranica koja prikazuje broj dosad generiranih ulaznica.
2. Pristupna točka (engl. endpoint) za generiranje ulaznice.
        Pristupna točka u tijelu zahtjeva prima json sa svojstvima vatin, firstName, lastName, koji predstavljaju OIB osobe ili tvrtke koja "kupuje" ulaznicu te ime i prezime na koga će ulaznica glasiti.
        Za jedan OIB se smiju generirati do (uključivo) 3 ulaznice.
        Identifikator ulaznice ne smije biti numerička vrijednosti, već npr. UUID iz PostgreSQL-a. Za svaku generiranu ulaznicu u bazi podataka osim prethodno navedenih podataka pospremiti i vrijeme kad je ulaznica kreirana.
        Rezultat uspješnog poziva je slika s QR kodom koji sadrži URL stranice određene identifikatorom ulaznici na kojoj se mogu doznati ostale informacije o ulaznici. U URL-u se ne smiju nalaziti podaci o OIB-u, imenu ili prezimenu, već samo identifikator ulaznice.
        U slučaju pogreške vratiti status 400 ili 500 s odgovarajućim opisom pogreške. Status 400 se treba vratiti ako ulazni json ne sadrži sve tražene podatke ili su za navedeni OIB već kupljene 3 ulaznice, pa nije dozvoljeno generirati dodatne ulaznice.
        Pristupna točka mora koristiti autorizacijski mehanizam OAuth2 Client Credentials (machine-to-machine) koji nije vezan za konkretnog korisnika, već za pojedinu aplikaciju. Detaljnije za ovaj mehanizam i Auth0 se može naći na https://auth0.com/blog/using-m2m-authorization
3. Stranica koja je jednoznačno određena identifikatorom ulaznice i prikazuje podatke o OIB-u, imenu, prezimenu te vremenu nastanka ulaznice.
        Pristup ovoj stranici imaju samo prijavljeni korisnici.
        Na stranici ispisati ime trenutno prijavljenog korisnika koristeći OpenId Connect protokol.

Upravljanje korisnicima odvija se korištenjem protokola OAuth2 i OpenId Connect (OIDC) i servisa Auth0. Korisnike na servisu Auth0 možete dodati kroz opciju User management/Users na Auth0. Za pohranu podataka koristiti PostgreSQL na Renderu ili neku drugu bazu podataka po izboru (npr. Firebase).

Aplikaciju postaviti u oblak (preporuča se besplatna opcija na Renderu), a kao odgovor na ovo pitanje isporučiti redom:

1. adresu javno dostupnog git repozitorija s izvornim kodom aplikacije (repozitorij postaviti javnim tek nakon isteka roka za predaju projekta)
2. adresu početne stranice aplikacije
3. adresu pristupne točke za kreiranje ulaznice te pristupne podatke za aplikaciju koja će pozivati generiranje ulaznica: clientid, clientsecret, issuerBaseUrl/authority (npr. https://fer-web2.eu.auth0.com)
4. testni korisnički račun s lozinkom s kojom se može otvoriti stranica s detaljima ulaznice

Zadatak ne postavlja nikakva ograničenje po pitanju programskog jezika i radnog okvira, ali se preporuča Node.js za koji je provjereno se da u njemu rješenje može implementirati i isporučiti na Render.

Rješenja koja nisu postavljena u oblak neće biti pregledavana.
