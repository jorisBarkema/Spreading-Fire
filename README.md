# Customizable Spreading Fire
Making an object burn in Unity is easy with particle systems. In real life however, fire spreads. This project allows for easy customizable spreading fire in any game. The scripts work for stationary and moving burning objects, objects with or without physics. In this guide I will explain how to use this in your own projects.

## Demo
Included assets in the demo:
| Asset                               | Source                                                            |
| ----------------------------------- |:-----------------------------------------------------------------:|
| Player with animations              | [Kenney](https://www.kenney.nl/assets/animated-characters-2)      |
| Trees and log                       | [Kenney](https://www.kenney.nl/assets/nature-kit)                 |
| Fire particle system                | [Sirhaian'Arts](https://www.youtube.com/watch?v=5Mw6NpSEb2o)      |
| Smoke and Dissolve particle systems | Me                                                                |

In the demo some simple prefabs have been included to check that the project has been imported successfully. If everything works correctly, one of the logs should start smoking when the game is started, and the fire should spread to all of the other objects. (including the player if you don’t move with arrow keys/awsd)

## Setup
To get this working in your own project, create an empty object and attach the **FireManager** script to it, or attach it to for example some other GameManager gameobject. **There can only be one firemanager per scene.** Customise the **Burn Distance** of the firemanager to fit how fast you want the fire to spread, the correct value here largely depends on the scale of your gameobjects. Then simply add prefabs with a **FireScript** to the scene, it’s that simple!

![alt text](https://github.com/jorisBarkema/Spreading-Fire/blob/master/FireManagerInspector.png "Fire Manager in inspector")

## Creating a burnable prefab
Any gameobject with a **MeshRenderer** or **SkinnedMeshRenderer** with a collider or multiple sub-colliders can become burnable. First attach a **FireScript** to the gameobject. You will see that there are three particle systems which need to be assigned a particle system.

![alt text](https://github.com/jorisBarkema/Spreading-Fire/blob/master/FireScriptInspector.png "Fire Script in inspector")

Create your own particle systems that you want to play, or use the ones from the demo. Tips for creating the particle systems:

1. If your mesh is animated, be sure to use a **SkinnedMeshRenderer** in the Shape module of the particle system.
2. The size of the particles has to be very small, especially for skinned mesh renderers. You can use the particle systems in the demo as a guide, but the particle systems can be whatever you want them to be, doesn't even have to be fire.
3. The fire particle system in the demo uses four sub-particle systems. If you want to use this as a starting point for your own fire, you need to change the particle size.emission rate/shape etc. of all four sub-systems, **not** the one parent system!

Once the particle systems are assigned, there are some other options which can be changed. By default, the script will start smoking when near fire, but if **Smoke On Start** is toggled it will start smoking on the start of the scene. It will smoke for **SmokeDuration** seconds, then it will burn for **FireDuration** seconds, then finally dissolve. If **Dissolvable** is turned off, it will keep burning forever and not dissolve.

## Usage of the burning objects
Fire and smoke can be stopped manually by calling the **StopBurning** method of the firescript. The status of the gameobject can be accessed by the public bools **Smoking** and **Burning**. The scripts can be edited to perform special actions for example if the player starts smoking or burning, or you can use **GetComponent<FireScript>** to use this status in your own scripts.

## License
You are free to use and adapt this project however you want in your own projects, also in commercial projects. Attribution is appreciated but not required. If you notice a bug, you can make a new issue on Github, and if you appreciate this project, please consider becoming a donor on Github!
