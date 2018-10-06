using UnityEngine;

public class GameObjectPoolManager : MonoSingletion<GameObjectPoolManager>
{
    private GameObjectPoolManager()
    {
    }

    public GameObjectPool[] Pool_ChessPool;
    public PoolObject[] ChessPrefab;

    public GameObjectPool[] Pool_MouseHoverBoxPool;
    public PoolObject[] MouseHoverBoxPrefab;

    public GameObjectPool Pool_ForbidPool;
    public PoolObject ForbidPrefab;

    void Awake()
    {
        for (int i = 0; i < Pool_ChessPool.Length; i++)
        {
            Pool_ChessPool[i].Initiate((ChessPrefab[i]), 20);
        }

        for (int i = 0; i < Pool_MouseHoverBoxPool.Length; i++)
        {
            Pool_MouseHoverBoxPool[i].Initiate((MouseHoverBoxPrefab[i]), 2);
        }

        Pool_ForbidPool.Initiate((ForbidPrefab), 20);
    }
}