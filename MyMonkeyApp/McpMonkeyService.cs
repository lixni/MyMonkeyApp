using System.Diagnostics;
using System.Text.Json;

namespace MyMonkeyApp;

/// <summary>
/// Service for retrieving monkey data from the MonkeyMCP server.
/// </summary>
public class McpMonkeyService : IDisposable
{
    private Process? _mcpProcess;
    private bool _disposed = false;

    /// <summary>
    /// Retrieves a list of monkeys from the MonkeyMCP server.
    /// </summary>
    /// <returns>A list of monkeys from the MCP server.</returns>
    public async Task<List<Monkey>> GetMonkeysFromMcpAsync()
    {
        try
        {
            var mcpMonkeys = await ExecuteMcpCommandAsync("list_monkeys");
            return ConvertMcpMonkeysToLocalFormat(mcpMonkeys);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"❌ Error retrieving monkeys from MCP: {ex.Message}");
            return new List<Monkey>();
        }
    }

    /// <summary>
    /// Retrieves a specific monkey by name from the MonkeyMCP server.
    /// </summary>
    /// <param name="name">The name of the monkey to retrieve.</param>
    /// <returns>The monkey if found; otherwise, null.</returns>
    public async Task<Monkey?> GetMonkeyByNameFromMcpAsync(string name)
    {
        try
        {
            var mcpResponse = await ExecuteMcpCommandAsync($"get_monkey/{name}");
            if (mcpResponse.Any())
            {
                return ConvertMcpMonkeyToLocalFormat(mcpResponse.First());
            }
            return null;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"❌ Error retrieving monkey '{name}' from MCP: {ex.Message}");
            return null;
        }
    }

    /// <summary>
    /// Executes a command against the MonkeyMCP server.
    /// </summary>
    /// <param name="command">The command to execute.</param>
    /// <returns>A list of monkey data from the MCP response.</returns>
    private async Task<List<McpMonkey>> ExecuteMcpCommandAsync(string command)
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
            throw new InvalidOperationException("Failed to start MCP process");
        }

        // Send the command to the MCP server
        await process.StandardInput.WriteLineAsync(CreateMcpRequest(command));
        await process.StandardInput.FlushAsync();
        process.StandardInput.Close();

        // Read the response
        var output = await process.StandardOutput.ReadToEndAsync();
        var error = await process.StandardError.ReadToEndAsync();

        await process.WaitForExitAsync();

        if (process.ExitCode != 0)
        {
            throw new InvalidOperationException($"MCP process failed with exit code {process.ExitCode}: {error}");
        }

        return ParseMcpResponse(output);
    }

    /// <summary>
    /// Creates an MCP request message.
    /// </summary>
    /// <param name="command">The command to include in the request.</param>
    /// <returns>A JSON string representing the MCP request.</returns>
    private static string CreateMcpRequest(string command)
    {
        var request = new
        {
            jsonrpc = "2.0",
            id = 1,
            method = "tools/call",
            @params = new
            {
                name = command,
                arguments = new { }
            }
        };

        return JsonSerializer.Serialize(request);
    }

    /// <summary>
    /// Parses the MCP response and extracts monkey data.
    /// </summary>
    /// <param name="response">The raw response from the MCP server.</param>
    /// <returns>A list of monkey data.</returns>
    private static List<McpMonkey> ParseMcpResponse(string response)
    {
        try
        {
            using var jsonDoc = JsonDocument.Parse(response);
            var result = jsonDoc.RootElement.GetProperty("result");
            var content = result.GetProperty("content");

            var monkeys = new List<McpMonkey>();
            
            if (content.ValueKind == JsonValueKind.Array)
            {
                foreach (var item in content.EnumerateArray())
                {
                    if (item.TryGetProperty("text", out var textElement))
                    {
                        var monkeyData = JsonSerializer.Deserialize<McpMonkey>(textElement.GetString() ?? "{}");
                        if (monkeyData != null)
                        {
                            monkeys.Add(monkeyData);
                        }
                    }
                }
            }

            return monkeys;
        }
        catch (JsonException ex)
        {
            throw new InvalidOperationException($"Failed to parse MCP response: {ex.Message}");
        }
    }

    /// <summary>
    /// Converts a list of MCP monkeys to the local Monkey format.
    /// </summary>
    /// <param name="mcpMonkeys">The MCP monkey data.</param>
    /// <returns>A list of local Monkey objects.</returns>
    private static List<Monkey> ConvertMcpMonkeysToLocalFormat(List<McpMonkey> mcpMonkeys)
    {
        return mcpMonkeys.Select(ConvertMcpMonkeyToLocalFormat).ToList();
    }

    /// <summary>
    /// Converts an MCP monkey to the local Monkey format.
    /// </summary>
    /// <param name="mcpMonkey">The MCP monkey data.</param>
    /// <returns>A local Monkey object.</returns>
    private static Monkey ConvertMcpMonkeyToLocalFormat(McpMonkey mcpMonkey)
    {
        return new Monkey
        {
            Name = mcpMonkey.Name ?? string.Empty,
            Species = ExtractSpeciesFromName(mcpMonkey.Name),
            Location = mcpMonkey.Location ?? string.Empty,
            Population = mcpMonkey.Population,
            Description = mcpMonkey.Details ?? string.Empty,
            ImageUrl = mcpMonkey.Image ?? string.Empty
        };
    }

    /// <summary>
    /// Extracts species information from the monkey name.
    /// </summary>
    /// <param name="name">The monkey name.</param>
    /// <returns>A species name or the original name if no mapping exists.</returns>
    private static string ExtractSpeciesFromName(string? name)
    {
        // Simple mapping based on common knowledge
        return name?.ToLowerInvariant() switch
        {
            "baboon" => "Papio",
            "capuchin" => "Cebus",
            "macaque" => "Macaca",
            "mandrill" => "Mandrillus sphinx",
            "tamarin" => "Saguinus",
            _ => name ?? string.Empty
        };
    }

    /// <summary>
    /// Disposes of the MCP service resources.
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
            _mcpProcess?.Dispose();
            _disposed = true;
        }
    }

    /// <summary>
    /// Represents monkey data from the MCP server.
    /// </summary>
    private class McpMonkey
    {
        public string? Name { get; set; }
        public string? Location { get; set; }
        public string? Details { get; set; }
        public string? Image { get; set; }
        public int Population { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
    }
}