using UnityEngine;
using System.Collections;

public class bishop : Chessman
{
    public override bool[,] PossibleMove ()
    {
        bool[,] r = new bool[8, 8];
        Chessman[,] temp = main.Instance.chessmans;

        if (isWhite)//white
        {
            int i, j;

            //up-left
            i = CurrentX - 1;
            j = CurrentY + 1;

            while (i > -1 && j < 8)
            {
                if(temp[i, j] == null)
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

            //when king is under attack
            if (main.whiteKingIsChecked)
            {
                for(i = 0; i < 8; ++i)
                {
                    for (j = 0; j < 8; ++j)
                    {
                        if(main.Instance.checkPath[i,j] != r[i,j])
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
        else//black
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

            while (i < 7 && j < 8)
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

            //when king is under attack
            if (main.blackKingIsChecked)
            {
                for (i = 0; i < 8; ++i)
                {
                    for (j = 0; j < 8; ++j)
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
