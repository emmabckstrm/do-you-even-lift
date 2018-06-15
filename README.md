# Do You Even Lift? - A VR puzzle game
### Introduction
This is a room scale VR puzzle game originally built for Emma Bäckström's master thesis project during spring 2018. The thesis project investigated how limitations in the lifting velocity affected the perception of heaviness of the virtual object, and an interaction method was developed with the aim of limiting the user's lifting velocity. This game is built around that interaction method.
### The interaction method
The interaction method tries to simulate weight without using any haptic technologies. This is done by setting a velocity limit to the objects, and when they are lifted with a velocity exceeding that limit, the objects are disconnected from the controller. In other words, the player needs to lift heavy objects slowly.
### How to play
The rules are simple. You are trapped in a tower and need to escape by solving a puzzle on each level. All puzzles are solved by activating all the buttons on that level. The player has to use wooden barrels of different weights to place on the buttons.
### FAQ
- **Why are the barrels dropping from the controller? Is it a bug?**
  No, it's not a bug. It's a feature, I promise. The fact that the barrels are dropping from the controller is the core functionality of the interaction method that tries to simulate weight.

- **I'm super annoyed about the barrels dropping, what should I do?**

  Lift the barrels slowly.
- **How do I win?**

  You finish every level until you have escaped the tower (finish the fifth level and you will fall out of the tower).
- **I have pressed all the buttons but I'm not reaching the next level, what is going on?**

  Check again. Make sure all buttons are *actually* activated (fully pressed down and have changed colour), and that you have activated *all* buttons on the level. Sometimes people forget to look behind themselves ;)
- **I fell to the next level and now I'm bouncing up and down and the floor is below the physical floor?!**
  Congratulations, you found a bug. You'll need to restart the game. This bug usually occurs when you stand too close to the edge of the playing area which makes you land on top of an object on the floor below. So try to stand more in the middle of the playing area!
- **I won. Now what?**

  I know the UX is not perfect, but the game will bring you back to lobby 8 seconds after you fallen out of the tower!
### TODO
- Display time after finish
- Reset level button in game
- Local highscore
### Technical stuff
Built with Unity 2017.3.0f3 for HTC Vive.
### Credits
- [Virtual Reality Toolkit](https://github.com/thestonefox/VRTK)
- [Cartoon temple building kit](https://assetstore.unity.com/packages/3d/environments/dungeons/cartoon-temple-building-kit-110397)
- [Grass Road Race](https://assetstore.unity.com/packages/3d/environments/roadways/grass-road-race-46974)
- [Old Props Pack](https://assetstore.unity.com/packages/3d/olde-props-pack-1-pbr-legacy-32221)
