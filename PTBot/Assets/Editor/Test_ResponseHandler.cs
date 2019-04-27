using LightBDD.Framework;
using LightBDD.Framework.Scenarios;
using LightBDD.NUnit3;

namespace Assets.Editor
{
    [Label("Assumes the API has returned an empty response, or the user hasn't provided a valid input")]
    public partial class Test_ResponseHandler
    {
        [Label("The string 'DefaultErrorResponse' should be returned from an invalid input, otherwise 'ChatbotResponse'")]
        [Scenario]
        public void TestAudioToPlay()
        {
            Runner.RunScenario(
                Given => TheResponseIsEmpty(string.Empty),
                When => TheResponseHandlerIsCalled(),
                Then => CheckTheAudioToPlay("DefaultErrorResponse"));

            Runner.RunScenario(
               Given => TheResponseIsEmpty(null),
               When => TheResponseHandlerIsCalled(),
               Then => CheckTheAudioToPlay("DefaultErrorResponse"));

            Runner.RunScenario(
                Given => TheResponseIsNotEmpty("Fake input."),
                When => TheResponseHandlerIsCalled(),
                Then => CheckTheAudioToPlay("ChatbotResponse"));
        }

        [Label("Assumes the user has chosen their equipment and the relevant panels will be returned and set active")]
        [Scenario]
        public void TestDiagramToShow()
        {
            Runner.RunScenario(
                Given => TheResponseIsNotEmpty("Arms workout"),
                When => TheResponseHandlerIsCalled(),
                Then => CheckTheDiagramToShow("ArmsDiagram"));

            Runner.RunScenario(
                Given => TheResponseIsNotEmpty("Arms workout"),
                When => TheResponseHandlerIsCalled(),
                Then => CheckTheDiagramToShow("ArmsDiagram"));

            Runner.RunScenario(
                Given => TheResponseIsNotEmpty("Back workout"),
                When => TheResponseHandlerIsCalled(),
                Then => CheckTheDiagramToShow("BackDiagram"));

            Runner.RunScenario(
                Given => TheResponseIsNotEmpty("Chest workout"),
                When => TheResponseHandlerIsCalled(),
                Then => CheckTheDiagramToShow("ChestDiagram"));

            Runner.RunScenario(
                Given => TheResponseIsNotEmpty("Core workout"),
                When => TheResponseHandlerIsCalled(),
                Then => CheckTheDiagramToShow("CoreDiagram"));

            Runner.RunScenario(
                Given => TheResponseIsNotEmpty("Legs workout"),
                When => TheResponseHandlerIsCalled(),
                Then => CheckTheDiagramToShow("LegsDiagram"));

            Runner.RunScenario(
                Given => TheResponseIsNotEmpty("Fake input."),
                When => TheResponseHandlerIsCalled(),
                Then => CheckTheDiagramToShow(string.Empty));
        }

        [Label("The returned value for goHome should be true when the response contains 'Bye', and false if not.")]
        [Scenario]
        public void TestGoHome()
        {
            Runner.RunScenario(
                Given => TheResponseIsNotEmpty("Bye"),
                When => TheResponseHandlerIsCalled(),
                Then => CheckGoHome(true));

            Runner.RunScenario(
                Given => TheResponseIsNotEmpty("Fake input."),
                When => TheResponseHandlerIsCalled(),
                Then => CheckGoHome(false));
        }

        [Label("The correct panel should be displayed based on the chatbot response.")]
        [Scenario]
        public void TestPanelToSetActive()
        {
            Runner.RunScenario(
                Given => TheResponseIsNotEmpty("Fake input."),
                When => TheResponseHandlerIsCalled(),
                Then => CheckThePanelShouldBeSetActive(""));

            Runner.RunScenario(
                Given => TheResponseIsNotEmpty("which area would you like to train today"),
                When => TheResponseHandlerIsCalled(),
                Then => CheckThePanelShouldBeSetActive("AreasToTrainPanel"));

            Runner.RunScenario(
                Given => TheResponseIsNotEmpty("- "),
                When => TheResponseHandlerIsCalled(),
                Then => CheckThePanelShouldBeSetActive("ExercisesPanel"));

            Runner.RunScenario(
                Given => TheResponseIsNotEmpty("Ok. Let's do the workout."),
                When => TheResponseHandlerIsCalled(),
                Then => CheckThePanelShouldBeSetActive("StartWorkoutPanel"));
        }

        [Label("The returned value for startWorkout should be true when the response contains 'Ok. Let's do the workout.'")]
        [Scenario]
        public void TestStartWorkout()
        {
            Runner.RunScenario(
                Given => TheResponseIsNotEmpty("Fake input."),
                When => TheResponseHandlerIsCalled(),
                Then => CheckIfTheWorkoutShouldStart());

            Runner.RunScenario(
                Given => TheResponseIsNotEmpty("Ok. Let's do the workout."),
                When => TheResponseHandlerIsCalled(),
                Then => CheckIfTheWorkoutShouldStart());
        }
    }
}