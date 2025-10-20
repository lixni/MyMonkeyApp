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
}
