using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GamePlayManager : MonoSingletion<GamePlayManager>
{
    public GameObject ChessBoard;

    public Chess[,] chessArr = new Chess[7, 7];
    public Forbid[,] forbidArr = new Forbid[7, 7];

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
                currentMouseHoverBox = GameObjectPoolManager.Instance.Pool_MouseHoverBoxPool[turn].AllocateGameObject<MouseHoverBox>(transform);
                currentMouseHoverBox.transform.position = new Vector3((pos[0] - 3) * 1.5f, (pos[1] - 3) * 1.5f, -0.5f);
            }
        }
    }

    private void UpdatePutChess()
    {
        if (GameManager.Instance.GameState == GameManager.GameStates.Playing)
        {
            if (Input.GetMouseButtonUp(0))
            {
                int[] pos = getClickPosition();
                if (pos == null) return;
                if (!chessArr[pos[0], pos[1]])
                {
                    PutChess(pos[0], pos[1]);
                    if (forbidArr[pos[0], pos[1]])
                    {
                        RemoveForbid(pos[0], pos[1]);
                    }
                }
            }

            if (Input.GetMouseButtonUp(1))
            {
                int[] pos = getClickPosition();
                if (pos == null) return;
                if (!chessArr[pos[0], pos[1]])
                {
                    if (!forbidArr[pos[0], pos[1]])
                    {
                        PutForbid(pos[0], pos[1]);
                    }
                    else
                    {
                        RemoveForbid(pos[0], pos[1]);
                    }
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

            return new int[] {x + 3, y + 3};
        }

        return null;
    }

    private void PutChess(int posX, int posY)
    {
        Chess chess;
        chess = GameObjectPoolManager.Instance.Pool_ChessPool[turn].AllocateGameObject<Chess>(transform);
        chessArr[posX, posY] = chess;

        chess.transform.position = new Vector3((posX - 3) * 1.5f, (posY - 3) * 1.5f, -0.5f);
        chess.Pos = new int[] {posX, posY};
        bool isOver = CheckGameOver();

        if (!isOver)
        {
            turn = turn == 0 ? 1 : 0;
            GameManager.Instance.PlayerA.SetActive(turn == 0);
            GameManager.Instance.PlayerB.SetActive(turn == 1);
        }
    }

    private void PutForbid(int posX, int posY)
    {
        Forbid fb = GameObjectPoolManager.Instance.Pool_ForbidPool.AllocateGameObject<Forbid>(transform);
        fb.transform.position = new Vector3((posX - 3) * 1.5f, (posY - 3) * 1.5f, -0.5f);
        fb.Pos = new int[] {posX, posY};
        forbidArr[posX, posY] = fb;
    }

    private void RemoveForbid(int posX, int posY)
    {
        Forbid fb = forbidArr[posX, posY];
        if (fb != null)
        {
            fb.PoolRecycle();
            forbidArr[posX, posY] = null;
        }
    }

    private bool CheckGameOver()
    {
        for (int i = 0; i < chessArr.GetLength(0); i++)
        {
            for (int j = 0; j < chessArr.GetLength(1); j++)
            {
                Chess ch = chessArr[i, j];
                if (ch == null)
                {
                    continue;
                }
                else
                {
                    if (CheckAllDirections(ch))
                    {
                        GameManager.Instance.GameOver(turn == 0);
                        return true;
                    }
                }
            }
        }

        return false;
    }

    enum Direction
    {
        Up,
        Down,
        Left,
        Right,
        LeftDown,
        RightDown,
        LeftUp,
        RightUp,
    }

    private bool CheckAllDirections(Chess ch)
    {
        if (CheckChessLine(ch, Direction.Up))
            return true;

        if (CheckChessLine(ch, Direction.Down))
            return true;

        if (CheckChessLine(ch, Direction.Left))
            return true;

        if (CheckChessLine(ch, Direction.Right))
            return true;

        if (CheckChessLine(ch, Direction.LeftDown))
            return true;

        if (CheckChessLine(ch, Direction.RightDown))
            return true;

        if (CheckChessLine(ch, Direction.LeftUp))
            return true;

        if (CheckChessLine(ch, Direction.RightUp))
            return true;

        return false;
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
            case Direction.LeftUp:
                if (pos[0] > 0 && pos[1] > 0) nextChess = chessArr[pos[0] - 1, pos[1] - 1];
                break;
            case Direction.RightUp:
                if (pos[0] < chessArr.GetLength(0) - 1 && pos[1] > 0) nextChess = chessArr[pos[0] + 1, pos[1] - 1];
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
        GameManager.Instance.PlayerA.SetActive(true);
        GameManager.Instance.PlayerB.SetActive(false);

        foreach (Chess ch in chessArr)
        {
            if (ch) ch.PoolRecycle();
        }

        foreach (Forbid fb in forbidArr)
        {
            if (fb) fb.PoolRecycle();
        }

        for (int i = 0; i < chessArr.GetLength(0); i++)
        {
            for (int j = 0; j < chessArr.GetLength(1); j++)
            {
                chessArr[i, j] = null;
            }
        }

        for (int i = 0; i < forbidArr.GetLength(0); i++)
        {
            for (int j = 0; j < forbidArr.GetLength(1); j++)
            {
                forbidArr[i, j] = null;
            }
        }
    }
}