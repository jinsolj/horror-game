using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Fungus
{

    [CommandInfo("Custom",
                 "Add Item",
                 "Add an item to the player's inventory.")]
    public class FungusAddItem : Command
    {

        Flowchart flowchart;
        ItemInfo info;

        [Tooltip("Item to add.")]
        [SerializeField]
        protected Item item;

        [Tooltip("Quantity of item to add.")]
        [SerializeField]
        protected int quantity = 1;

        #region Public members

        public override void OnEnter()
        {
            flowchart = GetFlowchart();
            info = ItemDatabase.GetInfo(item);

            PlayerManager.instance.inventory.AddItem(item, quantity);
            flowchart.SetStringVariable("LastItemAddedName", info.itemName);
            Continue();
        }

        public override string GetSummary()
        {
            return "Item: " + item.ToString();
        }

        public override Color GetButtonColor()
        {
            return new Color32(242, 209, 176, 255);
        }

        #endregion
    }
}
