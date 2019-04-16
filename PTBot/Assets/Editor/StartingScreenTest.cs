using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NUnit.Framework;
using UnityEditor.SceneManagement;

public class StartingScreenTest
{
    public StartingScreenTest()
    {
        EditorSceneManager.OpenScene("Assets/Scenes/StartingScreen.unity");
    }

    [Test]
    public void StartingScreen()
    {
        StartingScreen startingScreen = GameObject.Find("Main Camera").GetComponent<StartingScreen>();
        startingScreen.PlayAudio();
        Assert.AreEqual(true, startingScreen.audioPlayed);
    }
}
