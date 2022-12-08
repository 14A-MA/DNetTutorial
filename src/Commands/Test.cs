using Discord;
using Discord.Interactions;
using Discord.WebSocket;

namespace DNetTutorial.Commands;

// Az InteractionModuleBase-ből örökölünk, hogy a Discord.Interactions-ben
// lévő metódusokat használhassuk.
[RequireOwner] // Így az összes al-parancsot csak a tulajdonos használhatja
[Group("test", "Teszt parancsok")] // (név, leírás)
public class Test : InteractionModuleBase
{
	// Ha létrehozunk egy publikusan elérhető property-t, akkor azt a DI
	// automatikusan beinjektálja a fő osztályunkban lévő property-k
	// alapján, ha a név megegyezik. Ebben a példában a klienst kérjük be,
	// hogy az egyik parancsban kiolvashassunk belőle egy információt.
	public DiscordSocketClient Client { get; set;}

	[SlashCommand("hello-world", "Hello World parancs")]
	public async Task HelloWorld()
	{
		await RespondAsync("Hello World!");
	}

	// A metódus paraméterei a parancsban megadott argumentumok.
	// A Summary attribútummal felülírhatjuk a paraméterek nevét és leírását.
	[SlashCommand("hi", "Üdvözlő parancs")]
	public async Task Hi(
		[Summary(name: "Tag", description: "Akit üdvözölünk")]
		IUser user)
	{
		await RespondAsync($"Hello {user.Mention}!");
	}

	// Most nem számuljuk ki a bot pingjét, hanem csak kipróbáljuk, hogyan
	// lehet megemlíteni az aktuális felhasználót.
	[SlashCommand("ping", "Ping parancs")]
	public async Task Ping()
	{
		// A Context változóban találunk jópár hasznos dolgot.
		await RespondAsync($"Pong! {Context.User.Mention}");
	}

	[SlashCommand("guild-count", "Szerverek száma, ahol a bot bent van")]
	public async Task GuildCount()
	{
		await RespondAsync($"A bot **`{Client.Guilds.Count}`** szerveren van bent.");
	}
}
