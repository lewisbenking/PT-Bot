using LightBDD.NUnit3;
using NUnit.Framework;

namespace Assets.Editor
{
    public partial class Test_ExerciseDetails : FeatureFixture
    {
        private ExerciseDetails exerciseDetails = new ExerciseDetails();
        private int indexToSearch;

        private void CheckTheArrayIsNotNull()
        {
            Assert.That(exerciseDetails.GetEntireArray() != null);
        }

        private void WhenTheExerciseIsChecked(string exerciseToCheck)
        {
            indexToSearch = exerciseDetails.GetArrayIndex(exerciseToCheck);
        }

        private void TheItemExistsInTheArray()
        {
            Assert.That(indexToSearch != -1);
        }

        private void TheExerciseDetailsAreFound()
        {
            Assert.That(!string.IsNullOrWhiteSpace(exerciseDetails.GetArrayValue(indexToSearch, 1)));
        }

        private void TheItemDoesNotExistInTheArray()
        {
            Assert.That(indexToSearch == -1);
        }
    }
}