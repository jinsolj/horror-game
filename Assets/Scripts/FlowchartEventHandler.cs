using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fungus;

[RequireComponent(typeof(Flowchart))]
public class FlowchartEventHandler : MonoBehaviour {

    private Flowchart flowchart;
    private bool currentlyExecuting;
    private bool previouslyExecuting;

    private MenuDialog menuDialog;
    private bool currentlyMenuActive;
    private bool previouslyMenuActive;

    public delegate void OnStartExecutingHandler();
    public event OnStartExecutingHandler OnStartExecuting;
    public delegate void OnStopExecutingHandler();
    public event OnStopExecutingHandler OnStopExecuting;

    public delegate void OnStartMenuHandler();
    public event OnStartMenuHandler OnStartMenu;
    public delegate void OnStopMenuHandler();
    public event OnStopMenuHandler OnStopMenu;

    // Use this for initialization
    void Start () {
        flowchart = GetComponent<Flowchart>();
        menuDialog = Fungus.MenuDialog.GetMenuDialog();
    }
	
	// Update is called once per frame
	void Update () {
        previouslyExecuting = currentlyExecuting;
        currentlyExecuting = flowchart.HasExecutingBlocks();

        previouslyMenuActive = currentlyMenuActive;
        currentlyMenuActive = menuDialog.DisplayedOptionsCount > 0;

        bool onStartDialog = currentlyExecuting && !previouslyExecuting;
        bool onEndDialog = !currentlyExecuting && previouslyExecuting;
        bool onStartMenu = currentlyMenuActive && !previouslyMenuActive;
        bool onEndMenu = !currentlyMenuActive && previouslyMenuActive;

        if (onStartDialog)
        {
            if(OnStartExecuting != null)
            {
                OnStartExecuting();
            }
        }

        if (onEndDialog)
        {
            if (OnStopExecuting != null)
            {
                OnStopExecuting();
            }
        }

        if (onStartMenu)
        {
            if (OnStartMenu != null)
            {
                OnStartMenu();
            }
        }

        if (onEndMenu)
        {
            if (OnStopMenu != null)
            {
                OnStopMenu();
            }
        }
    }
}
