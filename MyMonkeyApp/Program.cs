using MyMonkeyApp;

// Display welcome banner with ASCII art
DisplayWelcomeBanner();

// Main application loop
bool isRunning = true;
while (isRunning)
{
    DisplayMenu();
    var choice = Console.ReadLine()?.Trim();

    Console.WriteLine();

    switch (choice)
    {
        case "1":
            ListAllMonkeys();
            break;
        case "2":
            FindMonkeyByName();
            break;
        case "3":
            GetRandomMonkey();
            break;
        case "4":
            isRunning = false;
            Console.WriteLine("Thanks for visiting! See you later! 🐒");
            break;
        default:
            Console.WriteLine("❌ Invalid option. Please enter a number between 1 and 4.");
            break;
    }

    if (isRunning)
    {
        Console.WriteLine("\nPress Enter to continue...");
        Console.ReadLine();
        Console.Clear();
        DisplayWelcomeBanner();
    }
}

/// <summary>
/// Displays the welcome banner with ASCII art.
/// </summary>
static void DisplayWelcomeBanner()
{
    Console.WriteLine(@"
╔══════════════════════════════════════════════════════════════╗
║                                                              ║
║          🐵  WELCOME TO THE MONKEY DATABASE!  🐒            ║
║                                                              ║
║                    .-""-._.-""-._.-""-._                      ║
║                  ,'   ,'   ,'   ,'   `.                     ║
║                 /    /    /    /      /                     ║
║                :    :    :    :      :                      ║
║                 \    \    \    \      \                     ║
║                  `.   `.   `.   `.   ,'                     ║
║                    `-._,-._,-._,-._,'                       ║
║                                                              ║
║           Your gateway to fascinating monkey facts!         ║
║                                                              ║
╚══════════════════════════════════════════════════════════════╝
");
}

/// <summary>
/// Displays the main menu options.
/// </summary>
static void DisplayMenu()
{
    Console.WriteLine(@"
┌──────────────────────────────────────┐
│           MAIN MENU                  │
├──────────────────────────────────────┤
│  1. 📋 List all monkeys              │
│  2. 🔍 Find monkey by name           │
│  3. 🎲 Get random monkey             │
│  4. 🚪 Exit                          │
└──────────────────────────────────────┘
");
    Console.Write("Enter your choice (1-4): ");
}

/// <summary>
/// Lists all available monkeys with their details.
/// </summary>
static void ListAllMonkeys()
{
    Console.WriteLine("═══════════════════════════════════════════════════════════");
    Console.WriteLine("                    ALL MONKEYS                           ");
    Console.WriteLine("═══════════════════════════════════════════════════════════\n");

    var monkeys = MonkeyHelper.GetAllMonkeys();
    
    foreach (var monkey in monkeys)
    {
        DisplayMonkeyDetails(monkey);
        Console.WriteLine();
    }

    Console.WriteLine($"Total monkeys in database: {monkeys.Count}");
}

/// <summary>
/// Prompts user for a monkey name and displays its details if found.
/// </summary>
static void FindMonkeyByName()
{
    Console.Write("Enter the monkey name to search: ");
    var name = Console.ReadLine()?.Trim();

    if (string.IsNullOrWhiteSpace(name))
    {
        Console.WriteLine("❌ Please enter a valid monkey name.");
        return;
    }

    Console.WriteLine("\n🔍 Searching for monkey...\n");

    var monkey = MonkeyHelper.FindMonkeyByName(name);

    if (monkey != null)
    {
        Console.WriteLine("✅ Monkey found!\n");
        Console.WriteLine("═══════════════════════════════════════════════════════════");
        DisplayMonkeyDetails(monkey);
    }
    else
    {
        Console.WriteLine($"❌ No monkey found with the name '{name}'.");
        Console.WriteLine("\n💡 Tip: Try one of these names:");
        
        var allMonkeys = MonkeyHelper.GetAllMonkeys();
        foreach (var m in allMonkeys.Take(5))
        {
            Console.WriteLine($"   • {m.Name}");
        }
        if (allMonkeys.Count > 5)
        {
            Console.WriteLine($"   ... and {allMonkeys.Count - 5} more!");
        }
    }
}

/// <summary>
/// Displays details of a randomly selected monkey.
/// </summary>
static void GetRandomMonkey()
{
    Console.WriteLine("🎲 Picking a random monkey...\n");
    
    var monkey = MonkeyHelper.GetRandomMonkey();
    
    Console.WriteLine("═══════════════════════════════════════════════════════════");
    Console.WriteLine("              YOUR RANDOM MONKEY IS...                    ");
    Console.WriteLine("═══════════════════════════════════════════════════════════\n");
    
    DisplayMonkeyDetails(monkey);
}

/// <summary>
/// Displays detailed information about a specific monkey.
/// </summary>
/// <param name="monkey">The monkey to display.</param>
static void DisplayMonkeyDetails(Monkey monkey)
{
    Console.WriteLine($"🐵 Name:        {monkey.Name}");
    Console.WriteLine($"🧬 Species:     {monkey.Species}");
    Console.WriteLine($"🌍 Location:    {monkey.Location}");
    Console.WriteLine($"👥 Population:  {monkey.Population:N0}");
    Console.WriteLine($"📝 Description: {monkey.Description}");
    Console.WriteLine($"🖼️  Image URL:   {monkey.ImageUrl}");
    Console.WriteLine("───────────────────────────────────────────────────────────");
}
