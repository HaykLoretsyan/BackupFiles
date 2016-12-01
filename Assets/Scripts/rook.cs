using UnityEngine;
using System.Collections;

public class rook : Chessman {

    public override bool[,] PossibleMove()
    {
        bool[,] r = new bool[8, 8];
        Chessman[,] temp = main.Instance.chessmans;

        if (isWhite) //white
        {
            //up
            for (int i = CurrentY + 1; i < 8; ++i)
            {
                if (temp[CurrentX, i] == null)
                {
                    r[CurrentX, i] = true;
                }
                else
                {
                    if (temp[CurrentX, i].isWhite)
                    {
                        break;
                    }
                    else
                    {
                        r[CurrentX, i] = true;
                        break;
                    }
                }

            }

            //down
            for (int i = CurrentY - 1; i > -1; --i)
            {
                if (temp[CurrentX, i] == null)
                {
                    r[CurrentX, i] = true;
                }
                else
                {
                    if (temp[CurrentX, i].isWhite)
                    {
                        break;
                    }
                    else
                    {
                        r[CurrentX, i] = true;
                        break;
                    }
                }

            }

            //right
            for (int i = CurrentX + 1; i < 8; ++i)
            {
                if (temp[i, CurrentY] == null)
                {
                    r[i, CurrentY] = true;
                }
                else
                {
                    if (temp[i, CurrentY].isWhite)
                    {
                        break;
                    }
                    else
                    {
                        r[i, CurrentY] = true;
                        break;
                    }
                }

            }

            //left
            for (int i = CurrentX - 1; i > -1; --i)
            {
                if (temp[i, CurrentY] == null)
                {
                    r[i, CurrentY] = true;
                }
                else
                {
                    if (temp[i, CurrentY].isWhite)
                    {
                        break;
                    }
                    else
                    {
                        r[i, CurrentY] = true;
                        break;
                    }
                }

            }

            //when king is under attack
            if (main.whiteKingIsChecked)
            {
                for (int i = 0; i < 8; ++i)
                {
                    for (int j = 0; j < 8; ++j)
                    {
                        if (main.Instance.checkPath[i, j] != r[i, j])
                        {
                            r[i, j] = false;
                        }
                    }
                }
            }

            if (checking)
            {
                OnKingsWay(ref (r));
            }

        }
        else  //black
        {
            //up
            for (int i = CurrentY + 1; i < 8; ++i)
            {
                if (temp[CurrentX, i] == null)
                {
                    r[CurrentX, i] = true;
                }
                else
                {
                    if (!temp[CurrentX, i].isWhite)
                    {
                        break;
                    }
                    else
                    {
                        r[CurrentX, i] = true;
                        break;
                    }
                }

            }

            //down
            for (int i = CurrentY - 1; i > -1; --i)
            {
                if (temp[CurrentX, i] == null)
                {
                    r[CurrentX, i] = true;
                }
                else
                {
                    if (!temp[CurrentX, i].isWhite)
                    {
                        break;
                    }
                    else
                    {
                        r[CurrentX, i] = true;
                        break;
                    }
                }

            }

            //right
            for (int i = CurrentX + 1; i < 8; ++i)
            {
                if (temp[i, CurrentY] == null)
                {
                    r[i, CurrentY] = true;
                }
                else
                {
                    if (!temp[i, CurrentY].isWhite)
                    {
                        break;
                    }
                    else
                    {
                        r[i, CurrentY] = true;
                        break;
                    }
                }

            }
            //left
            for (int i = CurrentX - 1; i > -1; --i)
            {
                if (temp[i, CurrentY] == null)
                {
                    r[i, CurrentY] = true;
                }
                else
                {
                    if (!temp[i, CurrentY].isWhite)
                    {
                        break;
                    }
                    else
                    {
                        r[i, CurrentY] = true;
                        break;
                    }
                }

            }

            //when king is under attack
            if (main.blackKingIsChecked)
            {
                for (int i = 0; i < 8; ++i)
                {
                    for (int j = 0; j < 8; ++j)
                    {
                        if (main.Instance.checkPath[i, j] != r[i, j])
                        {
                            r[i, j] = false;
                        }
                    }
                }
            }

            if (checking)
            {
                OnKingsWay(ref (r));
            }

        }
        return r;
    }
}
