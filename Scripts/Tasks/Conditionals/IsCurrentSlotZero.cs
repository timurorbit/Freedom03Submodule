using Opsive.BehaviorDesigner.Runtime.Tasks;
using Opsive.BehaviorDesigner.Runtime.Tasks.Conditionals;
using Opsive.GraphDesigner.Runtime.Variables;
using UnityEngine;

public class IsCurrentSlotZero : Conditional
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
        if (currentIndex < 0)
        {
            return TaskStatus.Failure;
        }

        return currentIndex == 0 ? TaskStatus.Success : TaskStatus.Failure;
    }
}
