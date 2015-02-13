The Corgi Engine is a complete Unity 2D platformer engine.
Basically it'll allow you to create your own platformer game. 
It comes complete with lots of assets, animations, particle effects, etc...



WHAT's IN THE CORGI ENGINE ?
----------------------------

The Corgi Engine contains quite a lot of stuff. 
Everything is in the Assets folder, and in it you'll find the following folders :

- animations : contains all the animations in the game

- Editor : Unity Editor stuff, touch at your own risk

- fonts : the fonts used in the GUI

- materials : all the materials and associated sprites used in the particle effects

- materials-physics : physics materials you can use in your game (bouncy, icy, etc...)

- prefabs : all the game's prefabs. You just have to drag one of these in your scene and they'll be ready
  - effects : particle effects (rocket trail, gun shot...
  - enemies : robots, cactus, buzzsaw. 
  - environment : grass and trees, already set on the right layer
  - GUI : gui stuff, already included in the scenes
  - items : health packs, coins
  - level managers : stuff like checkpoints, level bounds, start, restarter
  - platforms : pre-created platforms. You can easily create your own, all they need is a collider2D
    it can be a poly, box, circle, whatever you like.
  - projectiles : these are the stuff shot by the player's gun, or its enemies.
  - player : your player. You just have to drag it on a new scene and you're good to go

- scenes : a start screen and two demo levels (from the start screen press A on an xbox controller, or space on your keyboard)

- scripts : all the game scripts. This includes camera scripts, character controllers, enemy AI, health management, ladders, jumpers, level management, etc... It's all sorted for you in the right folders.

- sprites : all the sprites and spritesheets used in the game. Feel free to reuse them in your own game.


WHAT AM I SUPPOSED TO DO WITH IT ?
----------------------------------

The Corgi Engine includes everything you need to start your own platformer game.
You can either start fresh and pick the scripts you need from it, or modify the existing demo levels brick by brick, adding your own sprites and scripts as you go.

MOBILE INPUT
------------

You can activate / desactivate mobile inputs via the menu in your editor's top bar (the bar with File, Edit, etc...).
There's a "Mobile Input" entry there, just enable or disable it. Disabling it will hide all mobile inputs.
It should also automatically enable or disable it depending on your target platform when building.


