# UnityCameraRecorder
Prototype that enables the recording of video straight from an Unity Camera

# Dependecies
* ffmpeg https://www.ffmpeg.org
* static opencv libraries compiled with ffmpeg support http://opencv.org

# Description
This prototype enables the recording of a video straight from an Unity Camera. It comes with a csharp script that should be added to the game scene and provided with the desired Camera. It has also a native c++ plugin that should be compiled and added as a native plugin to the Unity Project.
The Managed code gathers the Rendered frames of the camera while the unmanaged code packs them into a video file.

When the script is asked to start recording, it'll wait for each Frame to be rendered, rerender into a native texture and then stock it as an array of pixels. It'll proceed as such by storing each frame this way and adding it the previous ones. When the recording is stopped, it'll send this array of frame to the c++ plugin as a mean to pack them in a video.

The c++ plugin will receive this pack of data, initiate a video file writing thanks to opencv, transfrom each frame into a compatible image that is then added to the video. When done, the video is avaiable to the user.

# Matter of concerns and improvements
