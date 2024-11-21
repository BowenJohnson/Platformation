# Platformation

Platformation is a procedurally generated platformer game. There are 2 game modes, story and survival, that run on the same basic concept. As the player progresses through the game random platform pieces or 'tiles' are grabbed at random from a pregenerated list of tiles. Each of these tiles contains a handful of spawn points for other game objects such as monsters, items, or hazards. All of these game objects are instantiated dynamically by randomly selecting each game object from its corrisponding list when the tile is instantiated to create a unique gaming experience each play through.

Player survival mode game data is stored remotely on Firebase and used to populate the scoreboard which can also be seen by each player via the survival mode menu after singing up. There is currently no email verification in place so players can sign up with a mock email address. No personal or private data is ever recorded. The only player data recorded and sent to the Firebase database are the player's score information from survival mode such as how many tiles traveled, monsters killed, and items collected.

## Game Modes

Story mode has a linear level progression with checkpoints, unlimited player respawning, and a boss monster at the end of a few levels. 

Survival mode allows the player to try and see how far they can go before their character dies. After the character dies the player's best high score results are recorded into the database and displayed on the scoreboard.

## Download

You can find a working, playable, build of the game zipped in the 'Game_Builds' folder. Download and extract Platformation.zip then run SP.exe to start the game. How-to-play instructions are also included in the zip.