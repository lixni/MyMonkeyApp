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
            await MonkeyHelper.DisplayMonkeysFromMcpInTableAsync();
            break;
        case "5":
            await MonkeyHelper.ListMonkeysFromMcpAsync();
            break;
        case "6":
            await MonkeyHelper.ListAllMonkeysIncludingMcpAsync();
            break;
        case "7":
            isRunning = false;
            Console.WriteLine("Thanks for visiting! See you later! 🐒");
            break;
        default:
            Console.WriteLine("❌ Invalid option. Please enter a number between 1 and 7.");
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
│  1. 📋 List all monkeys (local)      │
│  2. 🔍 Find monkey by name           │
│  3. 🎲 Get random monkey             │
│  4. 📊 Show MCP monkeys table        │
│  5. 🌐 List monkeys from MCP         │
│  6. 📋 List all monkeys (local+MCP)  │
│  7. 🚪 Exit                          │
└──────────────────────────────────────┘
");
    Console.Write("Enter your choice (1-7): ");
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
        MonkeyHelper.DisplayMonkeyDetails(monkey);
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
        MonkeyHelper.DisplayMonkeyDetails(monkey);
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
    
    MonkeyHelper.DisplayMonkeyDetails(monkey);
}




