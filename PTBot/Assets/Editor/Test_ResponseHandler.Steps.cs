using LightBDD.NUnit3;
using NUnit.Framework;
using System.Collections;
using UnityEngine;
using UnityEngine.TestTools;

namespace Assets.Editor
{
    public partial class Test_ResponseHandler : FeatureFixture
    {
        private ResponseHandler responseHandler = new ResponseHandler();
        string chatbotResponse = "";

        private void TheResponseIsEmpty(string chatbotResponse)
        {
            this.chatbotResponse = chatbotResponse;
            Assert.That(string.IsNullOrWhiteSpace(chatbotResponse));
        }

        private void TheResponseIsNotEmpty(string chatbotResponse)
        {
            this.chatbotResponse = chatbotResponse;
            Assert.That(!string.IsNullOrWhiteSpace(chatbotResponse));
        }

        private void TheResponseIsChecked()
        {
            responseHandler.HandleResponse(chatbotResponse);
        }

        private void TheAudioToPlayIsTheDefaultError()
        {
            Assert.That(responseHandler.CheckAudioToPlay() == "DefaultErrorResponse");
        }

        private void GoHomeIsTrue()
        {
            Assert.That(responseHandler.CheckGoHome() == true);
        }

        private void TheWorkoutShouldStart()
        {
            Assert.That(responseHandler.CheckStartWorkout() == true);
            Assert.That(responseHandler.PanelsToSetActive().Contains("StartWorkoutPanel"));
        }

        private void TheAreasPanelShouldBeDisplayed()
        {
            Assert.That(responseHandler.PanelsToSetActive().Contains("AreasToTrainPanel"));
        }

        private void TheDiagramShouldBeCorrect(string diagramToShow)
        {
            Assert.That(diagramToShow == responseHandler.CheckDiagramToShow());
        }

        /*public IEnumerator TheResponseIsChecked()
        {
            responseHandler.PlayAudio();
            yield return null;
        }*/

    }
}