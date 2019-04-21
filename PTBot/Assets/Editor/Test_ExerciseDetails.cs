using LightBDD.Framework;
using LightBDD.Framework.Scenarios;
using LightBDD.NUnit3;

[assembly: LightBddScopeAttribute]

namespace Assets.Editor
{
    public partial class Test_ExerciseDetails
    {
        [Label("Get exercise details based on valid exercise")]
        [Scenario]
        public void ValidExercise()
        {
            Runner.RunScenario(
                Given => CheckTheArrayIsNotNull(),
                When => WhenTheExerciseIsChecked(" Air Bikes"),
                Then => ThenTheResultIsNotNull(),
                And => AndTheExerciseDetailsAreFound());
        }

        [Label("Try using an invalid exercise")]
        [Scenario]
        public void InvalidExercise()
        {
            Runner.RunScenario(
                Given => CheckTheArrayIsNotNull(),
                When => WhenTheExerciseIsChecked("Fake Exercise"),
                Then => ThenTheResultIsNull());
        }
    }
}