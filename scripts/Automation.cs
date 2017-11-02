//FILE:     Automation.cs
//AUTHOR:   Hao Xu
//DATE:     11/1/2017
//REVISION:
//REVISION DATE:
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
        private int[] lights = { 0 };

        private float timeLeft;
        private UnityEngine.Object[] clips;
        private List<GameObject> characters;
        public int cameras = 3;
        private int clipCounter = 0;
        private int cameraCounter = 0;
        private int characterCounter = 0;
        private int lightCounter = 0;

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
            clips = Resources.LoadAll("", typeof(AnimationClip));
            charLoc = clips[0].name.IndexOf('|');
            //AnimationClip[] clips = Resources.FindObjectsOfTypeAll<AnimationClip>();
            GameObject character = GameObject.Find("characters");
            initialPos = character.transform.position;
            initialRot = character.transform.rotation;
            Transform[] allChildren = character.GetComponentsInChildren<Transform>();
            characters = new List<GameObject>();
            int index = 0;
            foreach (Transform child in allChildren)
            {
                if (child.parent == character.transform)
                {
                    characters.Add(child.gameObject);
                    index++;
                }
            }
            for (int i = 1; i < characters.Count; i++)
            {
                characters[i].SetActive(false);
            }
            Init(0);
        }

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
            if (timeLeft < 0)
            {
                if (VideoCaptureCtrl.instance.status == VideoCaptureCtrl.StatusType.STARTED)
                {
                    VideoCaptureCtrl.instance.StopCapture();
                    while (VideoCaptureCtrl.instance.status == VideoCaptureCtrl.StatusType.STOPPED)
                    {

                    }
                }
                if (clipCounter < clips.Length)
                {
                    SwitchAnimation();
                }
                else
                {
                    cameraCounter++;
                    if (cameraCounter < cameras)
                    {
                        clipCounter = 0;
                        GetComponentInParent<VideoCaptureCtrl>().videoCaptures[0] = GameObject.Find("DedicatedCapture (" + cameraCounter + ")").GetComponent<VideoCapture>();
                        SwitchAnimation();
                    }
                    else
                    {
                        lightCounter++;
                        if (lightCounter < lights.Length)
                        {
                            clipCounter = 0;
                            cameraCounter = 0;
                            GetComponentInParent<VideoCaptureCtrl>().videoCaptures[0] = GameObject.Find("DedicatedCapture").GetComponent<VideoCapture>();                           
                            Transform myTransform = GameObject.Find("Directional light").transform;
                            Vector3 rot = myTransform.rotation.eulerAngles;
                            rot = new Vector3(rot.x, rot.y+lights[lightCounter]-lights[lightCounter-1], rot.z);
                            myTransform.rotation = Quaternion.Euler(rot);
                        }
                        else{
                            characterCounter++;
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
                                rename();
                                SwitchScene s = GetComponent<SwitchScene>();
                                s.finished = true;
                            }
                        }
                    }
                }
            }
        }

        void SwitchAnimation()
        {
            //----------------
            //Debug.Log(clips.Length);
            //Debug.Log(clips[clipCounter].name);
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
            clipCounter++;

            VideoCaptureCtrl.instance.StartCapture();
        }

        void rename()
        {
            string path = "C:/Users/ISL-WORKSTATION/Documents/RockVR/Video/";
            var files = Directory.GetFiles(path).OrderBy(f => f);
            ArrayList arr = getString();
            int i = 0;
            bool deleted = !SceneManager.GetActiveScene().name.Equals("town");
            foreach(var file in files){
                string fileName = Path.GetFileName(file);
                if(Char.IsNumber(fileName[0])){
                    if (i == 0 && !deleted)
                    {
                        File.Delete(file);
                        deleted = true;
                    }
                    else
                    {
                        Debug.Log(arr[i]);
                        File.Move(file, path + arr[i]);
                        i++;
                        if (i == arr.Count)
                        {
                            break;
                        }
                    }
                }
            }
        }

        public ArrayList getString()
        {
            ArrayList arr = new ArrayList();
            for (int i = 0; i < characters.Count; i++)
            {
                for (int j = 0; j < lights.Length; j++)
                {
                    for (int k = 0; k < cameras; k++)
                    {
                        for (int l = 0; l < clips.Length; l++)
                        {
                            arr.Add("_" + clips[l].name.Substring(charLoc+1) + "_" + characters[i].name + "_" + SceneManager.GetActiveScene().name +"_camera" + k + "_light" + j + ".mp4");
                        }
                    }
                }
            }
            return arr;
        }
    }
}
