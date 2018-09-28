using UnityEngine;

public class GameObjectPoolManager : MonoSingletion<GameObjectPoolManager>
{
    private GameObjectPoolManager()
    {
    }

    public GameObjectPool Pool_Chess1Pool;
    public PoolObject Chess1Prefab;

    public GameObjectPool Pool_Chess2Pool;
    public PoolObject Chess2Prefab;

    public GameObjectPool Pool_MouseHoverBoxPool;
    public PoolObject MouseHoverBoxPrefab;

    void Awake()
    {
        Pool_Chess1Pool.Initiate((Chess1Prefab), 20);
        Pool_Chess2Pool.Initiate((Chess2Prefab), 20);
        Pool_MouseHoverBoxPool.Initiate((MouseHoverBoxPrefab), 2);
    }
}