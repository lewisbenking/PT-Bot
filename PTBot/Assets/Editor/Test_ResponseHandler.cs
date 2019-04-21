using LightBDD.Framework;
using LightBDD.Framework.Scenarios;
using LightBDD.NUnit3;

namespace Assets.Editor
{
    [Label("FEAT-1")]
    public partial class Test_ResponseHandler
    {
        [Label("The default error response audio string should be returned from an invalid input")]
        [Scenario]
        public void InvalidResponse()
        {
            Runner.RunScenario(
                Given => TheResponseIsEmpty(""),
                When => TheResponseIsChecked(),
                Then => TheAudioToPlayIsTheDefaultError());

            Runner.RunScenario(
               Given => TheResponseIsEmpty(null),
               When => TheResponseIsChecked(),
               Then => TheAudioToPlayIsTheDefaultError());
        }   

        [Label("The returned value for goHome should be true when the response contains 'Bye'")]
        [Scenario]
        public void ValidResponse_Bye()
        {
            Runner.RunScenario(
                Given => TheResponseIsNotEmpty("Bye"),
                When => TheResponseIsChecked(),
                Then => GoHomeIsTrue());
        }

        [Label("The assumes the user has chosen their equipment and the relevant panels will be returned and set active")]
        [Scenario]
        public void ValidResponse_DiagramToShow()
        {
            Runner.RunScenario(
                Given => TheResponseIsNotEmpty("Arms workout"),
                When => TheResponseIsChecked(),
                Then => TheDiagramShouldBeCorrect("ArmsDiagram"));

            Runner.RunScenario(
                Given => TheResponseIsNotEmpty("Back workout"),
                When => TheResponseIsChecked(),
                Then => TheDiagramShouldBeCorrect("BackDiagram"));

            Runner.RunScenario(
                Given => TheResponseIsNotEmpty("Chest workout"),
                When => TheResponseIsChecked(),
                Then => TheDiagramShouldBeCorrect("ChestDiagram"));

            Runner.RunScenario(
                Given => TheResponseIsNotEmpty("Core workout"),
                When => TheResponseIsChecked(),
                Then => TheDiagramShouldBeCorrect("CoreDiagram"));

            Runner.RunScenario(
                Given => TheResponseIsNotEmpty("Legs workout"),
                When => TheResponseIsChecked(),
                Then => TheDiagramShouldBeCorrect("LegsDiagram"));
        }

        [Label("The returned value for startWorkout should be true when the response contains 'Ok. Let's do the workout.'")]
        [Scenario]
        public void ValidResponse_LetsDoWorkout()
        {
            Runner.RunScenario(
                Given => TheResponseIsNotEmpty("Ok. Let's do the workout."),
                When => TheResponseIsChecked(),
                Then => TheWorkoutShouldStart());
        }

        [Label("The correct panel should be displayed based on the chatbot response.")]
        [Scenario]
        public void ValidResponse_PanelToBeDisplayed()
        {
            Runner.RunScenario(
                Given => TheResponseIsNotEmpty("which area would you like to train today"),
                When => TheResponseIsChecked(),
                Then => TheCorrectPanelShouldBeActive("AreasToTrainPanel"));

            Runner.RunScenario(
                Given => TheResponseIsNotEmpty("- "),
                When => TheResponseIsChecked(),
                Then => TheCorrectPanelShouldBeActive("ExercisesPanel"));

            Runner.RunScenario(
                Given => TheResponseIsNotEmpty("Ok. Let's do the workout."),
                When => TheResponseIsChecked(),
                Then => TheCorrectPanelShouldBeActive("StartWorkoutPanel"));
        }
    }
}