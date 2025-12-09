using Opsive.BehaviorDesigner.Runtime.Tasks;
using Opsive.BehaviorDesigner.Runtime.Tasks.Conditionals;
using Opsive.GraphDesigner.Runtime.Variables;
using UnityEngine;

public class IsNextLineSlotAvailable : Conditional
{
    [Tooltip("The current slot GameObject in the line")]
    [SerializeField] private SharedVariable<GameObject> currentSlot;

    public override TaskStatus OnUpdate()
    {
        if (currentSlot == null || currentSlot.Value == null)
        {
            return TaskStatus.Failure;
        }

        Line line = currentSlot.Value.GetComponentInParent<Line>();
        if (line == null)
        {
            return TaskStatus.Failure;
        }

        int currentIndex = line.GetSpotIndex(currentSlot.Value);
        if (currentIndex <= 0)
        {
            return TaskStatus.Failure;
        }

        int nextIndex = currentIndex - 1;
        bool isNextSlotTaken = line.IsSlotTaken(nextIndex);

        return !isNextSlotTaken ? TaskStatus.Success : TaskStatus.Failure;
    }
}
