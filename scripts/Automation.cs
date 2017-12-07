//FILE:     Automation.cs
//AUTHOR:   Hao Xu (haoxunico@gmail.com)
//DATE:     11/1/2017
//PURPOSE: Generate videos of characters animated with motion captured data.   
//INFO: Differences in characters, camera angle, lighting, and scene for each motion

using System;
using System.IO;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace RockVR.Video
{
    public class Automation : MonoBehaviour
    {

        /*change this for difference in light settings
         *add more values to array for different light angles
         *(y component of rotation for directional light)
         */
        private int[] lights = { 0 , 180 };

        private float timeLeft;
        private UnityEngine.Object[] clips;
        private List<GameObject> characters;
        public int cameras = 3;
		public int clipCounter = 0;
        public int cameraCounter = 0;
        public int characterCounter = 0;
        public int lightCounter = 0;

        private Animator animator;

        private int charLoc;

        private Vector3 initialPos;
        private Quaternion initialRot;

        private void Awake()
        {
            Application.runInBackground = true;
        }

        // Use this for initialization
        void Start()
        {
            //get all clips in Resource folder
            clips = Resources.LoadAll("", typeof(AnimationClip));
            charLoc = clips[clipCounter].name.IndexOf('|');
            
            //get all characters
            GameObject character = GameObject.Find("characters");
            initialPos = character.transform.position;
            initialRot = character.transform.rotation;
            Transform[] allChildren = character.GetComponentsInChildren<Transform>();
            characters = new List<GameObject>();
            int index = 0;
            //find all direct child of the characters GameObject
            foreach (Transform child in allChildren)
            {
                if (child.parent == character.transform)
                {
                    characters.Add(child.gameObject);
                    index++;
                }
            }
            //make all characters invisible
            for (int i = 0; i < characters.Count; i++)
            {
                characters[i].SetActive(false);
            }
            if (cameraCounter > 0)
            {
                GetComponentInParent<VideoCaptureCtrl>().videoCaptures[0] = GameObject.Find("DedicatedCapture (" + cameraCounter + ")").GetComponent<VideoCapture>();

            }
            if (lightCounter > 0)
            {
                Transform myTransform = GameObject.Find("Directional light").transform;
                //add the difference in light values to the y rotational coordinate of the directional light
                Vector3 rot = myTransform.rotation.eulerAngles;
                rot = new Vector3(rot.x, rot.y + lights[lightCounter] - lights[0], rot.z);
                myTransform.rotation = Quaternion.Euler(rot);
            }
            characters[characterCounter].SetActive(true);
            Init(characterCounter);
        }

        //initialize character
        //get the animation controller of that character
        void Init(int c)
        {
            animator = characters[c].GetComponent<Animator>();
            RuntimeAnimatorController myController = animator.runtimeAnimatorController;

            AnimatorOverrideController myAnimatorOverride = new AnimatorOverrideController();
            myAnimatorOverride.runtimeAnimatorController = myController;

            animator.runtimeAnimatorController = myAnimatorOverride;
            SwitchAnimation();
        }

        // Update is called once per frame
        void Update()
        {
            timeLeft -= Time.deltaTime;
            //when an animation is done
            if (timeLeft < 0)
            {
                //wait until the video rendering is done and rename it
                if (VideoCaptureCtrl.instance.status == VideoCaptureCtrl.StatusType.STARTED)
                {
                    VideoCaptureCtrl.instance.StopCapture();
                    while (VideoCaptureCtrl.instance.status != VideoCaptureCtrl.StatusType.FINISH)
                    {

                    }
                    //wait one second for the video and audio files to merge
                    //this number is arbitrary, feel free to experiment with the value here
                    System.Threading.Thread.Sleep(1000);
                    rename();
                    clipCounter++;
                }
                //go to next animation if it exist
                if (clipCounter < clips.Length)
                {
                    SwitchAnimation();
                }
                else
                {
                    cameraCounter++;
                    //go to next camera if it exist
                    if (cameraCounter < cameras)
                    {
                        clipCounter = 0;
                        //find the camera with the name DedicatedCapture numbered camera counter
                        GetComponentInParent<VideoCaptureCtrl>().videoCaptures[0] = GameObject.Find("DedicatedCapture (" + cameraCounter + ")").GetComponent<VideoCapture>();
                        SwitchAnimation();
                    }
                    else
                    {
                        lightCounter++;
                        //go to next light if it exist
                        if (lightCounter < lights.Length)
                        {
                            clipCounter = 0;
                            cameraCounter = 0;
                            GetComponentInParent<VideoCaptureCtrl>().videoCaptures[0] = GameObject.Find("DedicatedCapture").GetComponent<VideoCapture>();                           
                            Transform myTransform = GameObject.Find("Directional light").transform;
                            //add the difference in light values to the y rotational coordinate of the directional light
                            Vector3 rot = myTransform.rotation.eulerAngles;
                            rot = new Vector3(rot.x, rot.y+lights[lightCounter]-lights[lightCounter-1], rot.z);
                            myTransform.rotation = Quaternion.Euler(rot);
                        }
                        else{
                            characterCounter++;
                            //go to next character if it exist
                            if (characterCounter < characters.Count)
                            {
                                clipCounter = 0;
                                cameraCounter = 0;
                                Transform myTransform = GameObject.Find("Directional light").transform;
                                Vector3 rot = myTransform.rotation.eulerAngles;
                                rot = new Vector3(rot.x, rot.y+360-lights[lightCounter-1], rot.z);
                                lightCounter = 0;
                                myTransform.rotation = Quaternion.Euler(rot);
                                GetComponentInParent<VideoCaptureCtrl>().videoCaptures[0] = GameObject.Find("DedicatedCapture").GetComponent<VideoCapture>();
                                characters[characterCounter - 1].SetActive(false);
                                characters[characterCounter].SetActive(true);
                                Init(characterCounter);
                            }
                            else
                            {
                                //switch to the next scene if all combination is done for this scene
                                GameObject handler = GameObject.Find("handler");
                                ProcessParameter pp = handler.GetComponent<ProcessParameter>();
                                pp.finished = true;
                            }
                        }
                    }
                }
            }
        }


        //Replace an animation clip in the animator controller with the next clip in the array
        //Most of this code is copied from unity forums
        //devs say there might be updates in this section so look out for it
        void SwitchAnimation()
        {
            //----------------
            //Debug.Log(clips.Length);
            //Debug.Log(clips[clipCounter].name);
            
            //reset character position after each clip
            characters[characterCounter].transform.position = initialPos;
            characters[characterCounter].transform.rotation = initialRot;


            timeLeft = ((AnimationClip)clips[clipCounter]).length/5;
            AnimatorOverrideController myCurrentOverrideController = animator.runtimeAnimatorController as AnimatorOverrideController;


            RuntimeAnimatorController myOriginalController = myCurrentOverrideController.runtimeAnimatorController;

            // Know issue: Disconnect the orignal controller first otherwise when you will delete this override it will send a callback to the animator
            // to reset the SM
            myCurrentOverrideController.runtimeAnimatorController = null;

            AnimatorOverrideController myNewOverrideController = new AnimatorOverrideController();
            myNewOverrideController.runtimeAnimatorController = myOriginalController;

            myNewOverrideController[myNewOverrideController.animationClips[0].name] = (AnimationClip)clips[clipCounter];
            animator.runtimeAnimatorController = myNewOverrideController;

            UnityEngine.Object.Destroy(myCurrentOverrideController);

            VideoCaptureCtrl.instance.StartCapture();
        }


        //method that finds an unnamed file (start with a number) and name it according to the it's description
        //currently get all files in folder and find one that start with a number to rename it
        //could use some optimization
        void rename()
        {
            string path = "C:/Users/ISL-WORKSTATION/Documents/RockVR/Video/";
            var files = Directory.GetFiles(path).OrderBy(f => f);
            foreach(var file in files){
                string fileName = Path.GetFileName(file);
                if (Char.IsNumber(fileName[0])){
                    charLoc = clips[clipCounter].name.IndexOf('|');
                    string s = "_" + clips[clipCounter].name.Substring(charLoc + 1) + "_" + characters[characterCounter].name + "_" + SceneManager.GetActiveScene().name + "_camera" + cameraCounter + "_light" + lightCounter + ".mp4";
                    Debug.Log(s);
                    Debug.Log(file);
                    File.Move(file, path + s);
                }
            }
        }


    }
}
