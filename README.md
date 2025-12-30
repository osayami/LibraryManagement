# LibraryManagement

Simple C# console app that stores up to five book titles using five string variables. The program supports adding a book, removing a book (by index or exact title), and displaying available books.

## Build & Run

From the project folder:

```bash
cd /mnt/HDD/Projects/Programming/dotnet/Coursera/LibraryManagement
dotnet build
dotnet run
```

The app will show a simple menu.

## Notes
- The program intentionally uses five separate `string` variables (no arrays or lists) to match the requirement.
- Removal accepts either the slot index (1-5) or an exact case-insensitive title match.
