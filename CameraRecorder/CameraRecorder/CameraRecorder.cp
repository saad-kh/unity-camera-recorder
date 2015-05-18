/*
 *  CameraRecorder.cp
 *  CameraRecorder
 *
 *  Created by Saad Khoudmi on 03/05/15.
 *  Copyright (c) 2015 Saad Khoudmi. All rights reserved.
 *
 */

#include "CameraRecorder.h"
#include <opencv2/opencv.hpp>

extern "C" void PackVideo(
                          const char* videoPath,
                          int width,
                          int height,
                          char** frames,
                          float frameCount,
                          float duration
                          )
{
    cv::VideoWriter videoWriter = cv::VideoWriter(
                                                  std::string(videoPath),
                                                  CV_FOURCC('M','J','P','G'),
                                                  frameCount/duration,
                                                  cv::Size(width, height)
                                                  );

    //Loop trough the frames to convert them into opencv frames and add them to the video writer
    for(int i = 0; i < frameCount; i++)
    {
        IplImage* frameData = cvCreateImageHeader(cvSize(width, height), IPL_DEPTH_8U, 4);
        cvSetData(frameData, frames[i], frameData->widthStep);
        
        
        cv::Mat flippedFrame;
        cv::Mat frame;
        
        //opencv has a reverse plan compared to unity frames, we need to flip the frame
        cv::flip(cv::Mat(frameData), flippedFrame, 0);
        //opencv has a reverse color scheme (BGR) we need to convert it from unity's RGB
        cv::cvtColor(flippedFrame, frame, CV_RGB2BGR);
        
        videoWriter.write(frame);
        cvReleaseImageHeader(&frameData);
    }
    videoWriter.release();
}