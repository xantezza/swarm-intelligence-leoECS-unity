using Leopotam.Ecs;
using UnityEngine;

public class AgentSearchForBaseSystem : IEcsRunSystem
{
    // auto-injected fields.
    private EcsWorld _ecsWorld;

    private StaticData _staticData;
    private RuntimeData _runtimeData;
    private EcsFilter<DistanceDataAgentComponent, EntityMoveComponent, BaseSearchAgentComponent> _agentFilter;

    public void Run()
    {
        foreach (var _agentIndex in _agentFilter)
        {
            ref var _agentDistanceDataComponent = ref _agentFilter.Get1(_agentIndex);
            _agentDistanceDataComponent.DistanceToBase++;
            _agentDistanceDataComponent.DistanceToFood++;

            ref var _agentMoveComponent = ref _agentFilter.Get2(_agentIndex);

            var distance = Vector2.Distance(_agentMoveComponent.ReferenceTransform.position, _runtimeData.BasePoint.position);

            if (distance < 0.2f)
            {
                _agentDistanceDataComponent.DistanceToBase = 0;

                _agentMoveComponent.Direction *= -1;

                var b = _agentFilter.GetEntity(_agentIndex);
                b.Del<BaseSearchAgentComponent>();
                b.Get<FoodSearchAgentComponent>();
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
                        if (_agentDistanceDataComponent.DistanceToBase + _runtimeData.TalkDistance * 10 < _anotherAgentDistanceDataComponent.DistanceToBase)
                        {
                            _anotherAgentDistanceDataComponent.DistanceToBase = _agentDistanceDataComponent.DistanceToBase + (int)(_runtimeData.TalkDistance * 10);

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