using MyMonkeyApp.Services;

namespace MyMonkeyApp;

/// <summary>
/// Static helper class for managing monkey data and operations.
/// </summary>
public static class MonkeyHelper
{
    private static readonly List<Monkey> _monkeys = new()
    {
        new Monkey
        {
            Name = "Baboon",
            Species = "Papio",
            Location = "Africa & Arabia",
            Population = 100000,
            Description = "Baboons are some of the world's largest monkeys, known for their distinctive elongated muzzle.",
            ImageUrl = "https://raw.githubusercontent.com/jamesmontemagno/app-monkeys/master/Baboon.jpg"
        },
        new Monkey
        {
            Name = "Capuchin",
            Species = "Cebus",
            Location = "Central & South America",
            Population = 23000,
            Description = "The capuchin monkeys are the most intelligent New World monkeys.",
            ImageUrl = "https://raw.githubusercontent.com/jamesmontemagno/app-monkeys/master/Capuchin.jpg"
        },
        new Monkey
        {
            Name = "Red-shanked Douc",
            Species = "Pygathrix nemaeus",
            Location = "Vietnam",
            Population = 1000,
            Description = "The red-shanked douc is a species of Old World monkey, one of the most colorful primates.",
            ImageUrl = "https://raw.githubusercontent.com/jamesmontemagno/app-monkeys/master/Golden-Headed_Lion_Tamarin.jpg"
        },
        new Monkey
        {
            Name = "Japanese Macaque",
            Species = "Macaca fuscata",
            Location = "Japan",
            Population = 100000,
            Description = "The Japanese macaque, also known as the snow monkey, is a terrestrial Old World monkey species.",
            ImageUrl = "https://raw.githubusercontent.com/jamesmontemagno/app-monkeys/master/Japanese_Macaque.jpg"
        },
        new Monkey
        {
            Name = "Mandrill",
            Species = "Mandrillus sphinx",
            Location = "Central Africa",
            Population = 4000,
            Description = "The mandrill is one of the most colorful mammals in the world, with red and blue facial markings.",
            ImageUrl = "https://raw.githubusercontent.com/jamesmontemagno/app-monkeys/master/Mandrill.jpg"
        },
        new Monkey
        {
            Name = "Proboscis Monkey",
            Species = "Nasalis larvatus",
            Location = "Borneo",
            Population = 7000,
            Description = "The proboscis monkey is known for its unusually large nose, which can exceed 10 cm in length.",
            ImageUrl = "https://raw.githubusercontent.com/jamesmontemagno/app-monkeys/master/Proboscis_Monkey.jpg"
        },
        new Monkey
        {
            Name = "Spider Monkey",
            Species = "Ateles",
            Location = "Central & South America",
            Population = 25000,
            Description = "Spider monkeys are New World monkeys known for their extremely long limbs and prehensile tails.",
            ImageUrl = "https://raw.githubusercontent.com/jamesmontemagno/app-monkeys/master/Spider_Monkey.jpg"
        },
        new Monkey
        {
            Name = "Squirrel Monkey",
            Species = "Saimiri",
            Location = "Central & South America",
            Population = 100000,
            Description = "Squirrel monkeys are small New World monkeys, known for their distinctive coloring and active nature.",
            ImageUrl = "https://raw.githubusercontent.com/jamesmontemagno/app-monkeys/master/Squirrel_Monkey.jpg"
        },
        new Monkey
        {
            Name = "Howler Monkey",
            Species = "Alouatta",
            Location = "Central & South America",
            Population = 15000,
            Description = "Howler monkeys are among the largest New World monkeys and are famous for their loud howls.",
            ImageUrl = "https://raw.githubusercontent.com/jamesmontemagno/app-monkeys/master/Howler_Monkey.jpg"
        },
        new Monkey
        {
            Name = "Golden Lion Tamarin",
            Species = "Leontopithecus rosalia",
            Location = "Brazil",
            Population = 3200,
            Description = "The golden lion tamarin is a small New World monkey with striking reddish-orange pelage.",
            ImageUrl = "https://raw.githubusercontent.com/jamesmontemagno/app-monkeys/master/Golden-Headed_Lion_Tamarin.jpg"
        }
    };

    /// <summary>
    /// Gets all available monkeys in the collection.
    /// </summary>
    /// <returns>A read-only list of all monkeys.</returns>
    public static IReadOnlyList<Monkey> GetAllMonkeys()
    {
        return _monkeys.AsReadOnly();
    }

    /// <summary>
    /// Finds a monkey by name (case-insensitive search).
    /// </summary>
    /// <param name="name">The name of the monkey to find.</param>
    /// <returns>The monkey if found; otherwise, null.</returns>
    public static Monkey? FindMonkeyByName(string name)
    {
        if (string.IsNullOrWhiteSpace(name))
        {
            return null;
        }

        return _monkeys.FirstOrDefault(m => 
            m.Name.Equals(name, StringComparison.OrdinalIgnoreCase));
    }

    /// <summary>
    /// Gets a random monkey from the collection.
    /// </summary>
    /// <returns>A randomly selected monkey.</returns>
    public static Monkey GetRandomMonkey()
    {
        var index = Random.Shared.Next(_monkeys.Count);
        return _monkeys[index];
    }

    /// <summary>
    /// Retrieves monkeys from the MonkeyMCP server and displays them in a formatted table.
    /// </summary>
    public static async Task DisplayMonkeysFromMcpInTableAsync()
    {
        try
        {
            var mcpMonkeys = await GetMonkeysFromMcpAsync();
            
            if (!mcpMonkeys.Any())
            {
                Console.WriteLine("❌ No monkeys retrieved from MCP server.");
                return;
            }

            // Sort monkeys alphabetically by name
            var sortedMonkeys = mcpMonkeys.OrderBy(m => m.Name).ToList();

            DisplayMonkeyTable(sortedMonkeys);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"❌ Error retrieving monkeys from MCP: {ex.Message}");
        }
    }

    /// <summary>
    /// Retrieves monkeys from the MonkeyMCP server using the MCP service.
    /// </summary>
    /// <returns>A list of monkeys from the MCP server.</returns>
    private static async Task<List<Services.McpMonkey>> GetMonkeysFromMcpAsync()
    {
        using var mcpService = new MonkeyMcpService();
        return await mcpService.GetAllMonkeysAsync();
    }

    /// <summary>
    /// Retrieves a specific monkey by name from the MonkeyMCP server.
    /// </summary>
    /// <param name="name">The name of the monkey to find.</param>
    /// <returns>The monkey if found; otherwise, null.</returns>
    public static async Task<Services.McpMonkey?> GetMonkeyByNameFromMcpAsync(string name)
    {
        using var mcpService = new MonkeyMcpService();
        return await mcpService.GetMonkeyByNameAsync(name);
    }

    /// <summary>
    /// Lists all monkeys from the MonkeyMCP server with details.
    /// </summary>
    public static async Task ListMonkeysFromMcpAsync()
    {
        Console.WriteLine("═══════════════════════════════════════════════════════════");
        Console.WriteLine("              MONKEYS FROM MCP SERVER                     ");
        Console.WriteLine("═══════════════════════════════════════════════════════════\n");

        try
        {
            var mcpMonkeys = await GetMonkeysFromMcpAsync();
            
            if (mcpMonkeys.Any())
            {
                foreach (var monkey in mcpMonkeys.OrderBy(m => m.Name))
                {
                    DisplayMcpMonkeyDetails(monkey);
                    Console.WriteLine();
                }

                Console.WriteLine($"📊 Total monkeys from MCP server: {mcpMonkeys.Count}");
                Console.WriteLine($"🌍 Total population: {mcpMonkeys.Sum(m => m.Population):N0}");
            }
            else
            {
                Console.WriteLine("❌ No monkeys retrieved from MCP server or server is unavailable.");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"❌ Error connecting to MCP server: {ex.Message}");
        }
    }

    /// <summary>
    /// Gets a combined list of local and MCP monkeys.
    /// </summary>
    /// <returns>A combined list of all monkeys.</returns>
    public static async Task ListAllMonkeysIncludingMcpAsync()
    {
        Console.WriteLine("═══════════════════════════════════════════════════════════");
        Console.WriteLine("           ALL MONKEYS (LOCAL + MCP)                      ");
        Console.WriteLine("═══════════════════════════════════════════════════════════\n");

        try
        {
            // Get local monkeys
            var localMonkeys = GetAllMonkeys();
            
            // Get MCP monkeys
            var mcpMonkeys = await GetMonkeysFromMcpAsync();

            Console.WriteLine("🏠 LOCAL MONKEYS:");
            Console.WriteLine("─────────────────");
            foreach (var monkey in localMonkeys)
            {
                MonkeyHelper.DisplayMonkeyDetails(monkey);
                Console.WriteLine();
            }

            Console.WriteLine("\n🌐 MCP SERVER MONKEYS:");
            Console.WriteLine("──────────────────────");
            foreach (var monkey in mcpMonkeys.OrderBy(m => m.Name))
            {
                DisplayMcpMonkeyDetails(monkey);
                Console.WriteLine();
            }

            Console.WriteLine($"📊 Total local monkeys: {localMonkeys.Count}");
            Console.WriteLine($"📊 Total MCP monkeys: {mcpMonkeys.Count}");
            Console.WriteLine($"📊 Grand total: {localMonkeys.Count + mcpMonkeys.Count}");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"❌ Error retrieving monkeys: {ex.Message}");
        }
    }

    /// <summary>
    /// Displays a formatted table of monkeys.
    /// </summary>
    /// <param name="monkeys">The list of monkeys to display.</param>
    private static void DisplayMonkeyTable(List<Services.McpMonkey> monkeys)
    {
        Console.WriteLine("═══════════════════════════════════════════════════════════════════════════════════════════════");
        Console.WriteLine("                                    MONKEYS FROM MCP SERVER                                    ");
        Console.WriteLine("═══════════════════════════════════════════════════════════════════════════════════════════════");
        Console.WriteLine();

        // Calculate column widths
        var nameWidth = Math.Max(20, monkeys.Max(m => m.Name.Length) + 2);
        var locationWidth = Math.Max(25, monkeys.Max(m => m.Location.Length) + 2);
        var populationWidth = 12;
        
        // Print header
        var headerLine = $"│ {"Name".PadRight(nameWidth)} │ {"Location".PadRight(locationWidth)} │ {"Population".PadLeft(populationWidth)} │";
        var separatorLine = "├" + new string('─', nameWidth + 2) + "├" + new string('─', locationWidth + 2) + "├" + new string('─', populationWidth + 2) + "┤";
        var topLine = "┌" + new string('─', nameWidth + 2) + "┬" + new string('─', locationWidth + 2) + "┬" + new string('─', populationWidth + 2) + "┐";
        var bottomLine = "└" + new string('─', nameWidth + 2) + "┴" + new string('─', locationWidth + 2) + "┴" + new string('─', populationWidth + 2) + "┘";

        Console.WriteLine(topLine);
        Console.WriteLine(headerLine);
        Console.WriteLine(separatorLine);

        // Print monkey data
        foreach (var monkey in monkeys)
        {
            var populationFormatted = monkey.Population.ToString("N0");
            var dataLine = $"│ {monkey.Name.PadRight(nameWidth)} │ {monkey.Location.PadRight(locationWidth)} │ {populationFormatted.PadLeft(populationWidth)} │";
            Console.WriteLine(dataLine);
        }

        Console.WriteLine(bottomLine);
        Console.WriteLine();
        Console.WriteLine($"📊 Total monkeys: {monkeys.Count}");
        Console.WriteLine($"🌍 Total population: {monkeys.Sum(m => m.Population):N0}");
    }

    /// <summary>
    /// Displays detailed information about a monkey from MCP server.
    /// </summary>
    /// <param name="monkey">The MCP monkey to display.</param>
    private static void DisplayMcpMonkeyDetails(Services.McpMonkey monkey)
    {
        Console.WriteLine($"🐒 {monkey.Name}");
        Console.WriteLine($"   📍 Location: {monkey.Location}");
        Console.WriteLine($"   👥 Population: {monkey.Population:N0}");
        Console.WriteLine($"   📝 Details: {monkey.Details}");
        if (!string.IsNullOrEmpty(monkey.Image))
        {
            Console.WriteLine($"   🖼️  Image: {monkey.Image}");
        }
    }

    /// <summary>
    /// Displays detailed information about a local monkey.
    /// </summary>
    /// <param name="monkey">The monkey to display.</param>
    public static void DisplayMonkeyDetails(Monkey monkey)
    {
        Console.WriteLine($"🐵 Name:        {monkey.Name}");
        Console.WriteLine($"🧬 Species:     {monkey.Species}");
        Console.WriteLine($"🌍 Location:    {monkey.Location}");
        Console.WriteLine($"👥 Population:  {monkey.Population:N0}");
        Console.WriteLine($"📝 Description: {monkey.Description}");
        Console.WriteLine($"🖼️  Image URL:   {monkey.ImageUrl}");
        Console.WriteLine("───────────────────────────────────────────────────────────");
    }
}


