using UnityEngine;
using Leopotam.Ecs;

public class FoodInitSystem : IEcsInitSystem
{
    // auto-injected fields.
    private EcsWorld _ecsWorld;

    private StaticData _staticData;
    private RuntimeData _runtimeData;

    public void Init()
    {
        foreach (var foodPoint in _runtimeData.FoodPoints)
        {
            var foodEntity = _ecsWorld.NewEntity();
            ref var moveComponent = ref foodEntity.Get<EntityMoveComponent>();
            moveComponent.ReferenceTransform = foodPoint;
            moveComponent.Direction = new Vector2(Random.Range(-1f, 1f), Random.Range(-1f, 1f)).normalized * Random.Range(0.1f, 0.3f);
            Debug.Log(moveComponent.Direction);
        }
    }
}