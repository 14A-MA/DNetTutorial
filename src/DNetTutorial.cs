using Discord;
using Discord.Interactions;
using Discord.WebSocket;
using Microsoft.Extensions.DependencyInjection; // NuGet-ről kell telepíteni

namespace DNetTutorial;

public class DNetTutorial
{
	private readonly IServiceProvider services;

	// A kliens, ami a Discord API-val kommunikál
	public DiscordSocketClient Client { get; }
	// A Discord Interactions API-val kommunikáló osztály
	public InteractionService Interaction { get; }

	public DNetTutorial()
	{
		Client = new();
		Interaction = new(Client);

		// Összegyűjtjük a szükséges osztályokat, hogy injektálva lehessenek.
		// Ez azért hasznos, mert így nem kell mindenhol újra példányosítani
		// az osztályokat. Az eltárolt értékek is megmaradnak. A parancsoknál
		// is könnyedén elérhetőek lesznek.
		this.services = new ServiceCollection()
			.AddSingleton(Client)
			.AddSingleton(Interaction)
			.BuildServiceProvider();
	}

	// Innen indul a bot.
	// Az asyncot szokjuk meg, mert a DC API-nak async metódusai vannak.
	public async Task MainAsync()
	{
		Client.Log += Log;

		// !!! Ezt a token-t így ne tedd ki publikusan sehova   !!!
		// !!! Később majd egy config file-ból fogjuk beolvasni !!!
		string token = "MTA1MDMzMzU0NTY3MzAxOTQyMw.GW7TDG.ih1tmOXChzyP4TZOj1Z05o01sdvUDJMPlJOeh8";

		await Client.LoginAsync(TokenType.Bot, token);
		await Client.StartAsync();

		// Minden perjeles parancs használatánál lefut ez a metódus.
		Client.SlashCommandExecuted += async (interaction) =>
		{
			// Itt létrehozunk egy Context-et, amiben eltárol néhány hasznos
			// információt, amit a parancsok használnak/használhatnak.
			SocketInteractionContext context = new(Client, interaction);
			// Majd kegyes egszerűséggel futtatjuk a parancsot.
			await Interaction.ExecuteCommandAsync(context, this.services);
		};

		// Ha a bot elindult, akkor ez a metódus fog lefutni.
		Client.Ready += async () =>
		{
			// Ez a metódus megkeresi az összes osztály a projektünkben, ami
			// örökli a korábban a parancsnál használt ModuleBase osztályt.
			await Interaction.AddModulesAsync(
				typeof(DNetTutorial).Assembly, this.services);

			// Az már ismert modulok segítségével regisztráljuk a létrehozott
			// parancsokat egy általunk kiválasztott szerverre.
			// A true azt jelenti, hogy törli azokat a parancsokat, amiket
			// nem talál meg a modulok között.
			await Interaction.RegisterCommandsToGuildAsync(
				712287958274801695, true);
		};

		await Task.Delay(-1);
	}

	// Log metódus kifejezetten a Discord.NET-hez.
	// Minden fontosabb történést fog logolni formázva.
	private Task Log(LogMessage message)
	{
		Console.WriteLine(message.ToString());
		return Task.CompletedTask;
	}
}
