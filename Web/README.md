# ZealotZone

**Projektledare för mina Protoss-enheter**  
En ASP.NET-baserad webbapplikation för hantering av projekt inom Protoss-fraktionen.

Denna text är genererad av AI :)

---

## ✅ Funktioner

- Skapa, visa och hantera projekt
- Filtrera projekt baserat på status (påbörjade/slutförda)
- Användarregistrering och inloggning
- Skyddade användarspecifika sidor
- Formulärvalidering med JavaScript (Vanilla & Alpine.js)
- Databas via Azure SQL med Entity Framework Core
- Följer Service Pattern och Microsoft Identity (Individual Account)

---

## 📋 Krav för godkänt

| Funktion                                       | Status | Kommentar                                        |
| ---------------------------------------------- | ------ | ------------------------------------------------ |
| Samtliga sidor enligt designfil                | ❌      | Projektformulär saknas (Skapa/redigera projekt)  |
| Visa projekt efter status (startade/slutförda) | ❌      | Ej implementerat, endast mockdata                |
| Hantera projektstatus                          | ❌      | Krävs även om det inte är med i designfil        |
| Utökning av projektmodell                      | ❌      | Ej implementerat                                 |
| Skapa projekt via formulär                     | ❌      | Endast användare kan läggas till via formulär    |
| Formulärvalidering med JavaScript              | ✅      | Använder Vanilla JS & Alpine.js                  |
| Databas (Entity Framework Core)                | ✅      | Azure SQL (online)                               |
| Microsoft Identity (authentisering)            | ✅      | Registrering, inloggning och utloggning fungerar |
| Skydd av användarspecifika sidor               | ✅      | Projektsidor är skyddade med `Authorize`         |

---

## ⚙️ Teknikstack

- ASP.NET (Razor Pages eller MVC)
- Entity Framework Core
- Microsoft Identity
- Azure SQL Database
- JavaScript (Vanilla & Alpine.js)
- HTML/CSS

---

## 🚀 Kom igång

### Klona projektet

```bash
git clone https://github.com/ditt-användarnamn/ZealotZone.git
cd ZealotZone
