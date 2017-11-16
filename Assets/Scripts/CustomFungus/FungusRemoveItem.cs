using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Fungus
{

    [CommandInfo("Custom",
                 "Remove Item",
                 "Remove an item from the player's inventory.")]
    public class FungusRemoveItem : Command
    {

        Flowchart flowchart;
        ItemInfo info;

        [Tooltip("Item to remove.")]
        [SerializeField]
        protected Item item;

        [Tooltip("Quantity of item to remove.")]
        [SerializeField]
        protected int quantity = 1;

        #region Public members

        public override void OnEnter()
        {
            flowchart = GetFlowchart();
            info = ItemDatabase.GetInfo(item);

            PlayerManager.instance.inventory.RemoveItem(item, quantity);
            flowchart.SetStringVariable("LastItemRemovedName", info.itemName);
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
