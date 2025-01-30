using System;
using Unity.Behavior;
using UnityEngine;

[Serializable, Unity.Properties.GeneratePropertyBag]
[Condition(name: "Player is too close to restricted area", story: "[Player] is too close to target [area]", category: "Conditions", id: "97d105af343504b89fa1ca25c24cd3ad")]
public partial class PlayerIsTooCloseToRestrictedAreaCondition : Condition
{
    [SerializeReference] public BlackboardVariable<GameObject> Player;
    [SerializeReference] public BlackboardVariable<Transform> Area;

    public override bool IsTrue()
    {
        return true;
    }

    public override void OnStart()
    {
    }

    public override void OnEnd()
    {
    }
}
