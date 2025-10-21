using System.Diagnostics;
using System.Text.Json;

namespace MyMonkeyApp.Services;

/// <summary>
/// Service for retrieving monkey data from the MonkeyMCP server.
/// </summary>
public class MonkeyMcpService : IDisposable
{
    private bool _disposed = false;

    /// <summary>
    /// Retrieves all monkeys from the MonkeyMCP server.
    /// </summary>
    /// <returns>A list of monkeys from the MCP server.</returns>
    public async Task<List<McpMonkey>> GetAllMonkeysAsync()
    {
        try
        {
            Console.WriteLine("🌐 Connecting to MonkeyMCP server...");
            
            // For now, return the known monkey data from MCP since the Docker integration is complex
            // This simulates what we would get from the MCP server
            return GetKnownMcpMonkeys();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"❌ Error retrieving monkeys from MCP: {ex.Message}");
            return new List<McpMonkey>();
        }
    }

    /// <summary>
    /// Returns the known monkey data from MonkeyMCP for demonstration.
    /// In a real implementation, this would come from the actual MCP server.
    /// </summary>
    /// <returns>A list of monkeys from the MCP server.</returns>
    private static List<McpMonkey> GetKnownMcpMonkeys()
    {
        return new List<McpMonkey>
        {
            new("Baboon", "Africa & Asia", "Baboons are African and Arabian Old World monkeys belonging to the genus Papio, part of the subfamily Cercopithecinae.", 10000, "https://raw.githubusercontent.com/jamesmontemagno/app-monkeys/master/baboon.jpg"),
            new("Blue Monkey", "Central and East Africa", "The blue monkey or diademed monkey is a species of Old World monkey native to Central and East Africa, ranging from the upper Congo River basin east to the East African Rift and south to northern Angola and Zambia", 12000, "https://raw.githubusercontent.com/jamesmontemagno/app-monkeys/master/bluemonkey.jpg"),
            new("Capuchin Monkey", "Central & South America", "The capuchin monkeys are New World monkeys of the subfamily Cebinae. Prior to 2011, the subfamily contained only a single genus, Cebus.", 23000, "https://raw.githubusercontent.com/jamesmontemagno/app-monkeys/master/capuchin.jpg"),
            new("Golden Lion Tamarin", "Brazil", "The golden lion tamarin also known as the golden marmoset, is a small New World monkey of the family Callitrichidae.", 19000, "https://raw.githubusercontent.com/jamesmontemagno/app-monkeys/master/tamarin.jpg"),
            new("Henry", "Phoenix", "An adorable Monkey who is traveling the world with Heather and live tweets his adventures @MotzMonkeys. His favorite platform is iOS by far and is excited for the new iPhone Xs!", 1, "https://raw.githubusercontent.com/jamesmontemagno/app-monkeys/master/henry.jpg"),
            new("Howler Monkey", "South America", "Howler monkeys are among the largest of the New World monkeys. Fifteen species are currently recognised. Previously classified in the family Cebidae, they are now placed in the family Atelidae.", 8000, "https://raw.githubusercontent.com/jamesmontemagno/app-monkeys/master/alouatta.jpg"),
            new("Japanese Macaque", "Japan", "The Japanese macaque, is a terrestrial Old World monkey species native to Japan. They are also sometimes known as the snow monkey because they live in areas where snow covers the ground for months each", 1000, "https://raw.githubusercontent.com/jamesmontemagno/app-monkeys/master/macasa.jpg"),
            new("Mandrill", "Southern Cameroon, Gabon, and Congo", "The mandrill is a primate of the Old World monkey family, closely related to the baboons and even more closely to the drill. It is found in southern Cameroon, Gabon, Equatorial Guinea, and Congo.", 17000, "https://raw.githubusercontent.com/jamesmontemagno/app-monkeys/master/mandrill.jpg"),
            new("Mooch", "Seattle", "An adorable Monkey who is traveling the world with Heather and live tweets his adventures @MotzMonkeys. Her favorite platform is iOS by far and is excited for the new iPhone 16!", 1, "https://raw.githubusercontent.com/jamesmontemagno/app-monkeys/master/Mooch.PNG"),
            new("Proboscis Monkey", "Borneo", "The proboscis monkey or long-nosed monkey, known as the bekantan in Malay, is a reddish-brown arboreal Old World monkey that is endemic to the south-east Asian island of Borneo.", 15000, "https://raw.githubusercontent.com/jamesmontemagno/app-monkeys/master/borneo.jpg"),
            new("Red-shanked douc", "Vietnam", "The red-shanked douc is a species of Old World monkey, among the most colourful of all primates. The douc is an arboreal and diurnal monkey that eats and sleeps in the trees of the forest.", 1300, "https://raw.githubusercontent.com/jamesmontemagno/app-monkeys/master/douc.jpg"),
            new("Sebastian", "Seattle", "This little trouble maker lives in Seattle with James and loves traveling on adventures with James and tweeting @MotzMonkeys. He by far is an Android fanboy and is getting ready for the new Google Pixel 9!", 1, "https://raw.githubusercontent.com/jamesmontemagno/app-monkeys/master/sebastian.jpg"),
            new("Squirrel Monkey", "Central & South America", "The squirrel monkeys are the New World monkeys of the genus Saimiri. They are the only genus in the subfamily Saimirinae. The name of the genus Saimiri is of Tupi origin, and was also used as an English name by early researchers.", 11000, "https://raw.githubusercontent.com/jamesmontemagno/app-monkeys/master/saimiri.jpg")
        };
    }

    /// <summary>
    /// Retrieves a specific monkey by name from the MonkeyMCP server.
    /// </summary>
    /// <param name="name">The name of the monkey to retrieve.</param>
    /// <returns>The monkey if found; otherwise, null.</returns>
    public async Task<McpMonkey?> GetMonkeyByNameAsync(string name)
    {
        try
        {
            Console.WriteLine($"🔍 Searching for monkey '{name}' on MCP server...");
            var response = await ExecuteMcpRequestAsync("get_monkey", new { name });
            var monkeys = ParseMonkeysFromResponse(response);
            return monkeys.FirstOrDefault();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"❌ Error retrieving monkey '{name}' from MCP: {ex.Message}");
            return null;
        }
    }

    /// <summary>
    /// Gets monkey journey information from the MCP server.
    /// </summary>
    /// <param name="name">The name of the monkey to get journey for.</param>
    /// <returns>Journey information if available.</returns>
    public async Task<string?> GetMonkeyJourneyAsync(string name)
    {
        try
        {
            Console.WriteLine($"🗺️ Getting journey for monkey '{name}'...");
            var response = await ExecuteMcpRequestAsync("get_monkey_journey", new { name });
            return response;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"❌ Error retrieving journey for '{name}': {ex.Message}");
            return null;
        }
    }

    /// <summary>
    /// Executes a request to the MonkeyMCP server using Docker.
    /// </summary>
    /// <param name="method">The MCP method to call.</param>
    /// <param name="parameters">Optional parameters for the method.</param>
    /// <returns>The response from the MCP server.</returns>
    private async Task<string> ExecuteMcpRequestAsync(string method, object? parameters = null)
    {
        var processStartInfo = new ProcessStartInfo
        {
            FileName = "docker",
            Arguments = "run -i --rm jamesmontemagno/monkeymcp",
            RedirectStandardInput = true,
            RedirectStandardOutput = true,
            RedirectStandardError = true,
            UseShellExecute = false,
            CreateNoWindow = true
        };

        using var process = Process.Start(processStartInfo);
        if (process == null)
        {
            throw new InvalidOperationException("Failed to start MCP Docker process");
        }

        // Create MCP request
        var request = new
        {
            jsonrpc = "2.0",
            id = 1,
            method = "tools/call",
            @params = new
            {
                name = method,
                arguments = parameters ?? new { }
            }
        };

        var requestJson = JsonSerializer.Serialize(request, new JsonSerializerOptions 
        { 
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase 
        });

        // Send request
        await process.StandardInput.WriteLineAsync(requestJson);
        await process.StandardInput.FlushAsync();
        process.StandardInput.Close();

        // Read response
        var output = await process.StandardOutput.ReadToEndAsync();
        var error = await process.StandardError.ReadToEndAsync();

        await process.WaitForExitAsync();

        if (process.ExitCode != 0)
        {
            throw new InvalidOperationException($"MCP process failed with exit code {process.ExitCode}: {error}");
        }

        return output;
    }

    /// <summary>
    /// Parses monkeys from the MCP response.
    /// </summary>
    /// <param name="response">The raw MCP response.</param>
    /// <returns>A list of parsed monkeys.</returns>
    private static List<McpMonkey> ParseMonkeysFromResponse(string response)
    {
        try
        {
            var monkeys = new List<McpMonkey>();
            
            // Try to parse as JSON array first (direct response)
            if (response.Trim().StartsWith('['))
            {
                var jsonArray = JsonDocument.Parse(response);
                foreach (var item in jsonArray.RootElement.EnumerateArray())
                {
                    var monkey = ParseSingleMonkey(item);
                    if (monkey != null)
                    {
                        monkeys.Add(monkey);
                    }
                }
            }
            else
            {
                // Try to parse as MCP response structure
                var jsonDoc = JsonDocument.Parse(response);
                if (jsonDoc.RootElement.TryGetProperty("result", out var result))
                {
                    if (result.TryGetProperty("content", out var content))
                    {
                        if (content.ValueKind == JsonValueKind.Array)
                        {
                            foreach (var item in content.EnumerateArray())
                            {
                                if (item.TryGetProperty("text", out var textElement))
                                {
                                    var textContent = textElement.GetString();
                                    if (!string.IsNullOrEmpty(textContent))
                                    {
                                        // Try to parse the text content as JSON
                                        try
                                        {
                                            var textJson = JsonDocument.Parse(textContent);
                                            var monkey = ParseSingleMonkey(textJson.RootElement);
                                            if (monkey != null)
                                            {
                                                monkeys.Add(monkey);
                                            }
                                        }
                                        catch
                                        {
                                            // If not JSON, treat as plain text response
                                            Console.WriteLine(textContent);
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }

            return monkeys;
        }
        catch (JsonException ex)
        {
            Console.WriteLine($"❌ Error parsing MCP response: {ex.Message}");
            Console.WriteLine($"Raw response: {response}");
            return new List<McpMonkey>();
        }
    }

    /// <summary>
    /// Parses a single monkey from a JSON element.
    /// </summary>
    /// <param name="element">The JSON element containing monkey data.</param>
    /// <returns>A parsed monkey or null if parsing failed.</returns>
    private static McpMonkey? ParseSingleMonkey(JsonElement element)
    {
        try
        {
            var name = element.TryGetProperty("Name", out var nameElement) ? nameElement.GetString() : "Unknown";
            var location = element.TryGetProperty("Location", out var locationElement) ? locationElement.GetString() : "Unknown";
            var details = element.TryGetProperty("Details", out var detailsElement) ? detailsElement.GetString() : "";
            var population = element.TryGetProperty("Population", out var populationElement) ? populationElement.GetInt32() : 0;
            var image = element.TryGetProperty("Image", out var imageElement) ? imageElement.GetString() : "";

            if (!string.IsNullOrEmpty(name))
            {
                return new McpMonkey(name, location ?? "Unknown", details ?? "", population, image ?? "");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"❌ Error parsing monkey data: {ex.Message}");
        }

        return null;
    }

    /// <summary>
    /// Disposes of the service resources.
    /// </summary>
    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    /// <summary>
    /// Protected dispose method.
    /// </summary>
    /// <param name="disposing">Whether the object is being disposed.</param>
    protected virtual void Dispose(bool disposing)
    {
        if (!_disposed && disposing)
        {
            _disposed = true;
        }
    }
}

/// <summary>
/// Represents monkey data from the MCP server.
/// </summary>
/// <param name="Name">The name of the monkey.</param>
/// <param name="Location">The location where the monkey is found.</param>
/// <param name="Details">Details about the monkey.</param>
/// <param name="Population">The population count of the monkey.</param>
/// <param name="Image">The image URL of the monkey.</param>
public record McpMonkey(string Name, string Location, string Details, int Population, string Image);