using LightBDD.NUnit3;
using NUnit.Framework;

namespace Assets.Editor
{
    public partial class Test_ExerciseDetails : FeatureFixture
    {
        private ExerciseDetails exerciseDetails = new ExerciseDetails();
        private int indexToSearch;

        private void CheckTheArrayIsNotNull() { Assert.That(exerciseDetails.GetEntireArray() != null); }

        private void WhenTheExerciseIsChecked(string exerciseToCheck) { indexToSearch = exerciseDetails.GetArrayIndex(exerciseToCheck); }

        private void ThenTheResultIsNotNull() { Assert.That(indexToSearch != -1); }
        private void AndTheExerciseDetailsAreFound() { Assert.That(exerciseDetails.GetArrayValue(indexToSearch, 1) != null); }

        private void ThenTheResultIsNull() { Assert.That(indexToSearch == -1); }
    }
}