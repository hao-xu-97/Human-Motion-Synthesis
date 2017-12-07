//FILE:     Automation.cs
//AUTHOR:   Hao Xu
//DATE:     11/1/2017
//PURPOSE: Set the desired resolution and frame rate for the recorded videos
//      Also sets the desired scenes to run
//Info: Add to this script for more parameters in the future

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine;



public class ProcessParameter : MonoBehaviour {

    public Dropdown resoDD;
    public Dropdown frameDD;
    public Toggle town;
    public Toggle terrain;
    public Toggle castle;
    public Toggle scifi;
    public Toggle island;
    public InputField clipF;
    public InputField camF;
    public InputField charF;
    public InputField lightF;

    public bool finished = false;

    private bool[] scenes;
    private int sceneCounter = -1;
    private string resolution;
    private string framerate;

    private int clipC;
    private int camC;
    private int charC;
    private int lightC;

    private bool initial = true;
    //this object will be kept when the scenes switch
    void Awake()
    {
        DontDestroyOnLoad(this.gameObject);
        scenes = new bool[5];
    }

    //record the values in the dropdown menu and record which scenes to load
    public void onClick()
    {
        resolution = "_" + resoDD.captionText.text;
        framerate = "_" + frameDD.captionText.text;
        scenes[0] = town.isOn;
        scenes[1] = terrain.isOn;
        scenes[2] = castle.isOn;
        scenes[3] = scifi.isOn;
        scenes[4] = island.isOn;
        clipC = Int32.Parse(clipF.text);
        camC = Int32.Parse(camF.text);
        charC = Int32.Parse(charF.text);
        lightC = Int32.Parse(lightF.text);

        SceneManager.LoadScene(findNext(), LoadSceneMode.Single);

    }

    void OnEnable()
    {
        Debug.Log("OnEnable called");
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    //when a scene is loaded
    //currently changes the parameters for the camera objects to record at the desired setting
    //look in the VideoCaptureBase script in RockVR for detailed info
    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.name != "intro")
        {
            if (initial)
            {
                RockVR.Video.Automation au = GameObject.Find("VideoCaptureCtrl").GetComponent<RockVR.Video.Automation>();
                au.clipCounter = clipC;
                au.cameraCounter = camC;
                au.characterCounter = charC;
                au.lightCounter = lightC;
                initial = false;
            }
            Debug.Log(scene.name);
            Debug.Log(resolution);
            Debug.Log(framerate);
            GameObject dc0 = GameObject.Find("DedicatedCapture");
            GameObject dc1 = GameObject.Find("DedicatedCapture (1)");
            GameObject dc2 = GameObject.Find("DedicatedCapture (2)");
            RockVR.Video.VideoCaptureBase vcb0 = dc0.GetComponent<RockVR.Video.VideoCaptureBase>();
            vcb0.frameSize = (RockVR.Video.VideoCaptureBase.FrameSizeType)Enum.Parse(
                typeof(RockVR.Video.VideoCaptureBase.FrameSizeType), resolution);
            vcb0._targetFramerate = (RockVR.Video.VideoCaptureBase.TargetFramerateType)Enum.Parse(
                typeof(RockVR.Video.VideoCaptureBase.TargetFramerateType), framerate);
            RockVR.Video.VideoCaptureBase vcb1 = dc1.GetComponent<RockVR.Video.VideoCaptureBase>();
            vcb1.frameSize = (RockVR.Video.VideoCaptureBase.FrameSizeType)Enum.Parse(
                typeof(RockVR.Video.VideoCaptureBase.FrameSizeType), resolution);
            vcb1._targetFramerate = (RockVR.Video.VideoCaptureBase.TargetFramerateType)Enum.Parse(
                typeof(RockVR.Video.VideoCaptureBase.TargetFramerateType), framerate);
            RockVR.Video.VideoCaptureBase vcb2 = dc2.GetComponent<RockVR.Video.VideoCaptureBase>();
            vcb2.frameSize = (RockVR.Video.VideoCaptureBase.FrameSizeType)Enum.Parse(
                typeof(RockVR.Video.VideoCaptureBase.FrameSizeType), resolution);
            vcb2._targetFramerate = (RockVR.Video.VideoCaptureBase.TargetFramerateType)Enum.Parse(
                typeof(RockVR.Video.VideoCaptureBase.TargetFramerateType), framerate);
        }
    }

    //check if a scene is done and load the next scene
    void Update()
    {
        if (finished)
        {
            SceneManager.LoadScene(findNext(), LoadSceneMode.Single);
            GameObject.Destroy(GameObject.Find("VideoCaptureCtrl"));
            finished = false;
        }
    }

    //find the next scene to load
    int findNext()
    {
        int index = 5;
        for (int i = sceneCounter+1; i < 5; i++)
        {
            if (scenes[i])
            {
                index = i;
                sceneCounter = i;
                break;
            }
        }
        if (index == 5)
        {
            UnityEditor.EditorApplication.isPlaying = false;
            Application.Quit();
        }
        return index+1;
    }

    void OnDisable()
    {
        Debug.Log("OnDisable");
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

}
