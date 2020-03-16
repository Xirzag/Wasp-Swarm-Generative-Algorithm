using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static ArchitectureList.Architecture;

public class Simulation : MonoBehaviour
{

    [SerializeField]private int numberOfWasp = 10;
    [SerializeField]private int iterationsPerFrame = 1;
    [SerializeField]private Vector3Int simulationDimensions = Vector3Int.one * 80;
    [SerializeField]private GameObject waspPrefab;
    [SerializeField]private GameObject brickPrefab;

    private GameObject[] waspsGo;

    private int[][][] board;
    private GameObject[][][] boardGo;
    private Wasp[] wasps;
    List<Rule> rules;

    private bool updateBricksUI = true;

    private const float AIR=0, BRICK1=1, BRICK2=2;

    Vector3Int RandomVector3Int(Vector3Int min, Vector3Int max) {
        return new Vector3Int(UnityEngine.Random.Range(min[0], max[0]),
                            UnityEngine.Random.Range(min[0], max[0]),
                            UnityEngine.Random.Range(min[0], max[0]));
    }

    bool simulationRunning = false;
    private float alpha = .5f;

    // Start is called before the first frame update
    public void StartSimulation()
    {
        board = new int[simulationDimensions.x][][];
        boardGo = new GameObject[simulationDimensions.x][][];
        for(int i = 0; i < simulationDimensions.x; i++) {
            board[i] = new int[simulationDimensions.y][];
            boardGo[i] = new GameObject[simulationDimensions.y][];
            for(int j = 0; j < simulationDimensions.y; j++) {
                board[i][j] = new int[simulationDimensions.z];
                boardGo[i][j] = new GameObject[simulationDimensions.z];
                for(int k = 0; k < simulationDimensions.z; k++) {
                    boardGo[i][j][k] = Instantiate(brickPrefab, new Vector3Int(i,j,k), Quaternion.identity);
                    boardGo[i][j][k].SetActive(false);
                }
            }
        }
        board[simulationDimensions.x / 2][simulationDimensions.y / 2][simulationDimensions.z / 2] = 1;

        waspsGo = new GameObject[numberOfWasp];
        wasps = new Wasp[numberOfWasp];
        for(int i = 0; i < numberOfWasp; i++) {
            wasps[i] = new Wasp(RandomVector3Int(Vector3Int.zero, simulationDimensions));
            waspsGo[i] = Instantiate(waspPrefab, wasps[i].position, Quaternion.identity);
        }

        simulationRunning = true;
        updateBricksUI = true;

    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.P))
            simulationRunning = !simulationRunning;

        if(Input.GetKeyDown(KeyCode.C)) {
            alpha = alpha == 1? .5f : 1;
            updateBricksInGame();
        }
            

        if(simulationRunning)
            SimulateFps();
    }

    public void SetRules(List<Rule> rules) {
        this.rules = rules;
    }

    private class Wasp{
        public Vector3Int position;
        public int putRefuses = 0;

        public Wasp(Vector3Int pos) {
            this.position = pos;
        }

    }

    void SimulateFps() {
        for(int i = 0; i < iterationsPerFrame; i++)
            SimulateIter();
            
        MoveWaspsInGame();
        if(updateBricksUI) {
            updateBricksUI = false;
            updateBricksInGame();
        }
    }

    private void updateBricksInGame()
    {
        Color brick1Color = new Color(1,1,1, alpha);
        Color brick2Color = new Color(1,0,0, alpha);
        for(int i = 0; i < simulationDimensions.x; i++) {
            for(int j = 0; j < simulationDimensions.y; j++) {
                for(int k = 0; k < simulationDimensions.z; k++) {
                    if(board[i][j][k] != AIR)
                        boardGo[i][j][k].SetActive(true);
                        boardGo[i][j][k].GetComponent<MeshRenderer>().material.color = board[i][j][k] == BRICK1? brick1Color : brick2Color;
                }
            }
        }
    }

    void SimulateIter() {
        MoveWasps();
    }

    private void MoveWaspsInGame()
    {
        for (int i = 0; i < numberOfWasp; i++)
        {
            waspsGo[i].transform.position = wasps[i].position;
        }
    }

    void MoveWasps() {
        foreach (Wasp w in wasps)
        {
            Vector3Int move = w.position + RandomVector3Int(Vector3Int.one * -1, Vector3Int.one * 2);
            if(isInsideBoard(move) && GetBoardPos(move) == AIR) {
                w.position = move;
                // w.position = new Vector3Int(5,4,5);
            }

            if(!isInBorder(w.position))
                if(PutBricks(w))
                    updateBricksUI = true;

        }
    }

    private bool PutBricks(Wasp w)
    {
        List<int> bricks = rules[0].bricks;

        Vector3Int w_pos = w.position;
        int[,] around = SenseAround(w_pos);

        for (int offset = 0; offset < bricks.Count; offset += 9 * 3)
        {

            for (int rot = 0; rot < 4; rot++)
            {

                bool canPutBrick = true;

                for (int brickIndex = 0; brickIndex < 27; brickIndex++)
                {

                    int y = brickIndex / 9;
                    int p = brickIndex % 9;

                    if (y == 1 && p == 4)
                        continue;

                    if (bricks[offset + brickIndex] != around[rot, brickIndex])
                    {
                        canPutBrick = false;
                        break;
                    }

                }

                if (canPutBrick)
                {
                    int brickToPut = bricks[offset + 9 + 4];
                    Debug.Log("Putting " + brickToPut);
                    board[w.position.x][w.position.y][w.position.z] = brickToPut;
                    return true;
                }

            }

        }

        return false;
    }

    private int[,] SenseAround(Vector3Int w_pos)
    {
        int[,] around = new int[4,27];
        int i = 0;
        for (int y = 1; y >= -1; y--)
        {
            for (int x = -1; x <= 1; x++)
            {
                for (int z = -1; z <= 1; z++)
                {
                    around[0, i] = board[w_pos.x + x][w_pos.y + y][w_pos.z + z];
                    around[1, i] = board[w_pos.x + z][w_pos.y + y][w_pos.z - x];
                    around[2, i] = board[w_pos.x - x][w_pos.y + y][w_pos.z - z];
                    around[3, i] = board[w_pos.x - z][w_pos.y + y][w_pos.z + x];
                    i++;
                }
            }
        }

        return around;
    }

    private bool isInsideBoard(Vector3Int move)
    {
        return move.x >= 0 && move.y >= 0 && move.z >= 0 &&
            move.x < simulationDimensions.x && move.y < simulationDimensions.y &&
            move.z < simulationDimensions.z;
    }

    private bool isInBorder(Vector3Int move)
    {
        return move.x == 0 || move.y == 0 || move.z == 0 ||
            move.x == simulationDimensions.x - 1|| move.y == simulationDimensions.y - 1||
            move.z == simulationDimensions.z - 1;
    }

    private bool IsABlock(Vector3Int v) {
        if(isInsideBoard(v)) {
            int brick = board[v.x][v.y][v.z];
            return brick == BRICK2 || brick == BRICK1;
        }
        return false;
    }

    private int GetBoardPos(Vector3Int v){
        return board[v.x][v.y][v.z];
    }

}
