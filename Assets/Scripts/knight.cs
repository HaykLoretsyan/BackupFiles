using UnityEngine;
using System.Collections;

public class knight : Chessman {

    public override bool[,] PossibleMove()
    {
        bool[,] r = new bool[8, 8];
        Chessman[,] temp = main.chessmans;

        if (isWhite)//white
        {
            //up-(right,left)
            if (CurrentY < 6)
            {
                if (CurrentX < 7)
                {
                    if(temp[CurrentX + 1,CurrentY + 2] == null ||
                       !temp[CurrentX + 1, CurrentY + 2].isWhite)
                    {
                        r[CurrentX + 1, CurrentY + 2] = true;
                    }
                }
                if (CurrentX > 0)
                {
                    if (temp[CurrentX - 1, CurrentY + 2] == null ||
                       !temp[CurrentX - 1, CurrentY + 2].isWhite)
                    {
                        r[CurrentX - 1, CurrentY + 2] = true;
                    }
                }
            }

            //down-(right,left)
            if (CurrentY > 1)
            {
                if (CurrentX < 7)
                {
                    if (temp[CurrentX + 1, CurrentY - 2] == null ||
                       !temp[CurrentX + 1, CurrentY - 2].isWhite)
                    {
                        r[CurrentX + 1, CurrentY - 2] = true;
                    }
                }
                if (CurrentX > 0)
                {
                    if (temp[CurrentX - 1, CurrentY - 2] == null ||
                       !temp[CurrentX - 1, CurrentY - 2].isWhite)
                    {
                        r[CurrentX - 1, CurrentY - 2] = true;
                    }
                }
            }

            //left-(up,down)
            if (CurrentX > 1)
            {
                if (CurrentY < 7)
                {
                    if (temp[CurrentX - 2, CurrentY + 1] == null ||
                       !temp[CurrentX - 2, CurrentY + 1].isWhite)
                    {
                        r[CurrentX - 2, CurrentY + 1] = true;
                    }
                }
                if (CurrentY > 0)
                {
                    if (temp[CurrentX - 2, CurrentY - 1] == null ||
                       !temp[CurrentX - 2, CurrentY - 1].isWhite)
                    {
                        r[CurrentX - 2, CurrentY - 1] = true;
                    }
                }
            }

            //right-(up,down)
            if (CurrentX < 6)
            {
                if (CurrentY < 7)
                {
                    if (temp[CurrentX + 2, CurrentY + 1] == null ||
                       !temp[CurrentX + 2, CurrentY + 1].isWhite)
                    {
                        r[CurrentX + 2, CurrentY + 1] = true;
                    }
                }
                if (CurrentY > 0)
                {
                    if (temp[CurrentX + 2, CurrentY - 1] == null ||
                       !temp[CurrentX + 2, CurrentY - 1].isWhite)
                    {
                        r[CurrentX + 2, CurrentY - 1] = true;
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
        else//black
        {
            //up-(right,left)
            if (CurrentY < 6)
            {
                if (CurrentX < 7)
                {
                    if (temp[CurrentX + 1, CurrentY + 2] == null ||
                       temp[CurrentX + 1, CurrentY + 2].isWhite)
                    {
                        r[CurrentX + 1, CurrentY + 2] = true;
                    }
                }
                if (CurrentX > 0)
                {
                    if (temp[CurrentX - 1, CurrentY + 2] == null ||
                       temp[CurrentX - 1, CurrentY + 2].isWhite)
                    {
                        r[CurrentX - 1, CurrentY + 2] = true;
                    }
                }
            }

            //down-(right,left)
            if (CurrentY > 1)
            {
                if (CurrentX < 7)
                {
                    if (temp[CurrentX + 1, CurrentY - 2] == null ||
                       temp[CurrentX + 1, CurrentY - 2].isWhite)
                    {
                        r[CurrentX + 1, CurrentY - 2] = true;
                    }
                }
                if (CurrentX > 0)
                {
                    if (temp[CurrentX - 1, CurrentY - 2] == null ||
                       temp[CurrentX - 1, CurrentY - 2].isWhite)
                    {
                        r[CurrentX - 1, CurrentY - 2] = true;
                    }
                }
            }

            //left-(up,down)
            if (CurrentX > 1)
            {
                if (CurrentY < 7)
                {
                    if (temp[CurrentX - 2, CurrentY + 1] == null ||
                       temp[CurrentX - 2, CurrentY + 1].isWhite)
                    {
                        r[CurrentX - 2, CurrentY + 1] = true;
                    }
                }
                if (CurrentY > 0)
                {
                    if (temp[CurrentX - 2, CurrentY - 1] == null ||
                       temp[CurrentX - 2, CurrentY - 1].isWhite)
                    {
                        r[CurrentX - 2, CurrentY - 1] = true;
                    }
                }
            }

            //right-(up,down)
            if (CurrentX < 6)
            {
                if (CurrentY < 7)
                {
                    if (temp[CurrentX + 2, CurrentY + 1] == null ||
                       temp[CurrentX + 2, CurrentY + 1].isWhite)
                    {
                        r[CurrentX + 2, CurrentY + 1] = true;
                    }
                }
                if (CurrentY > 0)
                {
                    if (temp[CurrentX + 2, CurrentY - 1] == null ||
                       temp[CurrentX + 2, CurrentY - 1].isWhite)
                    {
                        r[CurrentX + 2, CurrentY - 1] = true;
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
