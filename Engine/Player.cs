using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Engine
{
    public class Player : LivingCreature
    {
        public int Gold { get; set; }
        public int ExperiencePoints { get; set; }
        public int Level { get; set; }
        public Location CurrentLocation { get; set; }
        public List<InventoryItem> Inventory { get; set; }
        public List<PlayerQuest> Quests { get; set; }

        public Player(int currentHitPoints, int maximumHitPoints, int gold, int experiencePoints, int level)
            : base(currentHitPoints, maximumHitPoints)
        {
            Gold = gold;
            ExperiencePoints = experiencePoints;
            Level = level;

            Inventory = new List<InventoryItem>();
            Quests = new List<PlayerQuest>();
        }

        public bool HasRequiredItemToEnterThisLocation(Location location)
        {
            if(location.ItemRequiredToEnter == null)
            {
                // There is no required item for this location, so return true
                return true;
            }

            // See if the player has the required item in their inventory
            foreach(InventoryItem ii in Inventory)
            {
                if(ii.Details.ID == location.ItemRequiredToEnter.ID)
                {
                    // We found the required item
                    return true;
                }
            }

            // We didn't find the required item in the player's inventory
            return false;
        }

        public bool HasThisQuest(Quest quest)
        {
            foreach(PlayerQuest playerQuest in Quests)
            {
                if(playerQuest.Details.ID == quest.ID)
                {
                    return true;
                }
            }

            return false;
        }

        public bool CompletedThisQuest(Quest quest)
        {
            foreach(PlayerQuest playerQuest in Quests)
            {
                if(playerQuest.Details.ID == quest.ID)
                {
                    return playerQuest.IsCompleted;
                }
            }

            return false;
        }

        public bool hasAllQuestCompletionItems(Quest quest)
        {
            // See if the player has all the items to complete the quest
            foreach (QuestCompletionItem qci in quest.QuestCompletionItems)
            {
                bool foundItemInPlayersInventory = false;

                // Check each item in the player's inventory, to see if they have it, and enough of it
                foreach (InventoryItem ii in Inventory)
                {
                    if(ii.Details.ID == qci.Details.ID)
                    {
                        foundItemInPlayersInventory = true;

                        if(ii.Quantity < qci.Quantity) // The player does not have enough of this item to complete the quest
                        {
                            return false;
                        }
                    }
                }
                // The player does not have all the required items to complete the quest
                if (!foundItemInPlayersInventory)
                {
                    return false;
                }
            }
            // The player has all the required items to complete the quest
            return true;
        }

        public void RemoveQuestCompletionItems(Quest quest)
        {
            foreach(QuestCompletionItem qci in quest.QuestCompletionItems)
            {
                foreach(InventoryItem ii in Inventory)
                {
                    if(ii.Details.ID == qci.Details.ID)
                    {
                        // Substract the quantity of the item from the player's inventory
                        ii.Quantity -= qci.Quantity;
                        break;
                    }
                }
            }
        }

        public void AddItemToInventory(Item itemToAdd)
        {
            foreach(InventoryItem ii in Inventory)
            {
                if(ii.Details.ID == itemToAdd.ID)
                {
                    // They have the item in their inventory, so increase the quantity by one
                    ii.Quantity++;

                    return; 
                }
            }

            // They didn't have the item, so add it to their inventory, with a quantity of 1
            Inventory.Add(new InventoryItem(itemToAdd, 1));
        }

        public void MarkQuestCompleted(Quest quest)
        {
            foreach(PlayerQuest pq in Quests)
            {
                if(pq.Details.ID == quest.ID)
                {
                    // Mark it as completed
                    pq.IsCompleted = true;

                    return;
                }
            }
        }


    }
}
