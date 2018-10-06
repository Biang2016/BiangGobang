using UnityEngine;
using System.Collections;

public class Forbid : PoolObject
{
    public int[] Pos;

    public override void PoolRecycle()
    {
        base.PoolRecycle();
        Pos = new int[] { 0, 0 };
    }

    void Start()
    {

    }

    void Update()
    {

    }
}
