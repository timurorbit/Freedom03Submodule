using Opsive.GraphDesigner.Runtime.Variables;
using UnityEngine;

public class GoToTransform : GoToActionBase
{
    [SerializeField] SharedVariable<Transform> Destination;

    protected override Vector3? GetDestinationPosition()
    {
        if (Destination == null || Destination.Value == null)
            return null;
        return Destination.Value.position;
    }
}
