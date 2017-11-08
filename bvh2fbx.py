import bpy
import sys
import os
import glob
import random, string

def randomword(length):
   letters = string.ascii_lowercase
   return ''.join(random.choice(letters) for i in range(length))



dir_path = "C:\\Users\\ISL-WORKSTATION\\Desktop\\New folder\\bvh\\"
#Get command line arguments
#argv = sys.argv
#argv = argv[argv.index("--") + 1:] # get all args after "—"
print(dir_path)
os.chdir(dir_path)
fbx_out = "C:\\Users\\ISL-WORKSTATION\\Desktop\\New folder\\MocapUnity\\Assets\\Resources\\Input\\"
for file in glob.glob("*.bvh"):
    print(file)
    bvh_in = dir_path+file
     
    # Import the BVH file
    # See http://www.blender.org/documentation/blender_python_api_2_60_0/bpy.ops.import_anim.html
    bpy.ops.import_anim.bvh(filepath=bvh_in, filter_glob="*.bvh", global_scale=1, frame_start=1, use_fps_scale=False, use_cyclic=False, rotate_mode='NATIVE', axis_forward='-Z', axis_up='Y')
     
# Export as FBX 
# See http://www.blender.org/documentation/blender_python_api_2_62_1/bpy.ops.export_scene.html
bpy.ops.export_scene.fbx(filepath=fbx_out+randomword(5)+".fbx", axis_forward='-Z', axis_up='Y', use_anim=True, use_selection=True, use_default_take=False)
bpy.ops.wm.read_factory_settings()
