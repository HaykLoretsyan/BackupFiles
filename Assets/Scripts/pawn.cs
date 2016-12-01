using UnityEngine;
using System.Collections;

public class pawn : Chessman {

    public override bool[,] PossibleMove()
    {
        bool[,] r = new bool[8, 8];
        Chessman[,] temp = main.Instance.chessmans;

        //white
        if (isWhite)
        {
            //1-step
            if (CurrentY < 7 && temp[CurrentX, CurrentY + 1] == null)
            {
                r[CurrentX, CurrentY + 1] = true;
                //2-step
                if (!touched && temp[CurrentX, CurrentY + 2] == null)
                {
                    r[CurrentX, CurrentY + 2] = true;
                }
            }

            //left attack
            if (CurrentX != 0 && CurrentY != 7 &&
                temp[CurrentX - 1, CurrentY + 1] != null &&
                !temp[CurrentX - 1, CurrentY + 1].isWhite)
            {
                r[CurrentX - 1, CurrentY + 1] = true;
            }

            //right attack
            if (CurrentX != 7 && CurrentY != 7 &&
                temp[CurrentX + 1, CurrentY + 1] != null &&
                !temp[CurrentX + 1, CurrentY + 1].isWhite)
            {
                r[CurrentX + 1, CurrentY + 1] = true;
            }

            //right cross attack
            if (CurrentX < 7 &&
                temp[CurrentX + 1, CurrentY] != null &&
                !temp[CurrentX + 1, CurrentY].isWhite &&
                temp[CurrentX + 1, CurrentY].GetType() == typeof(pawn) &&
                temp[CurrentX + 1, CurrentY].elPassants)
            {
                r[CurrentX + 1, CurrentY + 1] = true;
            }

            //left cross attack
            if (CurrentX > 0 &&
                temp[CurrentX - 1, CurrentY] != null &&
                !temp[CurrentX - 1, CurrentY].isWhite &&
                temp[CurrentX - 1, CurrentY].GetType() == typeof(pawn) &&
                temp[CurrentX - 1, CurrentY].elPassants)
            {
                r[CurrentX - 1, CurrentY + 1] = true;
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
        //black
        else
        {
            //1-step
            if (CurrentY > 0 && main.Instance.chessmans[CurrentX, CurrentY - 1] == null)
            {
                r[CurrentX, CurrentY - 1] = true;
                //2-step
                if (!touched && main.Instance.chessmans[CurrentX, CurrentY - 2] == null)
                {
                    r[CurrentX, CurrentY - 2] = true;
                }
            }

            //left attack
            if (CurrentX != 7 && CurrentY != 0 &&
                temp[CurrentX + 1, CurrentY - 1] != null &&
                temp[CurrentX + 1, CurrentY - 1].isWhite)
            {
                r[CurrentX + 1, CurrentY - 1] = true;
            }

            //right attack
            if (CurrentX != 0 && CurrentY != 0 &&
                temp[CurrentX - 1, CurrentY - 1] != null &&
                temp[CurrentX - 1, CurrentY - 1].isWhite)
            {
                r[CurrentX - 1, CurrentY - 1] = true;
            }

            //right cross attack
            if (CurrentX < 7 && 
                temp[CurrentX + 1, CurrentY] != null &&
                temp[CurrentX + 1, CurrentY].isWhite &&
                temp[CurrentX + 1, CurrentY].GetType() == typeof(pawn) &&
                temp[CurrentX + 1, CurrentY].elPassants)
            {
                r[CurrentX + 1, CurrentY - 1] = true;
            }

            //left cross attack
            if (CurrentX > 0 &&
                temp[CurrentX - 1, CurrentY] != null &&
                temp[CurrentX - 1, CurrentY].isWhite &&
                temp[CurrentX - 1, CurrentY].GetType() == typeof(pawn) &&
                temp[CurrentX - 1, CurrentY].elPassants)
            {
                r[CurrentX - 1, CurrentY - 1] = true;
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
