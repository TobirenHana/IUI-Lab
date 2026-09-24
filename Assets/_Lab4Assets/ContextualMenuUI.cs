using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ContextualMenuUI : MonoBehaviour
{
    [Header("Contextual Menu")]
    public TMP_Text mainText;
    public GameObject buttonsRoot;
    public Button startButton;

    [Header("Existing Progress System")]
    public ProgressTracker progressTracker;

    private TMP_Text startButtonText;
    private string lastDisplayedText = "";

    void Start()
    {
        //find tmp text inside the start button
        if (startButton != null)
        {
            startButtonText =
                startButton.GetComponentInChildren<TMP_Text>(true);
        }
    }

    void Update()
    {
        /*
         * when quit is pressed it hides the buttons object
         * and uses MainText for the final summary
         *
         * we stop updating MainText
         */
        if (buttonsRoot != null && !buttonsRoot.activeInHierarchy)
            return;

        if (mainText == null ||
            progressTracker == null ||
            progressTracker.runController == null)
            return;

        //shows instructions
        if (!progressTracker.runController.Started)
        {
            SetMenuText(BuildIntroText());
            return;
        }

        if (startButton != null)
            startButton.interactable = false;

        if (startButtonText != null)
            startButtonText.text = "Running";

        SetMenuText(BuildProgressText());
    }

    void SetMenuText(string newText)
    {
        if (newText == lastDisplayedText)
            return;

        mainText.text = newText;
        lastDisplayedText = newText;
    }

    string BuildIntroText()
    {
        return
            "<b><size=90%>A Terrible Day in the Office</size></b>\n\n" +
            "Complete these 4 tasks:\n" +
            "• Drawers - file 8 files\n" +
            "• Trash - bin both items\n" +
            "• Coffee - fill the cup\n" +
            "• Cleaning - clean 5 spots\n\n" +
            "<b>Press Start when ready.</b>";
    }

    string BuildProgressText()
    {
        //both drawers must be complete for Drawers to show as DONE
        
        bool drawersDone =
            progressTracker.drawerA != null &&
            progressTracker.drawerB != null &&
            progressTracker.drawerA.IsComplete &&
            progressTracker.drawerB.IsComplete;

        bool trashDone =
            progressTracker.trashTask != null &&
            progressTracker.trashTask.IsComplete;

        bool coffeeDone =
            progressTracker.coffeeTask != null &&
            progressTracker.coffeeTask.IsComplete;

        bool cleaningDone =
            progressTracker.cleaningTask != null &&
            progressTracker.cleaningTask.IsComplete;

        int completed = 0;

        if (drawersDone) completed++;
        if (trashDone) completed++;
        if (coffeeDone) completed++;
        if (cleaningDone) completed++;

        string bottomMessage;

        if (completed == 4)
        {
            bottomMessage =
                "\n<color=#72E68A><b>All tasks complete!</b></color>\n" +
                "Press Quit to view the run summary.";
        }
        else
        {
            bottomMessage =
                "\nUse the controls on the right if needed.";
        }

        return
            "<b>Tasks complete: " + completed + "/4</b>\n\n" +
            Status(drawersDone) + "  Drawers\n" +
            Status(trashDone) + "  Trash\n" +
            Status(coffeeDone) + "  Coffee\n" +
            Status(cleaningDone) + "  Cleaning\n" +
            bottomMessage;
    }

    string Status(bool complete)
    {
        if (complete)
            return "<color=#72E68A><b>DONE</b></color>";

        return "<b>TODO</b>";
    }
}

