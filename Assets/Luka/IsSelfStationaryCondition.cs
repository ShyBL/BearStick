using System;
using Unity.Behavior;
using UnityEngine;

[Serializable, Unity.Properties.GeneratePropertyBag]
[Condition(name: "IsSelfStationary", story: "[Self] is a stationary enemy", category: "Conditions", id: "10220bf1c9d6833ac14aed3e261fbc60")]
public partial class IsSelfStationaryCondition : Condition
{
    [SerializeReference] public BlackboardVariable<GameObject> Self;

    public override bool IsTrue()
    {
        if (Self.Value.CompareTag("Stationary"))
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public override void OnStart()
    {
    }

    public override void OnEnd()
    {
    }
}
