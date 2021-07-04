using Leopotam.Ecs;
using UnityEngine;

public class MovementSystem : IEcsRunSystem
{
    // auto-injected fields.
    private EcsWorld _ecsWorld;

    private StaticData _staticData;
    private RuntimeData _runtimeData;
    private EcsFilter<EntityMoveComponent> _movementFilter;

    public void Run()
    {
        foreach (var _index in _movementFilter)
        {
            ref var _moveComponent = ref _movementFilter.Get1(_index);
            _moveComponent.Direction = new Vector2(_moveComponent.Direction.x * Random.Range(0.8f, 1.2f), _moveComponent.Direction.y * Random.Range(0.8f, 1.2f)).normalized * Random.Range(0.7f, 1.3f);
            _moveComponent.ReferenceTransform.position += (Vector3)_moveComponent.Direction * _runtimeData.AgentSpeed * Time.deltaTime * Random.Range(0.8f, 1.1f);
        }
    }
}