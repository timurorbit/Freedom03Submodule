using _Game.Scripts.Behaviours;
using Opsive.BehaviorDesigner.Runtime.Tasks;
using Opsive.BehaviorDesigner.Runtime.Tasks.Actions;
using Opsive.GraphDesigner.Runtime.Variables;
using UnityEngine;

public class OccupyNextSpotAction : Action
{
    [Tooltip("The current slot GameObject in the line (will be updated to next slot)")]
    [SerializeField] private SharedVariable<GameObject> currentSlot;

    private CharacterBehaviour characterBehaviour;

    public override void OnAwake()
    {
        base.OnAwake();
        characterBehaviour = gameObject.GetComponent<CharacterBehaviour>();
    }

    public override TaskStatus OnUpdate()
    {
        if (characterBehaviour == null || currentSlot == null || currentSlot.Value == null)
            return TaskStatus.Failure;

        Line line = currentSlot.Value.GetComponentInParent<Line>();
        if (line == null)
        {
            return TaskStatus.Failure;
        }

        int currentIndex = line.GetSpotIndex(currentSlot.Value);
        if (currentIndex < 0)
        {
            return TaskStatus.Failure;
        }

        // Can't move forward if already at the front (index 0)
        if (currentIndex == 0)
        {
            return TaskStatus.Failure;
        }

        int nextIndex = currentIndex - 1;
        
        // Check if next slot is available
        if (line.IsSlotTaken(nextIndex))
        {
            return TaskStatus.Failure;
        }

        // Release current spot
        characterBehaviour.ReleaseSpot(currentSlot.Value);

        // Get the next spot GameObject
        GameObject nextSpotObject = line.GetSpotGameObject(nextIndex);
        if (nextSpotObject == null)
        {
            return TaskStatus.Failure;
        }

        // Occupy the next spot
        characterBehaviour.OccupySpot(nextSpotObject);

        // Update the shared variable to the new spot
        currentSlot.Value = nextSpotObject;

        return TaskStatus.Success;
    }
}
