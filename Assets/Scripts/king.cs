using UnityEngine;
using System.Collections;

public class king : Chessman {

    public GameObject checkObject;
    private GameObject check;

    override public void ShowChecked()
    {
        Vector3 pos = main.Instance.GetTileCenterLight(CurrentX, CurrentY);
        if (check == null)
        {            
            check = Instantiate(checkObject, pos, Quaternion.Euler(90,0,0)) as GameObject;
        }
        else
        {
            check.transform.position = pos;
            check.SetActive(true);
        }
    }

    override public void HideChecked()
    {
        if (check != null)
        {
            check.SetActive(false);
        }
    }

    public override bool[,] PossibleMove()
    {
        bool[,] r = new bool[8,8];
        Chessman[,] temp = main.Instance.chessmans;


        if (isWhite) //white
        {
            //up
            if(CurrentY < 7 && (temp[CurrentX,CurrentY + 1] == null || 
                !temp[CurrentX, CurrentY + 1].isWhite) && Takeable(CurrentX, CurrentY + 1))
            {
                r[CurrentX, CurrentY + 1] = true;
            }
            //down
            if (CurrentY > 0 && (temp[CurrentX, CurrentY - 1] == null || 
                !temp[CurrentX, CurrentY - 1].isWhite) && Takeable(CurrentX, CurrentY - 1))
            {
                r[CurrentX, CurrentY - 1] = true;
            }
            //left
            if (CurrentX > 0 && (temp[CurrentX - 1, CurrentY] == null || 
                !temp[CurrentX - 1, CurrentY].isWhite) && Takeable(CurrentX - 1, CurrentY))
            {
                r[CurrentX - 1, CurrentY] = true;
            }
            //right
            if (CurrentX < 7 && (temp[CurrentX + 1, CurrentY] == null ||
                !temp[CurrentX + 1, CurrentY].isWhite) && Takeable(CurrentX + 1, CurrentY))
            {
                r[CurrentX + 1, CurrentY] = true;
            }
            //up-left
            if (CurrentX > 0 && CurrentY < 7 && (temp[CurrentX - 1, CurrentY + 1] == null ||
                !temp[CurrentX - 1, CurrentY + 1].isWhite) && Takeable(CurrentX - 1, CurrentY + 1))
            {
                r[CurrentX - 1, CurrentY + 1] = true;
            }
            //up-right
            if (CurrentX < 7 && CurrentY < 7 && (temp[CurrentX + 1, CurrentY + 1] == null ||
                !temp[CurrentX + 1, CurrentY + 1].isWhite) && Takeable(CurrentX + 1, CurrentY + 1))
            {
                r[CurrentX + 1, CurrentY + 1] = true;
            }
            //down-left
            if (CurrentX > 0 && CurrentY > 0 && (temp[CurrentX - 1, CurrentY - 1] == null ||
                !temp[CurrentX - 1, CurrentY - 1].isWhite) && Takeable(CurrentX - 1, CurrentY - 1))
            {
                r[CurrentX - 1, CurrentY - 1] = true;
            }
            //down-right
            if (CurrentX < 7 && CurrentY > 0 && (temp[CurrentX + 1, CurrentY - 1] == null ||
                !temp[CurrentX + 1, CurrentY - 1].isWhite) && Takeable(CurrentX + 1, CurrentY - 1))
            {
                r[CurrentX + 1, CurrentY - 1] = true;
            }

            bool s_castle = false, l_castle = false;
            CheckForCastling(ref(s_castle), ref(l_castle));
            if(s_castle)
            {
                r[6, 0] = true;
            }
            
            if(l_castle)
            {
                r[2, 0] = true;
            }

        }
        else //black
        {
            //up
            if (CurrentY < 7 && (temp[CurrentX, CurrentY + 1] == null ||
                temp[CurrentX, CurrentY + 1].isWhite) && Takeable(CurrentX, CurrentY + 1))
            {
                r[CurrentX, CurrentY + 1] = true;
            }
            //down
            if (CurrentY > 0 && (temp[CurrentX, CurrentY - 1] == null ||
                temp[CurrentX, CurrentY - 1].isWhite) && Takeable(CurrentX, CurrentY - 1))
            {
                r[CurrentX, CurrentY - 1] = true;
            }
            //left
            if (CurrentX > 0 && (temp[CurrentX - 1, CurrentY] == null ||
                temp[CurrentX - 1, CurrentY].isWhite) && Takeable(CurrentX - 1, CurrentY))
            {
                r[CurrentX - 1, CurrentY] = true;
            }
            //right
            if (CurrentX < 7 && (temp[CurrentX + 1, CurrentY] == null ||
                temp[CurrentX + 1, CurrentY].isWhite) && Takeable(CurrentX + 1, CurrentY))
            {
                r[CurrentX + 1, CurrentY] = true;
            }
            //up-left
            if (CurrentX > 0 && CurrentY < 7 && (temp[CurrentX - 1, CurrentY + 1] == null ||
                temp[CurrentX - 1, CurrentY + 1].isWhite) && Takeable(CurrentX - 1, CurrentY + 1))
            {
                r[CurrentX - 1, CurrentY + 1] = true;
            }
            //up-right
            if (CurrentX < 7 && CurrentY < 7 && (temp[CurrentX + 1, CurrentY + 1] == null ||
                temp[CurrentX + 1, CurrentY + 1].isWhite) && Takeable(CurrentX + 1, CurrentY + 1))
            {
                r[CurrentX + 1, CurrentY + 1] = true;
            }
            //down-left
            if (CurrentX > 0 && CurrentY > 0 && (temp[CurrentX - 1, CurrentY - 1] == null ||
                temp[CurrentX - 1, CurrentY - 1].isWhite) && Takeable(CurrentX - 1, CurrentY - 1))
            {
                r[CurrentX - 1, CurrentY - 1] = true;
            }
            //down-right
            if (CurrentX < 7 && CurrentY > 0 && (temp[CurrentX + 1, CurrentY - 1] == null ||
                temp[CurrentX + 1, CurrentY - 1].isWhite) && Takeable(CurrentX + 1, CurrentY - 1))
            {
                r[CurrentX + 1, CurrentY - 1] = true;
            }

            bool s_castle = false, l_castle = false;
            CheckForCastling(ref (s_castle), ref (l_castle));
            if (s_castle)
            {
                r[6, 7] = true;
            }

            if (l_castle)
            {
                r[2, 7] = true;
            }
        }

        bool[,] temp2;
        if (isWhite)
        {
            temp2 = main.Instance.fieldsUnderAttackWhite;
        }
        else
        {
            temp2 = main.Instance.fieldsUnderAttackBlack;
        }

        for(int i = 0; i < 8; ++i)
        {
            for (int j = 0; j < 8; ++j)
            {
                if(r[i,j] == temp2[i,j])
                {
                    r[i, j] = false;
                }
            }
        }

        return r;
    }

    private void CheckForCastling(ref bool short_castling, ref bool long_castling)
    {

        if (touched)
        {
            short_castling = false;
            long_castling = false;
            return;
        }

        Chessman[,] temp = main.Instance.chessmans;

        if (isWhite) //white
        {

            //short castling 
            if (temp[5, 0] == null && temp[6, 0] == null && 
                temp[7,0] != null && !temp[7,0].touched && 
                !main.whiteKingIsChecked &&
                !main.Instance.fieldsUnderAttackWhite[5, 0] &&
                !main.Instance.fieldsUnderAttackWhite[6, 0])
            {
                short_castling = true;
            }
            else
            {
                short_castling = false;
            }

            //long castling
            if (temp[1,0] == null && temp[2,0] == null && temp[3, 0] == null && 
                temp[0,0] != null && !temp[0, 0].touched &&
                !main.whiteKingIsChecked &&
                !main.Instance.fieldsUnderAttackWhite[2, 0] &&
                !main.Instance.fieldsUnderAttackWhite[3, 0] &&
                !main.Instance.fieldsUnderAttackWhite[3, 0])
            {
                long_castling = true;
            }
            else
            {
                long_castling = false;
            }
        }
        else //black
        {
            //short castling 
            if (temp[5, 7] == null && temp[6, 7] == null &&
                temp[7, 7] != null && !temp[7, 7].touched &&
                !main.blackKingIsChecked &&
                !main.Instance.fieldsUnderAttackBlack[5, 7] &&
                !main.Instance.fieldsUnderAttackBlack[6, 7])
            {
                short_castling = true;
            }
            else
            {
                short_castling = false;
            }

            //long castling
            if (temp[1, 7] == null && temp[2, 7] == null && temp[3, 7] == null &&
                temp[0, 7] != null && !temp[0, 7].touched &&
                !main.blackKingIsChecked &&
                !main.Instance.fieldsUnderAttackBlack[2, 7] &&
                !main.Instance.fieldsUnderAttackBlack[3, 7] &&
                !main.Instance.fieldsUnderAttackBlack[4, 7] )
            {
                long_castling = true;
            }
            else
            {
                long_castling = false;
            }
        }
    }

    private bool Takeable(int x, int y)
    {
        if (block)
        {
            Chessman figure = main.Instance.chessmans[x, y];
            main.Instance.chessmans[CurrentX, CurrentY] = null;
            int prevX = CurrentX, prevY = CurrentY;
            SetPosition(x, y);
            main.Instance.chessmans[x, y] = this;

            bool ret = !main.Instance.IsChecked(this);
            main.Instance.chessmans[x, y] = figure;
            main.Instance.chessmans[prevX, prevY] = this;
            SetPosition(prevX, prevY);

            return ret;
        }
        return true;
    }
}
