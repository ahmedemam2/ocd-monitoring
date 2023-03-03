import cv2
from cvzone.HandTrackingModule import HandDetector
import socket
socket = socket.socket()  # step 1
hostname = '127.0.0.1'
port = 65434
socket.bind((hostname, port))  # step 2

serverAddress = ((hostname, port))
socket.listen(5)
conn, addr = socket.accept()


cap=cv2.VideoCapture(0);
detector=HandDetector(detectionCon=0.8,maxHands=1)


while True:
    success , img=cap.read();
    img=cv2.flip(img,1)
    hands,img=detector.findHands(img,flipType =False)
    if hands:# if i detect at least one hand
        lmList=hands[0]["lmList"]
        pointIndex=lmList[8][0:2]
        print(pointIndex)
        msg = pointIndex
        Data1 = str.encode(str(msg))
        conn.send(Data1)
        print(pointIndex)
    cv2.imshow("img", img)
    cv2.waitKey(1)