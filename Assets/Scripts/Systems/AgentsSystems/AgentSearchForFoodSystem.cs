using Leopotam.Ecs;
using UnityEngine;

public class AgentSearchForFoodSystem : IEcsRunSystem
{
    // auto-injected fields.
    private EcsWorld _ecsWorld;

    private StaticData _staticData;
    private RuntimeData _runtimeData;
    private EcsFilter<DistanceDataAgentComponent, EntityMoveComponent, FoodSearchAgentComponent> _agentFilter;

    public void Run()
    {
        foreach (var _agentIndex in _agentFilter)
        {
            ref var _agentDistanceDataComponent = ref _agentFilter.Get1(_agentIndex);
            _agentDistanceDataComponent.DistanceToBase++;
            _agentDistanceDataComponent.DistanceToFood++;
            ref var _agentMoveComponent = ref _agentFilter.Get2(_agentIndex);

            foreach (var _foodPoint in _runtimeData.FoodPoints)
            {
                var distance = Vector2.Distance(_agentMoveComponent.ReferenceTransform.position, _foodPoint.position);

                if (distance < 0.2f)
                {
                    _agentDistanceDataComponent.DistanceToFood = 0;

                    _agentMoveComponent.Direction *= -1;

                    var b = _agentFilter.GetEntity(_agentIndex);
                    b.Del<FoodSearchAgentComponent>();
                    b.Get<BaseSearchAgentComponent>();
                }
            }

            foreach (var _anotherAgentIndex in _agentFilter)
            {
                if (_agentIndex != _anotherAgentIndex)
                {
                    ref var _anotherAgentMoveComponent = ref _agentFilter.Get2(_anotherAgentIndex);
                    if (Vector2.Distance(_agentMoveComponent.ReferenceTransform.position,
                        _anotherAgentMoveComponent.ReferenceTransform.position) < _runtimeData.TalkDistance)
                    {
                        ref var _anotherAgentDistanceDataComponent = ref _agentFilter.Get1(_anotherAgentIndex);

                        if (_agentDistanceDataComponent.DistanceToFood + _runtimeData.TalkDistance * 10 < _anotherAgentDistanceDataComponent.DistanceToFood)
                        {
                            _anotherAgentDistanceDataComponent.DistanceToFood = _agentDistanceDataComponent.DistanceToFood + (int)(_runtimeData.TalkDistance * 10);

                            _anotherAgentMoveComponent.Direction =
                                (_agentMoveComponent.ReferenceTransform.position
                                - _anotherAgentMoveComponent.ReferenceTransform.position).normalized;
                        }
                    }
                }
            }
        }
    }
}