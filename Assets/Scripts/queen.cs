using UnityEngine;
using System.Collections;

public class queen : Chessman {

    public override bool[,] PossibleMove()
    {
        bool[,] r = new bool[8, 8];

        Chessman[,] temp = main.chessmans;

        if (isWhite) //white
        {
            {
                int i, j;

                //up-left
                i = CurrentX - 1;
                j = CurrentY + 1;

                while (i > -1 && j < 8)
                {
                    if (temp[i, j] == null)
                    {
                        r[i, j] = true;
                    }
                    else
                    {
                        if (temp[i, j].isWhite)
                        {
                            break;
                        }
                        else
                        {
                            r[i, j] = true;
                            break;
                        }
                    }
                    --i;
                    ++j;
                }

                //up-right
                i = CurrentX + 1;
                j = CurrentY + 1;

                while (i < 8 && j < 8)
                {
                    if (temp[i, j] == null)
                    {
                        r[i, j] = true;
                    }
                    else
                    {
                        if (temp[i, j].isWhite)
                        {
                            break;
                        }
                        else
                        {
                            r[i, j] = true;
                            break;
                        }
                    }
                    ++i;
                    ++j;
                }

                //down-left
                i = CurrentX - 1;
                j = CurrentY - 1;

                while (i > -1 && j > -1)
                {
                    if (temp[i, j] == null)
                    {
                        r[i, j] = true;
                    }
                    else
                    {
                        if (temp[i, j].isWhite)
                        {
                            break;
                        }
                        else
                        {
                            r[i, j] = true;
                            break;
                        }
                    }
                    --i;
                    --j;
                }

                //down-right
                i = CurrentX + 1;
                j = CurrentY - 1;

                while (i < 8 && j > -1)
                {
                    if (temp[i, j] == null)
                    {
                        r[i, j] = true;
                    }
                    else
                    {
                        if (temp[i, j].isWhite)
                        {
                            break;
                        }
                        else
                        {
                            r[i, j] = true;
                            break;
                        }
                    }
                    ++i;
                    --j;
                }
            }

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
            {
                int i, j;

                //up-left
                i = CurrentX - 1;
                j = CurrentY + 1;

                while (i > -1 && j < 8)
                {
                    if (temp[i, j] == null)
                    {
                        r[i, j] = true;
                    }
                    else
                    {
                        if (!temp[i, j].isWhite)
                        {
                            break;
                        }
                        else
                        {
                            r[i, j] = true;
                            break;
                        }
                    }
                    --i;
                    ++j;
                }

                //up-right
                i = CurrentX + 1;
                j = CurrentY + 1;

                while (i < 8 && j < 8)
                {
                    if (temp[i, j] == null)
                    {
                        r[i, j] = true;
                    }
                    else
                    {
                        if (!temp[i, j].isWhite)
                        {
                            break;
                        }
                        else
                        {
                            r[i, j] = true;
                            break;
                        }
                    }
                    ++i;
                    ++j;
                }

                //down-left
                i = CurrentX - 1;
                j = CurrentY - 1;

                while (i > -1 && j > -1)
                {
                    if (temp[i, j] == null)
                    {
                        r[i, j] = true;
                    }
                    else
                    {
                        if (!temp[i, j].isWhite)
                        {
                            break;
                        }
                        else
                        {
                            r[i, j] = true;
                            break;
                        }
                    }
                    --i;
                    --j;
                }

                //down-right
                i = CurrentX + 1;
                j = CurrentY - 1;

                while (i < 8 && j > -1)
                {
                    if (temp[i, j] == null)
                    {
                        r[i, j] = true;
                    }
                    else
                    {
                        if (!temp[i, j].isWhite)
                        {
                            break;
                        }
                        else
                        {
                            r[i, j] = true;
                            break;
                        }
                    }
                    ++i;
                    --j;
                }
            }
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
