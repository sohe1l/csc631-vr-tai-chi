Level Design Set 1 - Wooden Walls v2
Created by Hristo Ivanov (Also known as Kris Development)

Instructions:
Inside the package there is a Models folder which contains a set.fbx model which combines all the meshes.
In the Prefabs folder there is a prefab called set. It contains meshes of which walls are made. The meshes are controlled by a script called WallSelector.js.
The following prefabs are also included: barrel, cobweb, plank, leaves, crg_metal, beam, floor.
You can choose to create your own prefabs from the set.fbx or use the pre-made prefabs.

How To Use WallSelector.cs:
Script Name - a STRING field which can be used to set a name for the specific component.
Assets - an array of type GAMEOBJECT which contains all wanted meshes.
Selector - an array of type ASSETSELECTOR (custom private class inside WallSelector.cs) which contains the combinations between the meshes.
Gizmo (Color, Dimension, Offset) - use to select how the prefab is displayed in your Scene view.
Selected style via the buttons;
When done, uncheck EDIT OBJ so the script doesn't call any new updates.