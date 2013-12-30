Yna Engine
==========

### Status

The engine is currently in rewriting phase, please be careful because the current API can be broken in the future. I'm working on the 2D engine for now, and after on 3D engine.

### What is Yna Engine ?

Yna is a lightweight 2D and 3D game engine using MonoGame Framework. 
The aim of this project is to give to the developer the ability to create games in 2D or 3D easily on multiple platforms such as Windows Phone, Windows 8, or Linux. 
Yna is not a complex engine compared to others but it suitable for all developers who want an easy way to quickly create a prototype or a game.

### Platforms

Yna Engine is currently support many platforms
* Windows Desktop and Modern UI (DirectX 11 / OpenGL / SDL2)
* Windows Phone 7 & 8 (XNA)
* Linux & Mac OSX (OpenGL / SDL2)

Do you want to see it working on another platform ? Fork it and send us a pull request.

### Sample 2D

```C#
public class AnimatedSprites : YnState2D
{
	private YnSprite background;
	private YnSprite playerSprite;

	public AnimatedSprites(string name)
		: base(name)
	{
		// Background
		background = new YnSprite("Scene/GreenGround");
		Add(background);

		// Player
		playerSprite = new YnSprite("Sprites/BRivera-malesoldier");
		spriteGroup.Add(playerSprite);
	}

	public override void Initialize()
	{
		base.Initialize();
		background.SetFullscreen();
		
		// Move the player and add a physics component
		playerSprite.Move(350, 350);
		playerSprite.AddComponent<SpritePhysics>().ForceInsideScreen = true;

		// Add animations
		var animator = playerSprite.AddComponent<SpriteAnimator>();
		animator.PrepareAnimation(64, 64);
		animator.AddAnimation("up", 0, 8, 25, false);
		animator.AddAnimation("left", 9, 17, 25, false);
		animator.AddAnimation("down", 18, 26, 25, false);
		animator.AddAnimation("right", 27, 35, 25, false);
	}

	public override void Update(GameTime gameTime)
	{
		base.Update(gameTime);

		var animator = playerSprite.GetComponent<SpriteAnimator>();
		
		// Move the player
		if (YnG.Keys.Up)
		{
			playerSprite.Y -= 2;
			animator.Play("up");
		}
		
		// Etc.
	}
}
```

### Contributors
* Lead developer : Yannick Comte (@CYannick)
* Contributor : Alex Frêne (aka @Drakulo)
* Logo & graphics : Thomas Ruffier

### License
Microsoft public license