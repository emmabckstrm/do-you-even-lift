# Do You Even Lift? - A VR puzzle game
### Introduction
This is a room scale VR puzzle game originally built for Emma Bäckström's master thesis project during spring 2018. The thesis project investigated how limitations in the lifting velocity affected the perception of heaviness of the virtual object, and an interaction method was developed with the aim of limiting the user's lifting velocity. This game is built around that interaction method.
### The interaction method
The interaction method tries to simulate weight without using any haptic technologies. This is done by setting a velocity limit to the objects, and when they are lifted with a velocity exceeding that limit, the objects are disconnected from the controller. In other words, the player needs to lift heavy objects slowly.
### How to play
The rules are simple. You are trapped in a tower and need to escape by solving a puzzle on each level. All puzzles are solved by activating all the buttons on that level. The player has to use wooden barrels of different weights to place on the buttons. 
### FAQ
- Why are the barrels dropping from the controller? Is it a bug?

  No, it's not a bug. It's a feature, I promise. The fact that the barrels are dropping from the controller is the core functionality of the interaction method that tries to simulate weight.
  
- I'm super annoyed about the barrels dropping, what should I do?
  
  Lift the barrels slowly.

### Technical stuff
Built with Unity 2017.3.0f3
