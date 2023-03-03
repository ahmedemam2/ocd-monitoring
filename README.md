# ocd-monitoring
The idea of this project is to monitor the repetitive actions of people with OCD in the context of organizing materials, by which instead of organizing it in real
life, they do it in the a simple 2D game.
I used socket programming to communicate between python and c#.
Applied face recognition to approve the user of the device has authority and applied face emotion to check their state when they came.
Then applied pose estimation by extracting the landmarks using Mediapipe and classification through dollarpy so once the user tries to adjust an object they are switched immediately to the game.
An excperiment was made to check which technology out of the three: Laser pointer, TUIO, and hand gesture was most suited for the user, with the results shown in file Evaluation Report.pdf .
Eye gaze was tracked as well as the face emotion to check which technology the user showed most attention and didnt have an angry or a confused face emotion.



<img width="170" alt="Results" src="https://user-images.githubusercontent.com/88343933/222791675-c5c93efe-99b4-4ac9-8681-024cadddf9aa.png">
