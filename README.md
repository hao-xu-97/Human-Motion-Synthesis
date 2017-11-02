#Human motion synthesis using Unity3D

## Prerequisite:
Software: amc2bvh.exe, Unity 2017<br />
Files: <br />
  Motion files: amc, asf or bvh formats. <br />
  Character models: fbx format. <br />

##Procedure
0. If motion files in amc/asf format, run amc2bvh.exe to convert them to bvh
1. Place all bvh files into "Desktop/New folder/bvh"
2. Open Blender and run the bvh2fbx.py script. It will convert the motion files to fbx format which Unity can process and place them under the unity "Resources/Input"<sup>[1]</sup>
3. Find the imported motion file in Unity and change its Animation Type to Humanoid under Rig. Check to make sure the model is mapped properly.
4. Configure the different variations to record video (characters, camera angle, scene, lighting)
    1. For characters, add<sup>[2]</sup> or remove from the "charcters" GameObject in Unity Editor for the ones desired. For new character added to the scene, add the "New Animation Controller" in Asset to the character's controller in the "Animator" section.
    2. For camera, change the position of the DedicatedCapture GameObjects to the desired location. Add additional DedicatedCapture GameObjects for more angle. Read the documentation for RockVR Video Capture for more detail.
    3. For scene, configure the list of scenes to use and their order in the SwitchScene.cs script. To add more scenes, create a new scene or find preexisting scenes in the Unity asset store and read the scene creation procedure below.
    4. For lighting, change the "lights" parameter in Automation.cs script. Add more values to the array for more variations in lighting angles.
5. Start up the "intro" scene and run it from Unity Editor
6. Adjust the desired resolution and framerate and click start. The videos will be recorded to "Documents/RockVR/Video"<sup>[3]</sup>

###Note
* [1] Converting too many bvh files at a time may result in Blender crahsing. Try converting them in batches of smaller quantity (~50).
* [2] To add a GameObject to a Scene in Unity, drag it from the Asset menu to a position in the Hierarchy menu or a position in the scene itself.
* [3] Unity will most likely freeze or crash if left running for too long. Try running on batches of small quantity of motions if that's the case.

###Scene Creation procedure
0. To get a scene, either download a pre-built one or build one yourself using various 3d models for GameObjects.
1. Create an empty GameObject named "characters" and place it at a location best suited for recording. Add a character to it to see if any adjusting or scaling is needed.
2. Add DedicatedCapture GameObjects from the "RockVR/Video/Prefabs" folder to the scene in desired locations.
3. Attach the AudioCapture script in "RockVR/Video/Scripts" folder to the main camera.
4. Create an empty GameObject named "VideoCaptureCtrl" and attach the VideoCaptureCtrl script in "RockVR/Video/Scripts" to it. Also attach the Automation.cs and SwitchScene.cs scripts from "Scripts" to it as well.
5. If there is no "Directional light" GameObject, create one.

###Additional characters
In the "characters" folder in Assets, there is a list of preprocessed characters I got from the Unity asset store for free. <br />
To process new characters: <br />
1. Change its Animation type to Humanoid under Rig
2. Fix any mapping problem for the bones of the character
3. Remove the mapping on the bones for both hands. This could be done using the "New Human Template" in the Assets folder. (This is to avoid weird finger mapping from the animations)
