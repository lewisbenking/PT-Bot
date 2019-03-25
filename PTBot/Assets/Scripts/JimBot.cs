using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class JimBot : MonoBehaviour
{
    private DialogflowAPIScript dialogflowAPIScript;
    private GameObject pt, areasToTrainPanel, exercisesPanel, individualExercisePanel, muscleDiagramPanel, scrollArea, workoutEquipmentPanel;

    private void Awake()
    {
        exercisesPanel = GameObject.Find("ExercisesPanel");
        PanelSetActive(exercisesPanel, false);
        individualExercisePanel = GameObject.Find("IndividualExercisePanel");
        PanelSetActive(individualExercisePanel, false);
        workoutEquipmentPanel = GameObject.Find("WorkoutEquipmentPanel");
        PanelSetActive(workoutEquipmentPanel, false);
        muscleDiagramPanel = GameObject.Find("MuscleDiagramPanel");
        PanelSetActive(muscleDiagramPanel, false);
    }


    public void PanelSetActive(GameObject panel, bool isActive)
    {
        if (panel != null)
        {
            panel.SetActive(isActive);
        }
    }
}
