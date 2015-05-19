using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;

public class CameraRecorder : MonoBehaviour {

	//External Plugin Function Definition
	//Pack the accumulated frames and samples in a video with a width and height size that will be recorded at videopath
	[DllImport ("CameraRecorder")]
	private static extern void PackVideo(
		string videoPath, 
		int width, 
		int height, 
		System.IntPtr[] colors, 
		float frameCount,
		float duration);
	
	
	public  Camera 			videoSource;

	private bool 	recording = false;
	private float 	startingTime = 0;
	
	private List<GCHandle> frames;	//Acumulated frames
	private List<GCHandle> samples;	//Acummulated samples
	
	void Start () {
		frames 	= new List<GCHandle>();
		samples = new List<GCHandle>();
	}
	
	
	void Update () {
		if(Input.GetButtonUp("Record") && videoSource)
		{
			recording = !recording;
			if(recording)
			{
				Debug.Log("Starting the recording");
				startingTime = Time.time;
			}
			else
			{
				Debug.Log("Stoping the recording");
				float duration = Time.time - startingTime;

				//Create a C friendly Array for the frames and then Call the C Function to Pack the video
				int frameCount = frames.Count;
				if(frameCount > 0)
				{
					System.IntPtr[] framesArray = new System.IntPtr[frameCount];
					for (int i = 0; i < frameCount; i++)
						framesArray[i] = frames[i].AddrOfPinnedObject();

					PackVideo(
						Application.dataPath + "/CameraRecorder/RecordedVideo.avi",
						videoSource.pixelWidth, 
						videoSource.pixelHeight,
						framesArray,
						frameCount,
						duration);
				}

				//Free the accumulated frames
				frames.Clear();

				Debug.Log ("FrameCount " + frameCount + " duration " + duration);
			}
		}
	}
	
	void LateUpdate() {
		if (videoSource && recording) {
			//Add the last Rendered Frame of the videoSource to the accumulated Frames

			//Render the Camera Frame into a 2D texture
			RenderTexture rt = new RenderTexture(videoSource.pixelWidth, videoSource.pixelHeight, 24);
			videoSource.targetTexture = rt;
			Texture2D 	screenShot = new Texture2D(videoSource.pixelWidth, videoSource.pixelHeight, TextureFormat.ARGB32, false);
			videoSource.Render();
			RenderTexture.active = rt;
			screenShot.ReadPixels(new Rect(0, 0, videoSource.pixelWidth, videoSource.pixelHeight), 0, 0);
			videoSource.targetTexture = null;
			RenderTexture.active = null;
			Destroy(rt);

			//Add the rendered frame as pixels to the accumulated frames 
			Color32[] pixels = screenShot.GetPixels32();
			GCHandle pixelsHandle = GCHandle.Alloc(pixels, GCHandleType.Pinned);
			frames.Add(pixelsHandle);
		}
	}
	
}
