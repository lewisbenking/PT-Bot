using LightBDD.Framework;
using LightBDD.Framework.Scenarios;
using LightBDD.NUnit3;

[assembly: LightBddScopeAttribute]

namespace Assets.Editor
{
    public partial class Test_ExerciseDetails
    {
        [Label("Use an invalid exercise.")]
        [Scenario]
        public void TestInvalidExercise()
        {
            Runner.RunScenario(
                Given => CheckTheArrayIsNotNull(),
                When => WhenTheExerciseIsChecked(null),
                Then => TheItemDoesNotExistInTheArray());

            Runner.RunScenario(
                Given => CheckTheArrayIsNotNull(),
                When => WhenTheExerciseIsChecked(string.Empty),
                Then => TheItemDoesNotExistInTheArray());

            Runner.RunScenario(
                Given => CheckTheArrayIsNotNull(),
                When => WhenTheExerciseIsChecked("Fake input."),
                Then => TheItemDoesNotExistInTheArray());
        }

        [Label("Use a valid exercise.")]
        [Scenario]
        public void TestValidExercise()
        {
            Runner.RunScenario(
                Given => CheckTheArrayIsNotNull(),
                When => WhenTheExerciseIsChecked(" Air Bikes"),
                Then => TheItemExistsInTheArray(),
                And => TheExerciseDetailsAreFound());

            Runner.RunScenario(
                Given => CheckTheArrayIsNotNull(),
                When => WhenTheExerciseIsChecked(" Pullups"),
                Then => TheItemExistsInTheArray(),
                And => TheExerciseDetailsAreFound());
        }
    }
}