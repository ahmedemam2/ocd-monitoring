# import the necessary packages
from collections import deque
import numpy as np
import cv2
import imutils

# construct the argument parse and parse the arguments

# define the lower and upper boundaries of the "green"
# ball in the HSV color space, then initialize the
# list of tracked points
import socket
socket = socket.socket()  # step 1
hostname = '127.0.0.1'
port = 65434
socket.bind((hostname, port))  # step 2

serverAddress = ((hostname, port))
list = [50, 100]
socket.listen(5)
listpoints = []
conn, addr = socket.accept()
# msg = list
# Data1 = str.encode(str(msg))
# conn.send(Data1)

greenLower = (0,0,0)
greenUpper = (30, 30, 32)
pts = deque(maxlen=300)
# if a video path was not supplied, grab the reference
# to the webcam

# otherwise, grab a reference to the video file
vs = cv2.VideoCapture(0)
# allow the camera or video file to warm up
pointslist = []
# keep looping

while vs.isOpened():
    # grab the current frame
    frame = vs.read()
    # handle the frame from VideoCapture or VideoStream
    frame = frame[1]
    # if we are viewing a video and we did not grab a frame,
    # then we have reached the end of the video
    if frame is None:
        center = [-1, -1]
        msg = center
        Data1 = str.encode(str(center))
        conn.send(Data1)
        break
    # resize the frame, blur it, and convert it to the HSV
    # color space
    frame = imutils.resize(frame, width=600)
    blurred = cv2.GaussianBlur(frame, (11, 11), 0)
    hsv = cv2.cvtColor(blurred, cv2.COLOR_BGR2HSV)
    # construct a mask for the color "black", then perform
    # a series of dilations and erosions to remove any small
    # blobs left in the mask
    mask = cv2.inRange(hsv, greenLower, greenUpper)
    mask = cv2.erode(mask, None, iterations=2)
    mask = cv2.dilate(mask, None, iterations=2)
    # find contours in the mask and initialize the current
    # (x, y) center of the ball
    cnts = cv2.findContours(mask.copy(), cv2.RETR_EXTERNAL,
                            cv2.CHAIN_APPROX_SIMPLE)
    cnts = imutils.grab_contours(cnts)
    center = None
    # only proceed if at least one contour was found
    if len(cnts) > 0:
        c = max(cnts, key=cv2.contourArea)
        ((x, y), radius) = cv2.minEnclosingCircle(c)
        M = cv2.moments(c)
        center = (int(M["m10"] / M["m00"]), int(M["m01"] / M["m00"]))
        listpoints.append(center[0])
        listpoints.append(center[1])
        print(listpoints)
        msg = listpoints
        Data1 = str.encode(str(msg))
        conn.send(Data1)
        listpoints=[]
        pointslist.append(center)
        if radius > 10:
            cv2.circle(frame, (int(x), int(y)), int(radius),
                       (0, 255, 255), 2)
        pts.appendleft(center)
        # loop over the set of tracked points
        for i in range(1, len(pts)):

            if pts[i - 1] is None or pts[i] is None:
                continue

            thickness = int(np.sqrt(300 / float(i + 1)) * 2.5)

        cv2.imshow('display',frame)
        key = cv2.waitKey(1) & 0xFF
        # if the 'q' key is pressed, stop the loop
        if key == ord("q"):
            break

vs.release()
# close all windows
cv2.destroyAllWindows()
