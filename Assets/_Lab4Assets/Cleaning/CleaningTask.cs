using UnityEngine;
using TMPro;

/**
This class handles the logic of the cleaning task

You can make changes in this file
*/
public class CleaningTask : MonoBehaviour
{   
    [Header("Drag colliders here")]
    public Collider[] targets;       // Drag grid colliders manually

    [Header("Filter (assign the sponge's Rigidbody)")]
    public Rigidbody spongeRigidbody; 

    [Header("State")]
    public bool cleaningTask;        // True when all zones touched
    public bool IsComplete { get { return cleaningTask; } }

    [Header("UI Feedback")]
    [SerializeField] private TMP_Text progressText; // 进度文本槽位

    // Internal fields
    private bool[] touched;
    private int touchedCount = 0;

    // DO NOT CHANGE THIS METHOD
    void Start()
    {
        // initialize array keeping track of progress
        int n = (targets != null) ? targets.Length : 0;
        touched = new bool[n];
        touchedCount = 0;

        // no zones means already complete
        cleaningTask = (n == 0); 

        UpdateProgressUI();
    }

    void UpdateProgressUI()
    {
        if (progressText != null && targets != null && targets.Length > 0)
        {
            int percentage = Mathf.RoundToInt(((float)touchedCount / targets.Length) * 100f);
            progressText.text = $"Cleaning: {percentage}%";
        }
    }

    // This method is called when a trigger collider is touched
    void OnTriggerEnter(Collider other)
    {
        if (cleaningTask || targets == null) return;

        // Only count when the assigned sponge Rigidbody touches the zone
        if (spongeRigidbody != null && other.attachedRigidbody != spongeRigidbody)
            return;

        // Loop over the targets, to see if this collision is a new one
        for (int i = 0; i < targets.Length; i++)
        {
            if (!touched[i] && other == targets[i])
            {
                touched[i] = true;
                touchedCount++;

                UpdateProgressUI();

                if (GetComponent<AudioSource>() != null)
                {
                    GetComponent<AudioSource>().Play(); // 播放音效
                }
                Debug.Log($"Touched cleaning spot. Progress: {touchedCount}/{targets.Length}");

                if (touchedCount == targets.Length)
                {
                    cleaningTask = true;
                    Debug.Log("Cleaning task COMPLETE: all zones touched.");
                }
                break;
            }
        }
    }
}