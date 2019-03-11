# PT-Bot
26-02-2019
- I've created a character in Fuse, and added an Idle animation with Mixamo.
- I have imported the character in Unity. I ran into issues with transparency but this is now fixed.
- I created an animation in Unity where the character model will rotate and his arm will rotate.
- This animation means he will turn and point at the canvas (where the video will play).
- The current project consists of the character model, the canvas and a button.
- When you click the button, the video will start playing and the character model will turn and point to the video. 
- When the video stops playing, the character model will return to the default idle animation.
- I've started writing the scripts to connect to the DialogFlow chatbot, I need to make some more modifications before I can start testing the connection.
- I've tested connecting to the DialogFlow API using Postman and have successfully sent POST requests and received responses.

04-03-2019
- I found a way to connect to the chatbot from Unity using the Google API NUGET package.
- I updated the script to connect to the chatbot with the Google API Plugins for Authorisation which generates the OAuth 2 Access Token.
- I quickly ran some data through the application and was successful.
- The chatbot API call returns text and audio output, which is currently displayed in Unity.
- However the audio is currently distorted, so need to fix that in next update.

11-03-2019
- I found a WavUtility script plugin online to fix the issue from last week with the distorted audio.
- It now loads the saved wav file into Unity to play the audio.
- I noticed a slight lag with the DialogFlow API call since last week, however this may depend on internet connection.
- Used the imported SALSA tools to set up lip syncing with the PT character model, and intial set up was successful.
- Started report layout.
