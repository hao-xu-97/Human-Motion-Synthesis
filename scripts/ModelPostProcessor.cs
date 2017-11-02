//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;
//using UnityEditor;
//using System;

//public class ModelImportSettings : AssetPostprocessor {
 
//    private const string processedTag = "Model_preprocessed";
//    private const string humanoidCharacterBaseFolder = "Assets/3D Models/Characters";
 
//    private void OnPreprocessModel() {
//        if (!assetPath.StartsWith(humanoidCharacterBaseFolder))
//            return;
 
//        var importer = (ModelImporter) assetImporter;
//        if (importer.userData.Contains(processedTag))
//            return;
 
//        importer.optimizeGameObjects = true;
//        importer.importAnimation = false;
//        importer.animationType = ModelImporterAnimationType.Human;
 
//        Func<string, string, HumanBone> Bone = (humanName, boneName) => new HumanBone() {humanName = humanName, boneName = boneName};
//        var humanDesc = new HumanDescription {
//            human = new[] {
//                Bone("Chest", "chest"),
//                Bone("Head", "head_0"),
//                Bone("Hips", "hips_0"),
//                Bone("Left Index Distal", "f_index_03_L"),
//                Bone("Left Index Intermediate", "f_index_02_L"),
//                Bone("Left Index Proximal", "f_index_01_L"),
//                Bone("Left Middle Distal", "f_middle_03_L"),
//                Bone("Left Middle Intermediate", "f_middle_02_L"),
//                Bone("Left Middle Proximal", "f_middle_01_L"),
//                Bone("Left Ring Distal", "f_ring_03_L"),
//                Bone("Left Ring Intermediate", "f_ring_02_L"),
//                Bone("Left Ring Proximal", "f_ring_01_L"),
//                Bone("Left Thumb Distal", "thumb_03_L"),
//                Bone("Left Thumb Intermediate", "thumb_02_L"),
//                Bone("Left Thumb Proximal", "thumb_01_L"),
//                Bone("LeftFoot", "foot_L"),
//                Bone("LeftHand", "hand_L"),
//                Bone("LeftLowerArm", "forearm_L_0"),
//                Bone("LeftLowerLeg", "shin_L_0"),
//                Bone("LeftShoulder", "shoulder_L"),
//                Bone("LeftToes", "toe_L"),
//                Bone("LeftUpperArm", "upper_arm_L"),
//                Bone("LeftUpperLeg", "thigh_L"),
//                Bone("Neck", "neck"),
//                Bone("Right Index Distal", "f_index_03_R"),
//                Bone("Right Index Intermediate", "f_index_02_R"),
//                Bone("Right Index Proximal", "f_index_01_R"),
//                Bone("Right Middle Distal", "f_middle_03_R"),
//                Bone("Right Middle Intermediate", "f_middle_02_R"),
//                Bone("Right Middle Proximal", "f_middle_01_R"),
//                Bone("Right Ring Distal", "f_ring_03_R"),
//                Bone("Right Ring Intermediate", "f_ring_02_R"),
//                Bone("Right Ring Proximal", "f_ring_01_R"),
//                Bone("Right Thumb Distal", "thumb_03_R"),
//                Bone("Right Thumb Intermediate", "thumb_02_R"),
//                Bone("Right Thumb Proximal", "thumb_01_R"),
//                Bone("RightFoot", "foot_R"),
//                Bone("RightHand", "hand_R"),
//                Bone("RightLowerArm", "forearm_R_0"),
//                Bone("RightLowerLeg", "shin_R_0"),
//                Bone("RightShoulder", "shoulder_R"),
//                Bone("RightToes", "toe_R"),
//                Bone("RightUpperArm", "upper_arm_R"),
//                Bone("RightUpperLeg", "thigh_R"),
//                Bone("Spine", "spine"),
//            }
//        };
//        importer.humanDescription = humanDesc;
//    }
 
//    void OnPostprocessModel(GameObject model) {
//        if (!assetPath.StartsWith(humanoidCharacterBaseFolder))
//            return;
 
//        var importer = (ModelImporter) assetImporter;
//        if (importer.userData.Contains(processedTag))
//            return;
 
//        importer.userData = importer.userData + processedTag;
 
//        var animator = model.GetComponent<Animator>();
//        animator.applyRootMotion = true;
//        animator.avatar = importer.sourceAvatar;
//    }
//}

