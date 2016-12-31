using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Networking;

public class main : NetworkBehaviour
{
    public static main Instance;
    private Quaternion orientation = Quaternion.Euler(-90, 0, 0);

    private const float TILE_SIZE = 0.053375f;
    private const float TILE_OFFSET = 0.0266875f;

    private int selectionX = -5;
    private int selectionY = -5;

    public List<GameObject> figurePrefabs;
    private List<GameObject> activeFigure;

    public Chessman[,] chessmans;
    private Chessman selectedChessman;
    public static Chessman whiteKing;
    public static Chessman blackKing;

    public static bool whiteKingIsChecked;
    public static bool blackKingIsChecked;
    public static bool doubleChecked;
    public bool[,] checkPath;

    public GameObject selectionArea;
    private GameObject selection;

    public bool[,] fieldsUnderAttackWhite;
    public bool[,] fieldsUnderAttackBlack;
    public bool isWhiteTurn = true;
    private bool moved = false;

    Vector3 step;
    Vector3 start;
    Vector3 destination;
    const float heightEtalon = 0.76f;
    double pathLength;
    double height;
    int CastlingX;
    int CastlingY;
    bool isCastling;

    private bool isMyTurn;

    //void Awake()
    //{
        
    //    Instance = this;
    //}

    // Use this for initialization
    public override void OnStartLocalPlayer()
    {
        Instance = this;
        SpawnAllChessmen();
        fieldsUnderAttackBlack = new bool[8, 8];
        fieldsUnderAttackWhite = new bool[8, 8];

        isMyTurn = true;
        if (transform.position.z > 0)
        {
            Camera.main.transform.position = new Vector3(0.216f, 1.472f, 0.9f);
            Camera.main.transform.rotation = Quaternion.Euler(30, 180, 0);
            isMyTurn = false;
        }

    }

    void SpawnChessman(int index, int x, int y)
    {
        Quaternion currentOrientation = orientation;

        if (index == 4)
        {
            currentOrientation = Quaternion.Euler(-90, -90, 0);
        }
        else if (index == 10)
        {
            currentOrientation = Quaternion.Euler(-90, 90, 0);
        }

        GameObject go = Instantiate(figurePrefabs[index], GetTileCenter(x, y), currentOrientation) as GameObject;
        go.transform.SetParent(transform);
        chessmans[x, y] = go.GetComponent<Chessman>();
        chessmans[x, y].SetPosition(x, y);

        activeFigure.Add(go);

        if (index == 0)
        {
            whiteKing = chessmans[x, y];
        }
        else if (index == 6)
        {
            blackKing = chessmans[x, y];
        }
    }

    void SpawnAllChessmen()
    {
        chessmans = new Chessman[8, 8];
        activeFigure = new List<GameObject>();
        //White
        //King
        SpawnChessman(0, 4, 0);

        //Queen
        SpawnChessman(1, 3, 0);

        //Rooks
        SpawnChessman(2, 0, 0);
        SpawnChessman(2, 7, 0);

        //Bishops
        SpawnChessman(3, 2, 0);
        SpawnChessman(3, 5, 0);

        //Knights
        SpawnChessman(4, 1, 0);
        SpawnChessman(4, 6, 0);

        //Pawns
        for (int i = 0; i < 8; i++)
            SpawnChessman(5, i, 1);

        // Spawn the Black team!

        //King
        SpawnChessman(6, 4, 7);

        //Queen
        SpawnChessman(7, 3, 7);

        //Rooks
        SpawnChessman(8, 0, 7);
        SpawnChessman(8, 7, 7);

        //Bishops
        SpawnChessman(9, 2, 7);
        SpawnChessman(9, 5, 7);

        //Knights
        SpawnChessman(10, 1, 7);
        SpawnChessman(10, 6, 7);

        //Pawns
        for (int i = 0; i < 8; i++)
            SpawnChessman(11, i, 6);
    }

    // Update is called once per frame
    void Update()
    {
        if (!isLocalPlayer)
            return;

        UpdateMousePosition();
        CheckForMove();
        ActualMove();
        UpdateSelection();

    }

    private void ActualMove()
    {
        Vector3 temp = step;

        if (moved)
        {
            float jump = 0;
            if (selectedChessman.GetType() == typeof(knight))
            {
                if (selectedChessman.transform.position.z == start.z)
                {
                    height = heightEtalon;
                }
                else
                {
                    double prevHeight = height;
                    double curPassedPath = Math.Sqrt(Math.Abs(selectedChessman.transform.position.x - start.x) *
                                                     Math.Abs(selectedChessman.transform.position.x - start.x) +
                                                     Math.Abs(selectedChessman.transform.position.z - start.z) *
                                                     Math.Abs(selectedChessman.transform.position.z - start.z));

                    height = heightEtalon + Math.Sqrt(pathLength * curPassedPath -
                                                curPassedPath * curPassedPath);
                    jump = (float)(height - prevHeight) * 60;

                }
            }

            if (step.x > 0)
            {
                if (selectedChessman.transform.position.x < destination.x)
                {
                    if (selectedChessman.GetType() == typeof(knight))
                    {
                        if (selectedChessman.isWhite)
                        {
                            temp = new Vector3(-step.y, step.x, jump);

                        }
                        else
                        {
                            temp = new Vector3(step.y, -step.x, jump);
                        }
                    }
                    selectedChessman.gameObject.transform.Translate(temp * Time.deltaTime, Space.Self);
                }
                else
                {
                    if (isCastling)
                    {
                        Castling(CastlingX, CastlingY);
                    }
                    selectedChessman.transform.position = new Vector3(destination.x, heightEtalon, destination.z);
                    selectedChessman = null;
                    moved = false;
                    CheckForEnd();
                }
            }
            else if (step.x == 0)
            {
                if (step.y < 0)
                {
                    if (selectedChessman.transform.position.z < destination.z)
                    {
                        if (selectedChessman.GetType() == typeof(knight))
                        {
                            if (selectedChessman.isWhite)
                            {
                                temp = new Vector3(-step.y, step.x, jump);
                            }
                            else
                            {
                                temp = new Vector3(step.y, -step.x, jump);
                            }
                        }
                        selectedChessman.gameObject.transform.Translate(temp * Time.deltaTime, Space.Self);
                    }
                    else
                    {
                        selectedChessman.transform.position = new Vector3(destination.x, heightEtalon, destination.z);
                        selectedChessman = null;
                        moved = false;
                        CheckForEnd();
                    }
                }
                else
                {
                    if (selectedChessman.transform.position.z > destination.z)
                    {
                        if (selectedChessman.GetType() == typeof(knight))
                        {
                            if (selectedChessman.isWhite)
                            {
                                temp = new Vector3(-step.y, step.x, jump);
                            }
                            else
                            {
                                temp = new Vector3(step.y, -step.x, jump);
                            }
                        }
                        selectedChessman.gameObject.transform.Translate(temp * Time.deltaTime, Space.Self);
                    }
                    else
                    {
                        selectedChessman.transform.position = new Vector3(destination.x, heightEtalon, destination.z);
                        selectedChessman = null;
                        moved = false;
                        CheckForEnd();
                    }
                }
            }
            else
            {
                //Debug.Log("Actual moving");
                if (selectedChessman.transform.position.x > destination.x)
                {
                    if (selectedChessman.GetType() == typeof(knight))
                    {
                        if (selectedChessman.isWhite)
                        {
                            temp = new Vector3(-step.y, step.x, jump);
                        }
                        else
                        {
                            temp = new Vector3(step.y, -step.x, jump);
                        }
                    }
                    selectedChessman.gameObject.transform.Translate(temp * Time.deltaTime, Space.Self);
                }
                else
                {
                    if (isCastling)
                    {
                        Castling(CastlingX, CastlingY);
                    }
                    selectedChessman.transform.position = new Vector3(destination.x, heightEtalon, destination.z);
                    selectedChessman = null;
                    moved = false;
                    CheckForEnd();
                }
            }
        }

    }

    private void CheckForEnd()
    {
        if (!AreAnyPossibleMoves(isWhiteTurn))
        {
            StartCoroutine(WaitForSomeSeconds());
            if (whiteKingIsChecked || blackKingIsChecked)
            {
                EndGame(true);
            }
            else
            {
                EndGame(false);
            }
            new WaitForSeconds(5000);
        }
    }

    IEnumerator WaitForSomeSeconds()
    {
        print(Time.time);
        yield return new WaitForSeconds(2f);
        print(Time.time);
    }

    private void CheckForMove()
    {
        if (Input.GetMouseButtonDown(0) && !moved)
        {
            if (selectionX >= 0 && selectionY >= 0 && selectedChessman != null)
            {
                selectedChessman.checking = true;
                bool[,] allowed = selectedChessman.PossibleMove();
                selectedChessman.checking = false;

                // Move the chessman
                if (allowed[selectionX, selectionY])
                {
                    //CmdMoveChessman(selectedChessman.CurrentX, selectedChessman.CurrentY, selectionX, selectionY);
                    MoveChessman(selectionX, selectionY);
                }
            }
        }

    }

    private void MoveChessman(int x, int y)
    {
        moved = true;

        chessmans[selectedChessman.CurrentX, selectedChessman.CurrentY] = null;
        whiteKing.block = false;
        blackKing.block = false;

        if (chessmans[x, y] != null)
        {

            Destroy(chessmans[x, y].gameObject);
            chessmans[x, y] = null;
        }
        else if (selectedChessman.GetType() == typeof(pawn))
        {
            foreach (Chessman figure in chessmans)
            {
                if (figure != null)
                {
                    figure.elPassants = false;
                }
            }

            if (Mathf.Abs(selectedChessman.CurrentY - y) == 2)
            {
                selectedChessman.elPassants = true;
            }
            else if (x != selectedChessman.CurrentX)
            {
                if (selectedChessman.isWhite)
                {
                    Destroy(chessmans[x, y - 1].gameObject);
                    chessmans[x, y - 1] = null;
                }
                else
                {
                    Destroy(chessmans[x, y + 1].gameObject);
                    chessmans[x, y + 1] = null;
                }
            }
        }

        if (selectedChessman.GetType() == typeof(king))
        {
            if (Mathf.Abs(selectedChessman.CurrentX - x) > 1)
            {
                isCastling = true;
                CastlingX = x;
                CastlingY = y;
            }
        }
        else
        {
            isCastling = false;
        }

        start = selectedChessman.transform.position;
        destination = GetTileCenter(x, y);
        step = (destination - start);
        if (Math.Abs(step.x) < 0.01)
            step.x = 0;
        if (Math.Abs(step.z) < 0.01)
            step.z = 0;
        float speed = Math.Max(Math.Abs(step.x), Math.Abs(step.z)) * 6;
        step /= speed;
        float temp = step.z;
        step.z = 0;
        step.y = -temp;

        if (selectedChessman.GetType() == typeof(knight))
        {
            pathLength = (float)Math.Sqrt(Math.Abs(destination.x - start.x) *
                                          Math.Abs(destination.x - start.x) +
                                          Math.Abs(destination.z - start.z) *
                                          Math.Abs(destination.z - start.z));
        }

        selectedChessman.SetPosition(x, y);
        selectedChessman.touched = true;

        if (selectedChessman.GetType() == typeof(pawn) &&
            (selectedChessman.CurrentY == 7 || selectedChessman.CurrentY == 0))
        {
            ChangePawn(ref (selectedChessman));
        }
        else
        {
            chessmans[x, y] = selectedChessman;
        }

        doubleChecked = false;
        whiteKingIsChecked = false;
        blackKingIsChecked = false;
        whiteKing.HideChecked();
        blackKing.HideChecked();

        if (IsChecked(whiteKing))
        {
            whiteKingIsChecked = true;
            whiteKing.block = true;
            checkPath = CheckPath();
            whiteKing.ShowChecked();
        }
        else if (IsChecked(blackKing))
        {
            blackKingIsChecked = true;
            blackKing.block = true;
            checkPath = CheckPath();
            blackKing.ShowChecked();
        }

        Destroy(selection);
        FieldHighlighting.Instance.HideHighlights();
        isWhiteTurn = !isWhiteTurn;

    }

    private void ChangePawn(ref Chessman thePawn)
    {
        if (thePawn.isWhite)
        {
            SpawnChessman(1, thePawn.CurrentX, thePawn.CurrentY);
        }
        else
        {
            SpawnChessman(7, thePawn.CurrentX, thePawn.CurrentY);
        }
        Destroy(thePawn.gameObject);
    }

    private void Castling(int x, int y)
    {
        //white king's short castling
        if (x == 6 && y == 0)
        {
            Chessman selectedRook = chessmans[7, 0];
            chessmans[selectedRook.CurrentX, selectedRook.CurrentY] = null;

            selectedRook.transform.position = GetTileCenter(5, 0);
            selectedRook.SetPosition(5, 0);
            selectedRook.touched = true;
            chessmans[5, 0] = selectedRook;
        }
        //white king's long castling
        else if (x == 2 && y == 0)
        {
            Chessman selectedRook = chessmans[0, 0];
            chessmans[selectedRook.CurrentX, selectedRook.CurrentY] = null;

            selectedRook.transform.position = GetTileCenter(3, 0);
            selectedRook.SetPosition(3, 0);
            selectedRook.touched = true;
            chessmans[3, 0] = selectedRook;
        }
        //black king's short castling
        else if (x == 6 && y == 7)
        {
            Chessman selectedRook = chessmans[7, 7];
            chessmans[selectedRook.CurrentX, selectedRook.CurrentY] = null;

            selectedRook.transform.position = GetTileCenter(5, 7);
            selectedRook.SetPosition(5, 7);
            selectedRook.touched = true;
            chessmans[5, 7] = selectedRook;
        }
        //black king's long castling
        else if (x == 2 && y == 7)
        {
            Chessman selectedRook = chessmans[0, 7];
            chessmans[selectedRook.CurrentX, selectedRook.CurrentY] = null;

            selectedRook.transform.position = GetTileCenter(3, 7);
            selectedRook.SetPosition(3, 7);
            selectedRook.touched = true;
            chessmans[3, 7] = selectedRook;
        }
    }

    private void UpdateMousePosition()
    {
        if (!Camera.main)
            return;

        RaycastHit hit;
        if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit, 25.0f, LayerMask.GetMask("chessPlane")))
        {
            selectionX = (int)(18.735 * hit.point.x);
            selectionY = (int)(18.735 * hit.point.z);
        }
        else
        {
            selectionX = -1;
            selectionY = -1;
        }

        if (selectionX == 8 || selectionY == 8)
        {
            selectionX = -1;
            selectionY = -1;
        }

    }

    private void UpdateSelection()
    {
        if (Input.GetMouseButtonDown(0) && !moved)
        {
            if (selectionX >= 0 && selectionY >= 0 && chessmans[selectionX, selectionY] != null
                && chessmans[selectionX, selectionY].isWhite == isWhiteTurn)
            {
                selectedChessman = chessmans[selectionX, selectionY];
                Destroy(selection);
                selection = Instantiate(selectionArea, GetTileCenterLight(selectionX, selectionY), Quaternion.identity) as GameObject;
                selection.name = selectionArea.name;
                selection.transform.rotation = Quaternion.Euler(90, 0, 0);

                if (selectedChessman.isWhite)
                {
                    fieldsUnderAttackBlack = new bool[8, 8];
                    fieldsUnderAttackWhite = FieldsUnderAttack(true);
                }
                else
                {
                    fieldsUnderAttackWhite = new bool[8, 8];
                    fieldsUnderAttackBlack = FieldsUnderAttack(false);
                }

                selectedChessman.checking = true;
                bool[,] possibleMoves = selectedChessman.PossibleMove();
                selectedChessman.checking = false;

                FieldHighlighting.Instance.HideHighlights();
                FieldHighlighting.Instance.HighlightAllowedMoves(possibleMoves);
            }
            else if (!moved)
            {
                if (selection)
                {
                    Destroy(selection);
                    FieldHighlighting.Instance.HideHighlights();
                }
                selectedChessman = null;
            }
        }

        //moved = false;
    }

    private Vector3 GetTileCenter(int x, int y)
    {
        Vector3 origin = Vector3.zero;
        origin.x += (TILE_SIZE * x) + TILE_OFFSET;
        origin.y = heightEtalon;
        origin.z += (TILE_SIZE * y) + TILE_OFFSET;
        return origin;
    }

    public Vector3 GetTileCenterLight(int x, int y)
    {
        Vector3 origin = Vector3.zero;
        origin.x += (TILE_SIZE * x) + TILE_OFFSET;
        origin.y += heightEtalon + 0.11f;
        origin.z += (TILE_SIZE * y) + TILE_OFFSET;
        return origin;
    }

    private bool[,] FieldsUnderAttack(bool forWhite)
    {
        bool[,] r = new bool[8, 8];

        for (int i = 0; i < 8; ++i)
        {
            for (int j = 0; j < 8; ++j)
            {
                if (FieldIsAttacked(forWhite, i, j))
                {
                    r[i, j] = true;
                }
            }
        }

        return r;

    }

    public bool FieldIsAttacked(bool forWhite, int x, int y)
    {
        whiteKing.block = false;
        blackKing.block = false;
        foreach (Chessman figure in chessmans)
        {
            if (figure != null)
            {
                if (figure.isWhite != forWhite)
                {
                    if (figure.GetType() == typeof(pawn))
                    {
                        if (figure.isWhite)
                        {
                            if ((figure.CurrentX == x - 1 || figure.CurrentX == x + 1) &&
                                figure.CurrentY == y - 1)
                            {
                                whiteKing.block = true;
                                blackKing.block = true;
                                return true;
                            }
                        }
                        else
                        {
                            if ((figure.CurrentX == x - 1 || figure.CurrentX == x + 1) &&
                                figure.CurrentY == y + 1)
                            {
                                whiteKing.block = true;
                                blackKing.block = true;
                                return true;
                            }
                        }
                    }
                    else
                    {
                        figure.checking = true;

                        bool[,] temp = figure.PossibleMove();
                        figure.checking = false;
                        if (temp[x, y])
                        {
                            whiteKing.block = true;
                            blackKing.block = true;
                            return true;
                        }
                    }
                }
            }
        }

        whiteKing.block = true;
        blackKing.block = true;
        return false;
    }

    public bool IsChecked(Chessman king)
    {
        return FieldIsAttacked(king.isWhite, king.CurrentX, king.CurrentY);
    }

    private bool[,] CheckPath()
    {
        bool[,] checkedFields = new bool[8, 8];

        Chessman first = null, second = null;
        CheckedFigures(ref (first), ref (second));

        if (second != null)
        {
            doubleChecked = true;
        }
        else if (!isWhiteTurn)
        {
            //pawn
            if (first.GetType() == typeof(pawn))
            {
                checkedFields[first.CurrentX, first.CurrentY] = true;
            }
            //knight
            else if (first.GetType() == typeof(knight))
            {
                checkedFields[first.CurrentX, first.CurrentY] = true;
            }
            //bishop
            else if (first.GetType() == typeof(bishop))
            {
                //up-right
                if (first.CurrentX > whiteKing.CurrentX && first.CurrentY > whiteKing.CurrentY)
                {
                    for (int i = 1; i <= first.CurrentX - whiteKing.CurrentX; ++i)
                    {
                        checkedFields[whiteKing.CurrentX + i, whiteKing.CurrentY + i] = true;
                    }
                }
                //up-left
                else if (first.CurrentX < whiteKing.CurrentX && first.CurrentY > whiteKing.CurrentY)
                {
                    for (int i = 1; i <= whiteKing.CurrentX - first.CurrentX; ++i)
                    {
                        checkedFields[whiteKing.CurrentX - i, whiteKing.CurrentY + i] = true;
                    }
                }
                //down-right
                else if (first.CurrentX > whiteKing.CurrentX && first.CurrentY < whiteKing.CurrentY)
                {
                    for (int i = 1; i <= first.CurrentX - whiteKing.CurrentX; ++i)
                    {
                        checkedFields[whiteKing.CurrentX + i, whiteKing.CurrentY - i] = true;
                    }
                }
                //down-left
                else if (first.CurrentX < whiteKing.CurrentX && first.CurrentY < whiteKing.CurrentY)
                {
                    for (int i = 1; i <= whiteKing.CurrentX - first.CurrentX; ++i)
                    {
                        checkedFields[whiteKing.CurrentX - i, whiteKing.CurrentY - i] = true;
                    }
                }
            }
            //rook
            else if (first.GetType() == typeof(rook))
            {
                //vertical
                if (first.CurrentX == whiteKing.CurrentX)
                {
                    if (first.CurrentY > whiteKing.CurrentY)
                    {
                        //up
                        for (int i = 1; i <= first.CurrentY - whiteKing.CurrentY; ++i)
                        {
                            checkedFields[whiteKing.CurrentX, whiteKing.CurrentY + i] = true;
                        }
                    }
                    else
                    {
                        //down
                        for (int i = 1; i <= whiteKing.CurrentY - first.CurrentY; ++i)
                        {
                            checkedFields[whiteKing.CurrentX, whiteKing.CurrentY - i] = true;
                        }
                    }
                }

                //horizontal
                else if (first.CurrentY == whiteKing.CurrentY)
                {
                    if (first.CurrentX < whiteKing.CurrentX)
                    {
                        //left
                        for (int i = 1; i <= whiteKing.CurrentX - first.CurrentX; ++i)
                        {
                            checkedFields[whiteKing.CurrentX - i, whiteKing.CurrentY] = true;
                        }
                    }
                    else
                    {
                        //right
                        for (int i = 1; i <= first.CurrentX - whiteKing.CurrentX; ++i)
                        {
                            checkedFields[whiteKing.CurrentX + i, whiteKing.CurrentY] = true;
                        }
                    }
                }

            }
            //queen
            else if (first.GetType() == typeof(queen))
            {
                //up-right
                if (first.CurrentX > whiteKing.CurrentX && first.CurrentY > whiteKing.CurrentY)
                {
                    for (int i = 1; i <= first.CurrentX - whiteKing.CurrentX; ++i)
                    {
                        checkedFields[whiteKing.CurrentX + i, whiteKing.CurrentY + i] = true;
                    }
                }
                //up-left
                else if (first.CurrentX < whiteKing.CurrentX && first.CurrentY > whiteKing.CurrentY)
                {
                    for (int i = 1; i <= whiteKing.CurrentX - first.CurrentX; ++i)
                    {
                        checkedFields[whiteKing.CurrentX - i, whiteKing.CurrentY + i] = true;
                    }
                }
                //down-right
                else if (first.CurrentX > whiteKing.CurrentX && first.CurrentY < whiteKing.CurrentY)
                {
                    for (int i = 1; i <= first.CurrentX - whiteKing.CurrentX; ++i)
                    {
                        checkedFields[whiteKing.CurrentX + i, whiteKing.CurrentY - i] = true;
                    }
                }
                //down-left
                else if (first.CurrentX < whiteKing.CurrentX && first.CurrentY < whiteKing.CurrentY)
                {
                    for (int i = 1; i <= whiteKing.CurrentX - first.CurrentX; ++i)
                    {
                        checkedFields[whiteKing.CurrentX - i, whiteKing.CurrentY - i] = true;
                    }
                }

                //vertical
                else if (first.CurrentX == whiteKing.CurrentX)
                {
                    if (first.CurrentY > whiteKing.CurrentY)
                    {
                        //up
                        for (int i = 1; i <= first.CurrentY - whiteKing.CurrentY; ++i)
                        {
                            checkedFields[whiteKing.CurrentX, whiteKing.CurrentY + i] = true;
                        }
                    }
                    else
                    {
                        //down
                        for (int i = 1; i <= whiteKing.CurrentY - first.CurrentY; ++i)
                        {
                            checkedFields[whiteKing.CurrentX, whiteKing.CurrentY - i] = true;
                        }
                    }
                }

                //horizontal
                else if (first.CurrentY == whiteKing.CurrentY)
                {
                    if (first.CurrentX < whiteKing.CurrentX)
                    {
                        //left
                        for (int i = 1; i <= whiteKing.CurrentX - first.CurrentX; ++i)
                        {
                            checkedFields[whiteKing.CurrentX - i, whiteKing.CurrentY] = true;
                        }
                    }
                    else
                    {
                        //right
                        for (int i = 1; i <= first.CurrentX - whiteKing.CurrentX; ++i)
                        {
                            checkedFields[whiteKing.CurrentX + i, whiteKing.CurrentY] = true;
                        }
                    }
                }

            }
        }
        else
        {
            //pawn
            if (first.GetType() == typeof(pawn))
            {
                checkedFields[first.CurrentX, first.CurrentY] = true;
            }
            //knight
            else if (first.GetType() == typeof(knight))
            {
                checkedFields[first.CurrentX, first.CurrentY] = true;
            }
            //bishop
            else if (first.GetType() == typeof(bishop))
            {
                //up-right
                if (first.CurrentX > blackKing.CurrentX && first.CurrentY > blackKing.CurrentY)
                {
                    for (int i = 1; i <= first.CurrentX - blackKing.CurrentX; ++i)
                    {
                        checkedFields[blackKing.CurrentX + i, blackKing.CurrentY + i] = true;
                    }
                }
                //up-left
                else if (first.CurrentX < blackKing.CurrentX && first.CurrentY > blackKing.CurrentY)
                {
                    for (int i = 1; i <= blackKing.CurrentX - first.CurrentX; ++i)
                    {
                        checkedFields[blackKing.CurrentX - i, blackKing.CurrentY + i] = true;
                    }
                }
                //down-right
                else if (first.CurrentX > blackKing.CurrentX && first.CurrentY < blackKing.CurrentY)
                {
                    for (int i = 1; i <= first.CurrentX - blackKing.CurrentX; ++i)
                    {
                        checkedFields[blackKing.CurrentX + i, blackKing.CurrentY - i] = true;
                    }
                }
                //down-left
                else if (first.CurrentX < blackKing.CurrentX && first.CurrentY < blackKing.CurrentY)
                {
                    for (int i = 1; i <= blackKing.CurrentX - first.CurrentX; ++i)
                    {
                        checkedFields[blackKing.CurrentX - i, blackKing.CurrentY - i] = true;
                    }
                }
            }
            //rook
            else if (first.GetType() == typeof(rook))
            {
                //vertical
                if (first.CurrentX == blackKing.CurrentX)
                {
                    if (first.CurrentY > blackKing.CurrentY)
                    {
                        //up
                        for (int i = 1; i <= first.CurrentY - blackKing.CurrentY; ++i)
                        {
                            checkedFields[blackKing.CurrentX, blackKing.CurrentY + i] = true;
                        }
                    }
                    else
                    {
                        //down
                        for (int i = 1; i <= blackKing.CurrentY - first.CurrentY; ++i)
                        {
                            checkedFields[blackKing.CurrentX, blackKing.CurrentY - i] = true;
                        }
                    }
                }

                //horizontal
                if (first.CurrentY == blackKing.CurrentY)
                {
                    if (first.CurrentX < blackKing.CurrentX)
                    {
                        //left
                        for (int i = 1; i <= blackKing.CurrentX - first.CurrentX; ++i)
                        {
                            checkedFields[blackKing.CurrentX - i, blackKing.CurrentY] = true;
                        }
                    }
                    else
                    {
                        //right
                        for (int i = 1; i <= first.CurrentX - blackKing.CurrentX; ++i)
                        {
                            checkedFields[blackKing.CurrentX + i, blackKing.CurrentY] = true;
                        }
                    }
                }

            }
            //queen
            else if (first.GetType() == typeof(queen))
            {
                //up-right
                if (first.CurrentX > blackKing.CurrentX && first.CurrentY > blackKing.CurrentY)
                {
                    for (int i = 1; i <= first.CurrentX - blackKing.CurrentX; ++i)
                    {
                        checkedFields[blackKing.CurrentX + i, blackKing.CurrentY + i] = true;
                    }
                }
                //up-left
                else if (first.CurrentX < blackKing.CurrentX && first.CurrentY > blackKing.CurrentY)
                {
                    for (int i = 1; i <= blackKing.CurrentX - first.CurrentX; ++i)
                    {
                        checkedFields[blackKing.CurrentX - i, blackKing.CurrentY + i] = true;
                    }
                }
                //down-right
                else if (first.CurrentX > blackKing.CurrentX && first.CurrentY < blackKing.CurrentY)
                {
                    for (int i = 1; i <= first.CurrentX - blackKing.CurrentX; ++i)
                    {
                        checkedFields[blackKing.CurrentX + i, blackKing.CurrentY - i] = true;
                    }
                }
                //down-left
                else if (first.CurrentX < blackKing.CurrentX && first.CurrentY < blackKing.CurrentY)
                {
                    for (int i = 1; i <= blackKing.CurrentX - first.CurrentX; ++i)
                    {
                        checkedFields[blackKing.CurrentX - i, blackKing.CurrentY - i] = true;
                    }
                }

                //vertical
                else if (first.CurrentX == blackKing.CurrentX)
                {
                    if (first.CurrentY > blackKing.CurrentY)
                    {
                        //up
                        for (int i = 1; i <= first.CurrentY - blackKing.CurrentY; ++i)
                        {
                            checkedFields[blackKing.CurrentX, blackKing.CurrentY + i] = true;
                        }
                    }
                    else
                    {
                        //down
                        for (int i = 1; i <= blackKing.CurrentY - first.CurrentY; ++i)
                        {
                            checkedFields[blackKing.CurrentX, blackKing.CurrentY - i] = true;
                        }
                    }
                }

                //horizontal
                else if (first.CurrentY == blackKing.CurrentY)
                {
                    if (first.CurrentX < blackKing.CurrentX)
                    {
                        //left
                        for (int i = 1; i <= blackKing.CurrentX - first.CurrentX; ++i)
                        {
                            checkedFields[blackKing.CurrentX - i, blackKing.CurrentY] = true;
                        }
                    }
                    else
                    {
                        //right
                        for (int i = 1; i <= first.CurrentX - blackKing.CurrentX; ++i)
                        {
                            checkedFields[blackKing.CurrentX + i, blackKing.CurrentY] = true;
                        }
                    }
                }
            }
        }

        return checkedFields;
    }

    private void CheckedFigures(ref Chessman first, ref Chessman second)
    {
        if (!isWhiteTurn)
        {
            foreach (Chessman figure in chessmans)
            {
                if (figure != null && !figure.isWhite)
                {
                    bool[,] moves = new bool[8, 8];
                    if (figure.GetType() != typeof(king))
                    {
                        moves = figure.PossibleMove();
                    }

                    if (moves[whiteKing.CurrentX, whiteKing.CurrentY])
                    {
                        if (first == null)
                        {
                            first = figure;
                        }
                        else
                        {
                            second = figure;
                        }
                    }
                }
            }
        }
        else
        {
            foreach (Chessman figure in chessmans)
            {
                if (figure != null && figure.isWhite)
                {
                    bool[,] moves = new bool[8, 8];
                    if (figure.GetType() != typeof(king))
                    {
                        moves = figure.PossibleMove();
                    }

                    if (moves[blackKing.CurrentX, blackKing.CurrentY])
                    {
                        if (first == null)
                        {
                            first = figure;
                        }
                        else
                        {
                            second = figure;
                        }
                    }
                }
            }
        }
    }

    private bool AreAnyPossibleMoves(bool forWhite)
    {
        foreach (Chessman figure in chessmans)
        {
            if (figure != null)
            {
                if (figure.isWhite == forWhite)
                {
                    if (figure.GetType() == typeof(king))
                    {
                        if (figure.isWhite)
                        {
                            fieldsUnderAttackBlack = new bool[8, 8];
                            fieldsUnderAttackWhite = FieldsUnderAttack(true);
                        }
                        else
                        {
                            fieldsUnderAttackBlack = FieldsUnderAttack(false);
                            fieldsUnderAttackWhite = new bool[8, 8];
                        }
                    }

                    bool[,] moves = figure.PossibleMove();
                    if (SearchForTrue(moves))
                    {


                        return true;
                    }
                }
            }
        }

        return false;
    }

    private bool SearchForTrue(bool[,] moves)
    {
        for (int i = 0; i < 8; ++i)
        {
            for (int j = 0; j < 8; ++j)
            {
                if (moves[i, j] == true)
                {
                    return true;
                }
            }
        }
        return false;
    }

    private void EndGame(bool win)
    {
        if (win)
        {
            if (!isWhiteTurn)
                Debug.Log("White team wins");
            else
                Debug.Log("Black team wins");
        }
        else
        {
            Debug.Log("Draw");
        }

        whiteKing.HideChecked();
        blackKing.HideChecked();

        foreach (GameObject go in activeFigure)
            Destroy(go);

        whiteKingIsChecked = false;
        blackKingIsChecked = false;
        doubleChecked = false;
        isWhiteTurn = true;
        moved = false;
        FieldHighlighting.Instance.HideHighlights();
        SpawnAllChessmen();
    }

    [Command]
    public void CmdMoveChessman(int currentX, int currentY, int x, int y)
    {
        RpcMoveChessman(currentX, currentY, x, y);
        //RpcMoveChessman();
        //MoveHelp(currentX, currentY);
        //MoveChessman(x, y);
    }

    [ClientRpc]
    public void RpcMoveChessman(int currentX, int currentY, int x, int y)
    {
        isMyTurn = !isMyTurn;
    }

    private void MoveHelp(int currentX, int currentY)
    {
        selectedChessman = chessmans[currentX, currentY];
        moved = true;
    }
}
