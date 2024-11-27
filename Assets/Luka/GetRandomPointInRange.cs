using System;
using Unity.Behavior;
using UnityEngine;
using Action = Unity.Behavior.Action;
using Unity.Properties;
using Unity.Mathematics;

[Serializable, GeneratePropertyBag]
[NodeDescription(name: "GetRandomPointInRange", story: "Find random [point] in range of [Min] and [Max]", category: "Action", id: "b20ea864dcd800591d7513c266ce2d4a")]
public partial class GetRandomPointInRangeAction : Action
{
    [SerializeReference] public BlackboardVariable<Vector3> Point;
    [SerializeReference] public BlackboardVariable<Vector3> Min;
    [SerializeReference] public BlackboardVariable<Vector3> Max;
    protected override Status OnStart()
    {
                
        return Status.Running;
    }

    protected override Status OnUpdate()
    {
        //Only need a random for the x (at least for now)
        float randomX = UnityEngine.Random.Range(Min.Value.x, Max.Value.x);

        Point.Value = new Vector3(randomX, 0, 0);
        return Status.Success;
    }

    protected override void OnEnd()
    {
    }
}

