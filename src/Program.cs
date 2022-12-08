namespace DNetTutorial;

class Program
{
	static void Main() =>
		new DNetTutorial().MainAsync().GetAwaiter().GetResult();

	private Program() { }
}
