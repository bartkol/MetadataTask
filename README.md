## Spis treści
- [Opis](#opis)
- [Struktura solucji](#struktura-solucji)
- [Szybki start](#szybki-start)
- [Technologie](#technologie)
- [Standardy dla programistów](#standardy-dla-programistów)

---

## Opis
Projekt jest symulacją dodawania nowego konektora (integracji) do ekosystemu Dataedo. Został on przygotowany na potrzeby zadania rekrutującego w celu sprawdzenia znajomości dobrych praktyk, standardów oraz umiejętności praktycznych kandydata.

## Struktura solucji
- **FivetranClient** - klient REST pobierający z interfejsu Fivetran metadane niezbędne do importu.
- **Import** - warstwa aplikacyjna, która korzysta z klientów, aby wylistować w konsoli wszystkie przepływy danych (data lineage) na poziomie obiektów.
- **ImportTests** - projekt zawierający testy jednostkowe.

## Szybki start
### Wymagania
- .NET 8+
- odpowiednie IDE (Visual Studio, Visual Studio Code + C# Dev Kit)

### Instrukcja jak korzystać i jak testować
W celu uruchomienia programu należy wystartować projekt **Import**. Można to zrobić poprzez kilka opcji:
1. Terminal/wiersz poleceń - będąc w głównym katalogu solucji należy uruchomić polecenia:
```bash
dotnet build
dotnet run
```
2. Visual Studio - ustawić ww. projekt jako startowy i wcisnąć `Ctrl+F5` albo wybrać `Debug > Start without debugging` z górnego menu, albo kliknąć na zielony przycisk Start.
3. Visual Studio Code - wcisnąć przycisk `F5` albo wybrać z bocznego menu `Run and Debug` i wcisnąć zielony przycisk Start.

Niezależnie od wybranej opcji, w terminalu będzie można używać aplikacji, gdzie użytkownik będzie proszony o wybór odpowiednimi komunikatami. Aby można było w pełni przejść proces należy posiadać Fivetran API Key oraz Secret.

Oprócz testowania manualnego można uruchomić projekt testowy zawierający testy jednostkowe. Można to zrobić na różne sposoby:
1. W terminalu uruchomić polecenia
```bash
dotnet build
dotnet test
```
2. Uruchomić testy bezpośrednio z projektu testowego za pomocą Test Explorera w IDE (zakładka Testing w przypadku Visual Studio Code).

## Technologie
- **C#** - język programowania
- **.NET** - główny framework
- **REST API** - komunikacja z serwisem Fivetran

### Testy
**Frameworki i narzędzia:**
- **xUnit** - główny framework testowy,
- **FluentAssertions** - do sprawdzania wyników testów,
- **NSubstitute** - w celu tworzenia mocków.

**Metodologie i wzorce:**
- **AAA Pattern** - główny wzorzec testu jednostkowego,
- **Builder Pattern** - wzorzec wykorzystywany do tworzenia obiektów i serwisów podlegających testom. Dzięki nim testy są czytelne i proste w zrozumieniu.

## Standardy dla programistów
1. Wszystkie commity w projekcie powinny być zgodne z konwencją [Conventional Commits](https://www.conventionalcommits.org/). 
2. Kod powinien być zgodny z zasadami [.NET Coding Conventions](https://docs.microsoft.com/en-us/dotnet/csharp/fundamentals/coding-style/coding-conventions)