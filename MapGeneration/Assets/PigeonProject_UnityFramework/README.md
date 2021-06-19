
***Unity Version: 2020.3f***


# PigeonProject_Unity-Framework

> Base framework for projects in Unity

## Trello

Join the official [Trello Board](https://trello.com/invite/b/NCwEStcS/2b45a4cba2cb9e437d809575097650dd/pigeonpigeonprojectunity-framework) to contribute! There we manage and plan new features and keep track of known bugs.

## Including the framework

- The relevant scripts are located in the folder **PigeonProject**. It contains functionality for all framework features.
- Include the **PigeonProject** folder inside the Assets folder and you should be good to go.
- Most of the objects that need to be instantiated inside the project can be added via the Create menu.

## Features

### Event System

- A *Scriptable Object* based Event System.
- Create **Game Event** instances in the project.
- Attach **Game Event Listener** components to Game Objects and connect responses.

### Runtime Sets

- *Scriptable Object* based Runtime sets which track active Objects.
- A generic version exists though it is advised to use the specific **Game Object Set** implementation.
- Create **Game Object Set** instances in the project.
- Attach **Game Object Set Registrar** components to Game Objects.
  - The Registrar handles adding and removing the object from the set.
  - It also acts as a decorator to access the set.

### Serialized Variables

- *Scriptable Object* based variables that get serialized by the Unity Engine.
- This allows for easier access to shared data by referencing the variable instance.
- Specific implementations for *float*, *int* and *string* variables exist.

### Reference Dictionaries

- *Scriptable Object* that implements *IDictionary*.
- Allows for implementations of serialized dictionaries.
- No custom editor yet.

### Scene Handling

- Static Scene Changer class for switching between scenes.
- Scene Handler Component that gets accessed by the Scene Changer and receives Initialize/Terminate calls.
- Static Scene Safe class for storing information between Scenes.

### Hitbox

- Static Hitbox class for creating and checking temporary hitboxes for interactive abilities.

### Camera System

- A fully dynamic camera system
- Offers creation of camera perspectives directly from the scene view
- Possibility to create complex camera behaviours via code
- Trauma based camera shake

### Butler Push

- A *Scriptable Object* based pipeline for creating builds and automatic pushing to itch.io.
  - NOTE: Requires access to the *ButlerPush.bat* and *7z.exe* files which should be located in the solution directory or a *resource* folder in the solution directory.
- Create a **ButlerBuild** instance and provide neccessary settings for the Butler
  - *Butler Settings*: Required information for pushing to itch.io.
  - *Scenes*: Provide the scens to be included in the build.
  - *Build Infos*: Provide the Build Infos which give the build pipeline the neccessary information to create builds.
  - *Butler Push button*: Create builds of all provided versions and push them to itch.io.
- Create **Build Info** instances with settings for the build targets.
  - You need to add the Build Info instances to the **Butler Build** instance.

### Utility

- **Perry Debug Logger**
  - An advanced Debug Logger class that features log messages on several mutable layers.
- **Gizmo Drawer**
  - A debug class that can draw gizmos in Eitor mode.
  - Used by the hitbox class to visualize hit areas.
- **MathUtil**
  - A static utility class that offers additional math functionality

## Resources

- https://unity.com/how-to/architect-game-code-scriptable-objects
- https://gamedevelopment.tutsplus.com/articles/unity-now-youre-thinking-with-components--gamedev-12492
