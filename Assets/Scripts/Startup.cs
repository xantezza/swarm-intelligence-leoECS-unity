using Leopotam.Ecs;
using Leopotam.Ecs.UnityIntegration;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Startup : MonoBehaviour
{
    private EcsWorld _ecsWorld;
    private EcsSystems _initSystems;
    private EcsSystems _updateSystems;
    [SerializeField] private StaticData _staticData;
    [SerializeField] private RuntimeData _runtimeData;

    private void Start()
    {
        _ecsWorld = new EcsWorld();

        _initSystems = new EcsSystems(_ecsWorld)
               .Add(new BaseInitSystem())
               //.Add(new FoodInitSystem())
               .Add(new SpawnInitSystem())
               .Inject(_staticData)
               .Inject(_runtimeData);
        _updateSystems = new EcsSystems(_ecsWorld)
               .Add(new MovementSystem())
               .Add(new AgentSearchForFoodSystem())
               .Add(new AgentSearchForBaseSystem())
               .Inject(_staticData)
               .Inject(_runtimeData);
#if UNITY_EDITOR
        Leopotam.Ecs.UnityIntegration.EcsWorldObserver.Create(_ecsWorld);
#endif

        _initSystems.ProcessInjects();
        _updateSystems.ProcessInjects();

        _initSystems.Init();
        _updateSystems.Init();

#if UNITY_EDITOR
        Leopotam.Ecs.UnityIntegration.EcsSystemsObserver.Create(_initSystems);
        Leopotam.Ecs.UnityIntegration.EcsSystemsObserver.Create(_updateSystems);
#endif
    }

    public void Update()
    {
        _updateSystems.Run();
    }
}