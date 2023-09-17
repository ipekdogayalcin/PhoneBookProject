using System.Text.Json;

class PhoneBookEntry
{
    public string NationalId { get; set; }
    public string Name { get; set; }
    public string PhoneNumber { get; set; }
}

class PhoneBook
{
    private Dictionary<string, PhoneBookEntry> entries;

    public PhoneBook()
    {
        entries = new Dictionary<string, PhoneBookEntry>();
    }

    public void AddEntry(string nationalId, string name, string phoneNumber)
    {
        if (string.IsNullOrEmpty(nationalId))
        {
            Console.WriteLine("No national ID entered. Entry not added.");
            return;
        }

        if (entries.ContainsKey(nationalId))
        {
            Console.WriteLine("Entry with the same national ID already exists. Entry not added.");
            return;
        }

        if (string.IsNullOrEmpty(name))
        {
            Console.WriteLine("No name entered. Entry not added.");
            return;
        }

        if (name.Length > 50)
        {
            Console.WriteLine("Name exceeds the maximum character limit of 50. Entry not added.");
            return;
        }

        if (string.IsNullOrEmpty(phoneNumber))
        {
            Console.WriteLine("No phone number entered. Entry not added.");
            return;
        }

        if (phoneNumber.Length > 50)
        {
            Console.WriteLine("Phone number exceeds the maximum character limit of 50. Entry not added.");
            return;
        }

        var entry = new PhoneBookEntry { NationalId = nationalId, Name = name, PhoneNumber = phoneNumber };
        entries.Add(nationalId, entry);
        Console.WriteLine("Entry added successfully!");
    }

    public void DisplayEntries()
    {
        if (entries.Count == 0)
        {
            Console.WriteLine("Phone book is empty!");
            return;
        }

        var sortedEntries = entries.Values.OrderBy(e => e.Name);
        foreach (var entry in sortedEntries)
        {
            Console.WriteLine($"National ID: {entry.NationalId}\tName: {entry.Name}\tPhone Number: {entry.PhoneNumber}");
        }
    }


    public void SearchEntries(string searchTerm)
    {
        if (string.IsNullOrEmpty(searchTerm))
        {
            Console.WriteLine("No search term entered.");
            return;
        }

        var matchingEntries = new List<PhoneBookEntry>();
        foreach (var entry in entries.Values)
        {
            if (entry.NationalId.Contains(searchTerm, StringComparison.OrdinalIgnoreCase) ||
                entry.Name.Contains(searchTerm, StringComparison.OrdinalIgnoreCase) ||
                entry.PhoneNumber.Contains(searchTerm, StringComparison.OrdinalIgnoreCase))
            {
                matchingEntries.Add(entry);
            }
        }

        if (matchingEntries.Count == 0)
        {
            Console.WriteLine("No matching entries found.");
            return;
        }

        Console.WriteLine($"Matching Entries ({matchingEntries.Count}):");
        foreach (var entry in matchingEntries)
        {
            Console.WriteLine($"National ID: {entry.NationalId}\tName: {entry.Name}\tPhone Number: {entry.PhoneNumber}");
        }
    }

    public void UpdateEntry(string nationalId)
    {
        if (string.IsNullOrEmpty(nationalId))
        {
            Console.WriteLine("No national ID entered.");
            return;
        }

        if (!entries.ContainsKey(nationalId))
        {
            Console.WriteLine("No matching entry found.");
            return;
        }

        var entry = entries[nationalId];
        Console.WriteLine($"National ID: {entry.NationalId}\tName: {entry.Name}\tPhone Number: {entry.PhoneNumber}");

        Console.Write("Enter new name: ");
        string newName = Console.ReadLine();
        Console.Write("Enter new phone number: ");
        string newPhoneNumber = Console.ReadLine();

        entry.Name = newName;
        entry.PhoneNumber = newPhoneNumber;

        Console.WriteLine("Entry updated successfully!");
    }

    public void DeleteEntry(string nationalId)
    {
        if (string.IsNullOrEmpty(nationalId))
        {
            Console.WriteLine("No national ID entered.");
            return;
        }

        if (!entries.ContainsKey(nationalId))
        {
            Console.WriteLine("No matching entry found.");
            return;
        }

        entries.Remove(nationalId);
        Console.WriteLine("Entry deleted successfully!");
    }

    public void SavePhoneBook(string fileName)
    {
        try
        {
            string jsonData = JsonSerializer.Serialize(entries.Values);
            File.WriteAllText(fileName, jsonData);
            Console.WriteLine("Phone book saved successfully!");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Failed to save phone book: {ex.Message}");
        }
    }

    public void LoadPhoneBook(string fileName)
    {
        try
        {
            if (File.Exists(fileName))
            {
                string jsonData = File.ReadAllText(fileName);
                var loadedEntries = JsonSerializer.Deserialize<List<PhoneBookEntry>>(jsonData);

                entries.Clear();

                foreach (var entry in loadedEntries)
                {
                    if (!string.IsNullOrEmpty(entry.NationalId))
                    {

                        entries.Add(entry.NationalId, entry);
                    }
                }

                Console.WriteLine("Phone book loaded successfully!");
            }
            else
            {
                Console.WriteLine("Phone book file does not exist.");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Failed to load phone book: {ex.Message}");
        }
    }

}

class Program
{
    static void Main(string[] args)
    {
        PhoneBook phoneBook = new PhoneBook();
        string fileName = "phonebook.json";

        while (true)
        {
            Console.WriteLine("Phone Book Menu:");
            Console.WriteLine("1. Add Entry");
            Console.WriteLine("2. Display Entries");
            Console.WriteLine("3. Search Entries");
            Console.WriteLine("4. Update Entry");
            Console.WriteLine("5. Delete Entry");
            Console.WriteLine("6. Save Phone Book");
            Console.WriteLine("7. Load Phone Book");
            Console.WriteLine("8. Exit");
            Console.Write("Enter your choice: ");
            string choice = Console.ReadLine();

            switch (choice)
            {
                case "1":
                    Console.Write("Enter national ID: ");
                    string nationalId = Console.ReadLine();
                    Console.Write("Enter name: ");
                    string name = Console.ReadLine();
                    Console.Write("Enter phone number: ");
                    string phoneNumber = Console.ReadLine();
                    phoneBook.AddEntry(nationalId, name, phoneNumber);
                    break;
                case "2":
                    phoneBook.DisplayEntries();
                    break;
                case "3":
                    Console.Write("Enter the search term: ");
                    string searchTerm = Console.ReadLine();
                    phoneBook.SearchEntries(searchTerm);
                    break;
                case "4":
                    Console.Write("Enter the national ID: ");
                    string updateNationalId = Console.ReadLine();
                    phoneBook.UpdateEntry(updateNationalId);
                    break;
                case "5":
                    Console.Write("Enter the national ID: ");
                    string deleteNationalId = Console.ReadLine();
                    phoneBook.DeleteEntry(deleteNationalId);
                    break;
                case "6":
                    phoneBook.SavePhoneBook(fileName);
                    break;
                case "7":
                    phoneBook.LoadPhoneBook(fileName);
                    break;
                case "8":
                    Console.WriteLine("Exiting...");
                    return;
                default:
                    Console.WriteLine("Invalid choice. Please try again.");
                    break;
            }

            Console.WriteLine();
        }
    }
}
