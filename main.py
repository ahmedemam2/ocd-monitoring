import time

import mediapipe as mp
from dollarpy import Recognizer, Template, Point
import os
import faces
from faces import cv2 , face_recognition , DeepFace
import eyegaze
import numpy as np



def facerecemotion():
    encodelist,namesl = faces.encodelist, faces.namesl
    cap = cv2.VideoCapture(0)
    while True:
        state,frame = cap.read()
        frameres = cv2.resize(frame,(0,0),None,0.25,0.25)
        frameres = cv2.cvtColor(frame, cv2.COLOR_BGR2RGB)
        facesloc = face_recognition.face_locations(frameres)
        encodeframe = face_recognition.face_encodings(frameres,facesloc)
        # zip to iterate on both in same loop
        for encface, faceloc in zip(encodeframe,facesloc):
            isMatch = face_recognition.compare_faces(encodelist,encface)
            ind = isMatch.index(True)
            for match in isMatch:
                if match == True:
                    name = namesl[ind]
                    predict = DeepFace.analyze(frameres,enforce_detection=False)
                    user = name
                    if predict:
                        userstate = predict['dominant_emotion']
                    # time.sleep(1)
                    return user, userstate
        cv2.imshow('Live Detection',frameres)
        cv2.waitKey(1)

# userface, useremotion = facerecemotion()
# print(userface,useremotion)

mpDraw = mp.solutions.drawing_utils
mp_pose = mp.solutions.pose
pose = mp_pose.Pose()
mp_drawing=mp.solutions.drawing_utils
mp_drawing_styles = mp.solutions.drawing_styles
mp_hollistic = mp.solutions.holistic
adjust = 0
def getPoint(vidPath):
    with mp_hollistic.Holistic(static_image_mode=True,
                               min_detection_confidence=0.7, min_tracking_confidence=0.7) as holisitc:
        right_shoulder = []
        left_shoulder = []

        video = cv2.VideoCapture(vidPath)
        while video.isOpened():
            ret, frame = video.read()

            # Recolor Feed
            if ret == True:
                image = cv2.cvtColor(frame, cv2.COLOR_BGR2RGB)
                image.flags.writeable = False

                results = holisitc.process(cv2.flip(image, 1))
                if results.pose_landmarks:
                    right_shoulder.append(
                        Point(results.pose_landmarks.landmark[16].x,
                              results.pose_landmarks.landmark[16].y,
                              16)
                    )
                    left_shoulder.append(
                        Point(results.pose_landmarks.landmark[14].x,
                              results.pose_landmarks.landmark[14].y,
                              14)
                    )
                # mp_drawing.draw_landmarks(image, results.pose_landmarks, mp_pose.POSE_CONNECTIONS,
                #                           mp_drawing.DrawingSpec(color=(245, 117, 66), thickness=2, circle_radius=2),
                #                           mp_drawing.DrawingSpec(color=(245, 66, 230), thickness=2, circle_radius=2)
                #                           )
            else:
                break
        listpoints = []
        listpoints.extend(right_shoulder + left_shoulder)
        return listpoints

def getPoint2(vidPath,ct,testvariable):
    with mp_hollistic.Holistic(static_image_mode=True,
                               min_detection_confidence=0.7, min_tracking_confidence=0.7) as holisitc:
        right_shoulder = []
        left_shoulder = []
        ct2 = 0
        resultslist= []
        video = cv2.VideoCapture(vidPath)
        while video.isOpened():
            ret, frame = video.read()
            image = cv2.cvtColor(frame, cv2.COLOR_BGR2GRAY)
            ct+=1
            if ct==35:
                listpoints = []
                listpoints.extend(right_shoulder + left_shoulder)
                results = reco.recognize(listpoints)
                resultslist.append(results[0])
                print(results[0])
                if results[0] == testvariable:
                    ct2+=1
                if ct2>1:
                    return ct2
                ct=0
                right_shoulder = []
                left_shoulder = []
            # Recolor Feed
            if ret == True:
                image = cv2.cvtColor(frame, cv2.COLOR_BGR2RGB)
                image.flags.writeable = False

                results = holisitc.process(cv2.flip(image, 1))
                if results.pose_landmarks:
                    right_shoulder.append(
                        Point(results.pose_landmarks.landmark[16].x,
                              results.pose_landmarks.landmark[16].y,
                              16)
                    )
                    left_shoulder.append(
                        Point(results.pose_landmarks.landmark[14].x,
                              results.pose_landmarks.landmark[14].y,
                              14)
                    )
            else:
                break
            cv2.imshow('Feed', image)
            cv2.waitKey(1)
    cv2.destroyAllWindows()




classes=[]
pth = "videos/adjust1.mp4"#path of video
points2 = getPoint(pth)
tmp = Template('adjust',points2)
classes.append(tmp)

pth = "videos/adjust2.mp4"#path of video
points2 = getPoint(pth)
tmp = Template('adjust',points2)
classes.append(tmp)

pth = "videos/sitstill1.mp4"#path of video
points2 = getPoint(pth)
tmp = Template('sit',points2)
classes.append(tmp)
reco= Recognizer(classes)
pth="videos/testadjust.mp4"
ct=0

if adjust>=1:
    os.system('cmd /c "cd D:/Ahmed/Programs/PycharmProjects/OcdMonitoring/TUIO-2D-game-master/bin/Debug & start TuioDemo.exe"')

userinterest = eyegaze.eyecontact()



