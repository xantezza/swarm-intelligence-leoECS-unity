using UnityEngine;
using Leopotam.Ecs;

public class BaseInitSystem : IEcsInitSystem
{
    // auto-injected fields.
    private EcsWorld _ecsWorld;

    private StaticData _staticData;
    private RuntimeData _runtimeData;

    public void Init()
    {
        var baseEntity = _ecsWorld.NewEntity();
        ref var health = ref baseEntity.Get<FoodPointSupplyComponent>();
        health.Amount = _staticData.StartedBaseFoodAmount;
    }
}