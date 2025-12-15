 using System;
 using System.Collections.Generic;
 using UnityEngine;

 class Shelf : MonoBehaviour
 {
     [SerializeField] private List<InventorySlot> slots = new();

     private void OnEnable()
     {
         slots = new List<InventorySlot>(GetComponentsInChildren<InventorySlot>());
     }



     public InventorySlot getFreeSlot()
     {
         foreach (var slot in slots)
         {
             if (slot.GetItem() == null)
             {
                 return slot;
             }
         }
         return null;
     }
 }