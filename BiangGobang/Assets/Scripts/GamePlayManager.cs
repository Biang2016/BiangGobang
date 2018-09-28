using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GamePlayManager : MonoBehaviour {

    public GameObject ChessBoard;
    public GameObject Chess1;
    public GameObject Chess2;

    public GameObject[,] chess1Arr = new GameObject[7, 7];
    public GameObject[,] chess2Arr = new GameObject[7, 7];
    public GameObject[,] chessArr = new GameObject[7, 7];

    void Start() {

    }

    float gridSize = 1.5f;
    int turn = 0;
    void Update() {
        moveInChess();
    }

    private void moveInChess() {
        if (Input.GetMouseButtonUp(0)) {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit)) {
                int x = Mathf.RoundToInt(hit.point.x / gridSize);
                int y = Mathf.RoundToInt(hit.point.y / gridSize);
                if (x < -3 || y < -3 || x > 3 || y > 3) {
                    return;
                }
                if (!chessArr[x + 3, y + 3]) {
                    GameObject chess;
                    if (turn == 0) {
                        chess = Instantiate(Chess1, gameObject.transform.parent);
                        chess1Arr[x + 3, y + 3] = chess;
                        chessArr[x + 3, y + 3] = chess;
                        turn = 1;
                    } else {
                        chess = Instantiate(Chess2, gameObject.transform.parent);
                        chess2Arr[x + 3, y + 3] = chess;
                        chessArr[x + 3, y + 3] = chess;
                        turn = 0;
                    }
                    chess.transform.position = new Vector3(x * 1.5f, y * 1.5f, -0.5f);
                }
            }

        }
    }
}


