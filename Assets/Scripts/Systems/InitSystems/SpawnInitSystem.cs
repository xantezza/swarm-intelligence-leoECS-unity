using Leopotam.Ecs;
using UnityEngine;

public class SpawnInitSystem : IEcsInitSystem
{
    // auto-injected fields.
    private StaticData _staticData;

    private RuntimeData _runtimeData;
    private EcsWorld _ecsWorld;

    public void Init()
    {
        InstantiateAgents(_staticData.StartedAgentsQuantity, _staticData.AgentsPrefabs);
    }

    public void InstantiateAgents(int objectsCount, GameObject[] agentPrefabs)
    {
        for (int i = 0; i < objectsCount; i++)
        {
            var randomAgentPrefab = agentPrefabs[(int)Random.Range(0, agentPrefabs.Length)];
            var agent = Object.Instantiate(randomAgentPrefab);
            Vector2 agentPosition = _runtimeData.BasePoint.position;
            agent.transform.position = agentPosition;

            var agentEntity = _ecsWorld.NewEntity();
            ref var agentMoveComponent = ref agentEntity.Get<EntityMoveComponent>();
            ref var agentDistanceDataComponent = ref agentEntity.Get<DistanceDataAgentComponent>();
            ref var agentFoodSearchComponent = ref agentEntity.Get<FoodSearchAgentComponent>();

            agentMoveComponent.ReferenceTransform = agent.transform;
            agentMoveComponent.Direction = new Vector2(Random.Range(-1f, 1f), Random.Range(-1f, 1f)).normalized;
        }
    }
}