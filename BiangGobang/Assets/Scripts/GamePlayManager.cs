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
                    PutChess(pos[0], pos[1]);
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

    private void PutChess(int posX, int posY)
    {
        Chess chess;
        if (turn == 0)
        {
            chess = GameObjectPoolManager.Instance.Pool_Chess1Pool.AllocateGameObject<Chess>(transform);
            chess1Arr[posX, posY] = chess;
            chessArr[posX, posY] = chess;
            turn = 1;
        }
        else
        {
            chess = GameObjectPoolManager.Instance.Pool_Chess2Pool.AllocateGameObject<Chess>(transform);
            chess2Arr[posX, posY] = chess;
            chessArr[posX, posY] = chess;
            turn = 0;
        }
        chess.transform.position = new Vector3((posX - 3) * 1.5f, (posY - 3) * 1.5f, -0.5f);
        chess.Pos = new int[] { posX, posY };
        CheckGameOver();
    }

    private void CheckGameOver()
    {
        foreach (Chess ch in chessArr)
        {
            if (CheckChessLine(ch, Direction.Up))
            {
                GameManager.Instance.GameOver();
                break;
            }
            if (CheckChessLine(ch, Direction.Down))
            {
                GameManager.Instance.GameOver();
                break;
            }
            if (CheckChessLine(ch, Direction.Left))
            {
                GameManager.Instance.GameOver();
                break;
            }
            if (CheckChessLine(ch, Direction.Right))
            {
                GameManager.Instance.GameOver();
                break;
            }
            if (CheckChessLine(ch, Direction.LeftDown))
            {
                GameManager.Instance.GameOver();
                break;
            }
            if (CheckChessLine(ch, Direction.RightDown))
            {
                GameManager.Instance.GameOver();
                break;
            }
        }
    }

    enum Direction
    {
        Up,
        Down,
        Left,
        Right,
        LeftDown,
        RightDown
    }

    private bool CheckChessLine(Chess chess, Direction direction, List<Chess> temp_ChessGroup = null)
    {
        if (temp_ChessGroup == null)
        {
            temp_ChessGroup = new List<Chess>();
        }

        if (chess == null) return false;
        temp_ChessGroup.Add(chess);
        if (temp_ChessGroup.Count >= GameManager.Instance.GameWinChessNum) return true;
        int[] pos = chess.Pos;
        Chess nextChess = null;
        switch (direction)
        {
            case Direction.Up:
                if (pos[1] > 0) nextChess = chessArr[pos[0], pos[1] - 1];
                break;
            case Direction.Down:
                if (pos[1] < chessArr.GetLength(1) - 1) nextChess = chessArr[pos[0], pos[1] + 1];
                break;
            case Direction.Left:
                if (pos[0] > 0) nextChess = chessArr[pos[0] - 1, pos[1]];
                break;
            case Direction.Right:
                if (pos[0] < chessArr.GetLength(0) - 1) nextChess = chessArr[pos[0] + 1, pos[1]];
                break;
            case Direction.LeftDown:
                if (pos[0] > 0 && pos[1] < chessArr.GetLength(1) - 1) nextChess = chessArr[pos[0] - 1, pos[1] + 1];
                break;
            case Direction.RightDown:
                if (pos[0] < chessArr.GetLength(0) - 1 && pos[1] < chessArr.GetLength(1) - 1) nextChess = chessArr[pos[0] + 1, pos[1] + 1];
                break;
        }
        if (nextChess != null)
        {
            return CheckChessLine(nextChess, direction, temp_ChessGroup);
        }
        return false;
    }

    public void ResetGameBoard()
    {
        foreach (Chess ch in chessArr)
        {
            if (ch) ch.PoolRecycle();
        }

        for (int i = 0; i < chessArr.GetLength(0); i++)
        {
            for (int j = 0; j < chessArr.GetLength(1); j++)
            {
                chessArr[i, j] = null;
            }
        }

        for (int i = 0; i < chess1Arr.GetLength(0); i++)
        {
            for (int j = 0; j < chess1Arr.GetLength(1); j++)
            {
                chess1Arr[i, j] = null;
            }
        }

        for (int i = 0; i < chess2Arr.GetLength(0); i++)
        {
            for (int j = 0; j < chess2Arr.GetLength(1); j++)
            {
                chess2Arr[i, j] = null;
            }
        }
    }

}


