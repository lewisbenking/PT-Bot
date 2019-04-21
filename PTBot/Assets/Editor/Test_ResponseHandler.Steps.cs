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

        private void TheCorrectPanelShouldBeActive(string panel)
        {
            Assert.That(responseHandler.PanelsToSetActive().Contains(panel));
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