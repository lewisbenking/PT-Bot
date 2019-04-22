using LightBDD.NUnit3;
using NUnit.Framework;

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

        private void TheResponseHandlerIsCalled()
        {
            responseHandler.HandleResponse(chatbotResponse);
        }

        private void CheckTheAudioToPlay(string audioToPlay)
        {
            Assert.That(responseHandler.CheckAudioToPlay() == audioToPlay);
        }

        private void CheckGoHome(bool isTrue)
        {
            Assert.That(responseHandler.CheckGoHome() == isTrue);
        }

        private void CheckIfTheWorkoutShouldStart()
        {
            if (responseHandler.CheckStartWorkout() == true)
            {
                Assert.That(responseHandler.CheckPanelsToSetActive().Contains("StartWorkoutPanel"));
            }
            else
            {
                Assert.That(responseHandler.CheckStartWorkout() == false);
            }
        }

        private void CheckThePanelShouldBeSetActive(string panel)
        {
            if (responseHandler.CheckPanelsToSetActive().Count > 0)
            {
                Assert.That(responseHandler.CheckPanelsToSetActive().Contains(panel));
            }
            else
            {
                Assert.That(responseHandler.CheckPanelsToSetActive().Count <= 0);
            }
        }

        private void CheckTheDiagramToShow(string diagramToShow)
        {
            Assert.That(diagramToShow == responseHandler.CheckDiagramToShow());
        }
    }
}