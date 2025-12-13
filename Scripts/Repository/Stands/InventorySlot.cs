using UnityEngine;

public class InventorySlot : Table
{
    [SerializeField]
    private GameObject item;

    [SerializeField] private Transform objectPosition;

    public bool Add(GameObject item)
    {
        // check if there is already an item
        //tweening to position
    }

    public GameObject Take()
    {
        // unset parent of object
        // return item and set to null
    }
}