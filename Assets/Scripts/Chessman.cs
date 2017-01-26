using UnityEngine;
using System.Collections;

public abstract class Chessman : MonoBehaviour {

    public int CurrentX { set; get; }
    public int CurrentY { set; get; }
    public bool isWhite;
    public bool touched = false;
    public bool elPassants = false;
    public bool checking = false;
    public bool block = false;

    public void SetPosition(int x, int y)
    {
        CurrentX = x;
        CurrentY = y;
    }

    public virtual bool[,] PossibleMove()
    {
        return new bool[8, 8];
    }

    public virtual void ShowChecked()
    {
        Debug.Log("Not a King");
    }

    public virtual void HideChecked()
    {
        Debug.Log("Not a King");
    }

    protected void OnKingsWay(ref bool[,] r)
    {      
        Chessman[,] temp = main.chessmans;
        Chessman figure = temp[CurrentX, CurrentY];
        int x = CurrentX, y = CurrentY;
        temp[CurrentX, CurrentY] = null;

        

        for (int i = 0; i < 8; ++i)
        {
            for(int j = 0; j < 8; ++j)
            {
                if(r[i,j] == true)
                {
                    figure.SetPosition(i, j);
                    Chessman temp_figure = temp[i, j];
                    temp[i, j] = figure;
                    if (KingIsUnderAttack(temp))
                    {
                        r[i, j] = false;
                    }
                    temp[i, j] = temp_figure;
                }
            }
        }

        figure.SetPosition(x, y);
        temp[x,y] = figure;
    }

    private bool KingIsUnderAttack(Chessman[,] chessmans)
    {
        int x, y;
        if(isWhite)
        {
            x = main.whiteKing.CurrentX;
            y = main.whiteKing.CurrentY;
        }
        else
        {
            x = main.blackKing.CurrentX;
            y = main.blackKing.CurrentY;
        }

        foreach (Chessman figure in chessmans)
        {
            if (figure != null)
            {
                if (figure.isWhite != isWhite)
                {
                    if (figure.GetType() == typeof(pawn))
                    {
                        if (figure.isWhite)
                        {
                            if ((figure.CurrentX == x - 1 || figure.CurrentX == x + 1) &&
                                figure.CurrentY == y - 1)
                            {
                                return true;
                            }
                        }
                        else
                        {
                            if ((figure.CurrentX == x - 1 || figure.CurrentX == x + 1) &&
                                figure.CurrentY == y + 1)
                            {
                                return true;
                            }
                        }
                    }
                    else
                    {
                        bool[,] temp = figure.PossibleMove();
                        if (temp[x, y])
                        {
                            return true;
                        }
                    }
                }
            }
        }

        return false;
    }
}
