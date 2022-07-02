using DungeonCrawler;
using SFML.Graphics;
using SFML.Window;

// Set a random seed
var random = new Random(Seed: 42);

// Create the main game object.
RenderWindow window =
    new(mode: VideoMode.DesktopMode, title: "Dungeon Crawler", style: Styles.Fullscreen);

Game game = new(window: window);

// game.Initialize();
// game.Run();

Console.ReadLine();