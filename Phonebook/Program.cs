using System.Text.Json;

class Program
{
    static void Main(string[] args)
    {
        List<Contact> phonebook = new List<Contact>
        {
            new Contact("Alice", "123-456-7890", "alice@gmail.com"), new Contact("Bob", "543-543-5432", "bob@gmail.com")
        };
        string choice;
        string fileName;

        while (true)
        {
            PrintMenu();
            choice = Console.ReadLine();


            switch (choice)
            {
                case "1":
                    Console.WriteLine("Type name of file: ");
                    fileName = Console.ReadLine();
                    ExportPhonebookJSON(phonebook, fileName + ".JSON");
                    Console.WriteLine($"Phonebook exported to {fileName}.json");
                    break;
                case "2":
                    fileName = GetFileName();
                    ExportPhonebookCSV(phonebook, fileName + ".csv");
                    Console.WriteLine($"Phonebook exported to {fileName}.csv");
                    break;
                case "3":
                    fileName = GetFileName();
                    if (File.Exists(fileName))
                    {
                        phonebook = ImportPhonebook(fileName);
                        Console.WriteLine($"Phonebook imported from {fileName}");
                    }
                    else
                    {
                        Console.WriteLine("File not found.");
                    }
                    break;
                case "4":
                    Console.WriteLine();
                    Console.WriteLine("Phonebook contents:");
                    if (phonebook.Count == 0)
                    {
                        Console.WriteLine("Phonebook is empty.");
                    }
                    else
                    {
                        foreach (var contact in phonebook)
                        {
                            Console.WriteLine(contact.ToString());
                        }
                    }
                    Console.WriteLine();
                    break;

                case "5":
                    Console.Write("Enter new contact name: ");
                    string name = Console.ReadLine();
                    Console.Write("Enter new contact phone number: ");
                    string phoneNumber = Console.ReadLine();
                    Console.Write("Enter new contact email: ");
                    string email = Console.ReadLine();
                    if (!string.IsNullOrEmpty(name) && !string.IsNullOrEmpty(phoneNumber) && !string.IsNullOrEmpty(email))
                    {
                        phonebook.Add(new Contact(name, phoneNumber, email));
                        Console.WriteLine("New contact added.");
                    }
                    else
                    {
                        Console.WriteLine("Invalid input. Contact not added.");
                    }
                    break;

                case "6":
                    Console.Write("Enter the name of the contact to remove: ");
                    string contactName = Console.ReadLine();
                    Contact contactToRemove = phonebook.FirstOrDefault(c => c.Name.Equals(contactName, StringComparison.OrdinalIgnoreCase));
                    if (contactToRemove != null)
                    {
                        phonebook.Remove(contactToRemove);
                        Console.WriteLine($"Contact '{contactName}' removed.");
                    }
                    else
                    {
                        Console.WriteLine($"Contact '{contactName}' not found.");
                    }
                    break;

                case "exit":
                    Console.WriteLine("Exiting...");
                    return;
                default:
                    Console.WriteLine("Invalid choice. Please try again.");
                    break;
            }
        }
    }

    static string GetFileName()
    {
        Console.WriteLine("Type name of file: ");
        string fileName = Console.ReadLine();

        return fileName;
    }

    static List<Contact> ImportPhonebookJSON(string fileName)
    {
        List<Contact> phonebook = new List<Contact>();
        try
        {
            StreamReader reader = new StreamReader(fileName);

            string line;
            while ((line = reader.ReadLine()) != null)
            {
                if (!string.IsNullOrEmpty(line))
                {
                    var contact = JsonSerializer.Deserialize<Contact>(line);
                    if (contact != null)
                    {
                        if (contact.ToString() != "Name,PhoneNumber,Email")
                        {
                            phonebook.Add(contact);
                        }
                    }
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error reading file: {ex.Message}");
        }
        return phonebook;
    }
    static List<Contact> ImportPhonebookCSV(string fileName)
    {
        List<Contact> phonebook = new List<Contact>();
        try
        {
            StreamReader reader = new StreamReader(fileName);

            string line;
            while ((line = reader.ReadLine()) != null)
            {
                var parts = line.Split(',');
                if (parts.Length == 3)
                {
                    phonebook.Add(new Contact(parts[0], parts[1], parts[2]));
                }
            }
            reader.Close();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error reading file: {ex.Message}");
        }
        return phonebook;
    }

    static List<Contact> ImportPhonebook(string fileName)
    {
        List<Contact> defaultPhonebook = new List<Contact>() { new Contact("Alice", "123-456-7890", "alice@gmail.com"), new Contact("Bob", "543-543-5432", "bob@gmail.com") };
        if (Path.GetExtension(fileName).Equals(".csv", StringComparison.OrdinalIgnoreCase))
        {
            return ImportPhonebookCSV(fileName);
        }
        else
        {
            if (Path.GetExtension(fileName).Equals(".json", StringComparison.OrdinalIgnoreCase))
            {
                return ImportPhonebookJSON(fileName);
            }
        }
        return defaultPhonebook;
    }

    static void PrintMenu()
    {
        Console.WriteLine("Phonebook menu:");
        Console.WriteLine("1. Export to JSON");
        Console.WriteLine("2. Export to CSV");
        Console.WriteLine("3. Import Phonebook");
        Console.WriteLine("4. Show phonebook");
        Console.WriteLine("5. Add new contact");
        Console.WriteLine("6. Remove contact");
        Console.WriteLine("exit - Exit");
        Console.WriteLine();
        Console.Write("Select an option: ");
    }

    static void ExportPhonebookJSON(List<Contact> phonebook, string name)
    {
        StreamWriter writer = new StreamWriter(name);
        foreach (var contact in phonebook)
        {
            writer.WriteLine(JsonSerializer.Serialize(contact));
        }

        writer.Close();
    }

    static void ExportPhonebookCSV(List<Contact> phonebook, string name)
    {
        StreamWriter writer = new StreamWriter(name);

        foreach (var contact in phonebook)
        {
            if (contact.ToString() != "Name,PhoneNumber,Email")
            {
                writer.WriteLine(contact.ToString());
            }
            else
            {
                writer.WriteLine("Name,PhoneNumber,Email");
            }
        }
        writer.Close();
    }
}

class Contact
{
    public string Name { get; set; }
    public string PhoneNumber { get; set; }
    public string Email { get; set; }
    public Contact(string name, string phoneNumber, string email)
    {
        Name = name;
        PhoneNumber = phoneNumber;
        Email = email;
    }
    public override string ToString()
    {
        return $"{Name},{PhoneNumber},{Email}";
    }
}