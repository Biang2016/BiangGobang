using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GamePlayManager : MonoSingletion<GamePlayManager>
{
    public GameObject ChessBoard;

    public Chess[,] chess1Arr = new Chess[7, 7];
    public Chess[,] chess2Arr = new Chess[7, 7];
    public Chess[,] chessArr = new Chess[7, 7];

    void Start()
    {
    }

    float gridSize = 1.5f;
    int turn = 0;
    void Update()
    {
        UpdatePutChess();
        UpdateMouseHover();
    }

    int[] lastFocusPos;
    MouseHoverBox currentMouseHoverBox;
    private void UpdateMouseHover()
    {
        if (GameManager.Instance.GameState != GameManager.GameStates.Playing)
        {
            if (currentMouseHoverBox) currentMouseHoverBox.PoolRecycle();
            lastFocusPos = null;
            return;
        }
        int[] pos = getClickPosition();
        if (lastFocusPos != null && pos != null && lastFocusPos[0] == pos[0] && lastFocusPos[1] == pos[1])
        {
            return;
        }
        else
        {
            if (pos != null)
            {
                lastFocusPos = pos;
                if (currentMouseHoverBox) currentMouseHoverBox.PoolRecycle();
                currentMouseHoverBox = GameObjectPoolManager.Instance.Pool_MouseHoverBoxPool.AllocateGameObject<MouseHoverBox>(transform);
                currentMouseHoverBox.transform.position = new Vector3((pos[0] - 3) * 1.5f, (pos[1] - 3) * 1.5f, -0.5f);
            }
        }
    }

    private void UpdatePutChess()
    {
        if (GameManager.Instance.GameState == GameManager.GameStates.Playing)
        {
            if (Input.GetMouseButtonDown(0))
            {
                int[] pos = getClickPosition();
                if (pos == null) return;
                if (!chessArr[pos[0], pos[1]])
                {
                    Chess chess;
                    if (turn == 0)
                    {
                        chess = GameObjectPoolManager.Instance.Pool_Chess1Pool.AllocateGameObject<Chess>(transform);
                        chess1Arr[pos[0], pos[1]] = chess;
                        chessArr[pos[0], pos[1]] = chess;
                        turn = 1;
                    }
                    else
                    {
                        chess = GameObjectPoolManager.Instance.Pool_Chess2Pool.AllocateGameObject<Chess>(transform);
                        chess2Arr[pos[0], pos[1]] = chess;
                        chessArr[pos[0], pos[1]] = chess;
                        turn = 0;
                    }
                    chess.transform.position = new Vector3((pos[0] - 3) * 1.5f, (pos[1] - 3) * 1.5f, -0.5f);
                }
            }
        }
    }

    private int[] getClickPosition()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit))
        {
            int x = Mathf.RoundToInt(hit.point.x / gridSize);
            int y = Mathf.RoundToInt(hit.point.y / gridSize);
            if (x < -3 || y < -3 || x > 3 || y > 3)
            {
                return null;
            }
            return new int[] { x + 3, y + 3 };
        }
        return null;
    }
}


