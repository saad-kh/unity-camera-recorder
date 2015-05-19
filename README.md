# UnityCameraRecorder
Prototype that enables the recording of a video straight from an Unity3D Camera

# Dependencies
* [ffmpeg](www.ffmpeg.org)
* [opencv](www.opencv.org) with ffmpeg support

# Description
This prototype enables the recording of a video straight from an Unity3D Camera. It comes with a C# script that should be added to the scene and provided with the desired Camera. It has also a native C++ plugin that should be compiled and added as a native plugin to the Unity Project.
The managed code gathers the Rendered frames of the camera while the unmanaged code packs them into a video file.

When the script is asked to start recording, it'll wait for each frame to be rendered, rerender it into a native texture and then stock it as an array of pixels. It'll proceed as such by storing each frame this way along with the previous ones. When the recording is stopped, it'll send this array of frames to the C++ plugin as a mean to pack them in a video.

The C++ plugin will receive this pack of data, initiate a video file writing thanks to opencv, transform each frame into a compatible image that is then added to the video. When done, the video is available to the user.

# Matter of concerns and improvements

## Frame Rate
For now the frame rate of the video is as simple as the recoding duration divided by the number of frames. The result is that the total length of the video is equivalent to the recording session’s length, however the frame rate is constant for the video while might have been very inconsistent in the original material. This issue is pretty serious but beyond the scope of this prototype.

## Buffer of frames
For now the code just keep storing rendered frames as long as it has to record. It frees them when the recording is done and the video created. We could have chosen to stream a frame to the video each time it's rendered and free ourselves from the chore of storing them. However on lower end systems the decrease of performance could be noticed, impacting the user experience as well as the recorded video, while with our method, there is still a decrease of performance but it only happens once at the end of the recording session.

## Length of recording
How long we can keep recording a video is an issue because we keep allocating memory at each new frame and unfortunately computers don't have as much memory as infinite memory. One way of thinking is to enable a time limit that when reached will either stop recording the video or record what we already have and pursue the recoding. For the later we can either have multiple videos or merge them in the end.
Lastly, a feature that is out of the prototype's scope is to thread the video writing as a mean to continually uncharge our frames buffer while being sure to not overwhelm the application.
