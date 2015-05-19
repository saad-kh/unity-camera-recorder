/*
 *  CameraRecorder.h
 *  CameraRecorder
 *
 *  Created by Saad Khoudmi on 03/05/15.
 *  Copyright (c) 2015 Saad Khoudmi. All rights reserved.
 *
 */

extern "C" {
#include <CoreFoundation/CoreFoundation.h>
	
#pragma GCC visibility push(default)

/* External interface to the CameraRecorder, C-based */
extern "C" {
    void PackVideo(
                   const char* videoPath,
                   int width,
                   int height,
                   char** frames,
                   float frameCount,
                   float duration);
}


#pragma GCC visibility pop
}
