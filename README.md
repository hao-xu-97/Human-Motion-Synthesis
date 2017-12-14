# Human motion synthesis using Unity3D

## Prerequisite:
Software: amc2bvh.exe, Unity 2017, Blender. <br />
Unity: RockVR (Video Capture), scenes, character models
Files: <br />
  Motion files: amc, asf or bvh formats. <br />
  Character models: fbx format. <br />

## Procedure
0. If motion files in amc/asf format, run amc2bvh.exe to convert them to bvh
1. Place all bvh files into "Desktop/New folder/bvh" (or modify script)
2. Open Blender and run the bvh2fbx.py script. It will convert the motion files to fbx format which Unity can process and place them under the unity "Resources/Input"<sup>[1]</sup>
3. Find the imported motion file in Unity and change its Animation Type to Humanoid under Rig. Check to make sure the model is mapped properly.
4. Configure the different variations to record video (characters, camera angle, scene, lighting)
    1. For characters, add<sup>[2]</sup> or remove from the "characters" GameObject in Unity Editor for the ones desired. For new character added to the scene, add the "New Animation Controller"<sup>[3]</sup> in Asset to the character's controller in the "Animator" section.
    2. For camera, change the position of the DedicatedCapture GameObjects to the desired location. Add additional DedicatedCapture GameObjects for more angle. Read the documentation for RockVR Video Capture for more detail.
    3. For scene, check the desired scenes within the intro scene and run.
    4. For lighting, change the "lights" parameter in Automation.cs script. Add more values to the array for more variations in lighting angles.
5. Start up the "intro" scene and run it from Unity Editor. Click "Start" button to start the problem.
6. Adjust the desired resolution and framerate and click start. For initial run, leave all the counters to 0. For continuing runs enter the counters where the previous run left off. The videos will be recorded to "Documents/RockVR/Video"<sup>[4]</sup>

### Note
* [1] Converting too many bvh files at a time may result in Blender crashing. Try converting them in batches of smaller quantity (~50).
* [2] To add a GameObject to a Scene in Unity, drag it from the Asset menu to a position in the Hierarchy menu or a position in the scene itself. You can also create an empty GameObject from the "GameObject->Create Empty" option.
* [3] Depending on the framerate of the motion files, you may need to adjust the speed of the animation. To do this go to "Assets" and find the "New Animator Controller" and open it. Then click on "New State" and adjust the speed to framerate/24 (if 120 frames changes to 5, if 60 change to 2.5, etc).
Also find the line "timeLeft = ((AnimationClip)clips[clipCounter]).length;" in the SwitchAnimation function and divide it by the speed.
* [4] Unity will most likely freeze or crash if left running for too long. Adjust the counters in the "intro" scene to resume progress.

### Scene Creation procedure
0. To get a scene, either download a pre-built one or build one yourself using various 3d models for GameObjects.
1. Create an empty GameObject named "characters" and place it at a location best suited for recording. Add a character to it to see if any adjusting or scaling is needed.
2. Add DedicatedCapture GameObjects from the "RockVR/Video/Prefabs" folder to the scene in desired locations.
3. Attach the AudioCapture script in "RockVR/Video/Scripts" folder to the main camera.
4. Create an empty GameObject named "VideoCaptureCtrl" and attach the VideoCaptureCtrl script in "RockVR/Video/Scripts" to it. Also attach the Automation.cs script from "Scripts" to it as well.
5. Add the first DedicatedCapture GameObject as well as the AudioCapture to the the VideoCaptureCtrl script.
6. If there is no "Directional light" GameObject, create one.
7. Add the created scene to build settings.
8. Add a check box in the intro scene for the newly created scene and modify the scene "ProcessParameter" accordingly.

### Additional characters
In the "characters" folder in Assets, there is a list of preprocessed characters I got from the Unity asset store for free. <br />
To process new characters: <br />
1. Change its Animation type to Humanoid under Rig
2. Fix any mapping problem for the bones of the character
3. Remove the mapping on the bones for both hands. This could be done using the "New Human Template" in the Assets folder. (This is to avoid weird finger mapping from the animations)

## Instructions on error handling
* If you tried to terminate the program insider the Unity Editor, the ffmpeg.exe will still be running and result in unfinished video and audio files to remain in the videos folder. To solve this issue, simply terminate the ffmpeg.exe from task manager and delete the unfinished files.
* Since the program freezes fairly often, a temporary save state feature is implemented. Once Unity froze, terminate it from task manager. Look into the videos folder and figure out what combination the next video should be. Enter the parameters where the last run left off in the "intro" scene (various counters) to pick up from there.

## Local environment specs
* OS: Microsoft Windows 10 Pro
* Version: 10.0.16299 Build 16299
* Processor:	Intel(R) Xeon(R) CPU E5-2630 v4 @ 2.20GHz, 2201 Mhz, 10 Core(s), 20 Logical Processor(s)
* Total Physical Memory:	63.9 GB
* GPU:	NVIDIA Quadro M5000
