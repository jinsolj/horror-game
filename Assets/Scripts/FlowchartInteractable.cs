using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fungus;

public class FlowchartInteractable : MonoBehaviour, Interactable {

    Flowchart flowchart;
    public List<ItemEventPair> eventTriggers;

    // Use this for initialization
    void Start () {
        flowchart = Flowchart.CachedFlowcharts[0];
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void Interact(PlayerManager player, Item item)
    {
        foreach(ItemEventPair pair in eventTriggers)
        {
            if(item == pair.item)
            {
                flowchart.ExecuteBlock(pair.blockName);
            }
        }
    }
}
