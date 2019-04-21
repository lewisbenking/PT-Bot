using System.Collections;
using System.Collections.Generic;

public class ResponseHandler
{
    private string diagramToShow, audioToPlay;
    private ArrayList panelsToSetActive, panelsToSetInactive;
    private bool goHome, startWorkout, getExerciseDetails;
    JimBot jimbot = new JimBot();

    public void HandleResponse(string chatbotResponse)
    {
        goHome = false; startWorkout = false; getExerciseDetails = false;
        diagramToShow = "";
        panelsToSetActive = new ArrayList();
        panelsToSetInactive = new ArrayList();

        if (string.IsNullOrWhiteSpace(chatbotResponse))
        {
            audioToPlay = "DefaultErrorResponse";
        }
        else
        {
            if ((!chatbotResponse.Contains("Bye")) || (!chatbotResponse.Contains("Thanks for your time")) || (!chatbotResponse.Contains("No worries, take care")) || (!chatbotResponse.Contains("Thanks for using JimBot")))
            {
                if (chatbotResponse.Contains("Ok. Let's do the workout."))
                {
                    panelsToSetActive.Add("StartWorkoutPanel");
                    panelsToSetInactive.Add("ExercisesPanel");
                    panelsToSetInactive.Add("scrollArea");
                    panelsToSetInactive.Add("IndividualExercisePanel");
                    panelsToSetInactive.Add("MuscleDiagramPanel");
                    startWorkout = true;
                }
                else
                {
                    if (chatbotResponse.ToLower().Contains("which area would you like to train today"))
                    {
                        panelsToSetActive.Add("AreasToTrainPanel");
                        panelsToSetActive.Add("scrollArea");
                    }

                    if (chatbotResponse.Contains("Arms")) diagramToShow = "ArmsDiagram";
                    else if (chatbotResponse.Contains("Back")) diagramToShow = "BackDiagram";
                    else if (chatbotResponse.Contains("Chest")) diagramToShow = "ChestDiagram";
                    else if (chatbotResponse.Contains("Core")) diagramToShow = "CoreDiagram";
                    else if (chatbotResponse.Contains("Legs")) diagramToShow = "LegsDiagram";
                    else if (chatbotResponse.Contains("No Equipment")) diagramToShow = "CoreDiagram";

                    if (chatbotResponse.ToLower().Contains("- ")) getExerciseDetails = true;
                }
                audioToPlay = "ChatbotResponse";
            }
            else
            {
                goHome = true;
            }
        }
    }
    
    public ArrayList PanelsToSetActive() { return panelsToSetActive; }

    public ArrayList PanelsToSetInactive() { return panelsToSetInactive; }

    public bool CheckGetExerciseDetails() { return getExerciseDetails; }

    public bool CheckGoHome() { return goHome; }

    public bool CheckStartWorkout() { return startWorkout; }

    public string CheckAudioToPlay() { return audioToPlay; }

    public string CheckDiagramToShow() { return diagramToShow; }


    public void REE()
    {
        foreach (string item in panelsToSetActive)
        {
            //JimBot.PanelSetActive(GameObject.Find(item));
        }
    }
}
