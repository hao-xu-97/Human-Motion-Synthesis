using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine;



public class ProcessParameter : MonoBehaviour {

    public Dropdown resoDD;
    public Dropdown frameDD;

    private string resolution;
    private string framerate;

    void Awake()
    {
        DontDestroyOnLoad(this.gameObject);
    }

    public void onClick()
    {
        resolution = "_" + resoDD.captionText.text;
        framerate = "_" + frameDD.captionText.text;
        SceneManager.LoadScene(1, LoadSceneMode.Single);

    }

    void OnEnable()
    {
        Debug.Log("OnEnable called");
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.name != "intro")
        {
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

    void OnDisable()
    {
        Debug.Log("OnDisable");
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

}
