using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GameJing_android
{
    class GameJing
    {
        public int[,] okSet ;
        public int[] numUsed ;
        public int[,] GameGrid ;
        public int Target = 15;
        public bool aiWon = false;
        public bool aiFirst;
        public bool gameOver;
        public GameJing(int he)
        {
            Target = he;
            okSet = new int[3, 8];
            numUsed = new int[10];
            GameGrid = new int[9, 2];
            aiWon = false;
            aiFirst = false;
            gameOver = false;
            getOkSet();
            getNumUsed();
            for (int i = 0; i < 9; i++)
            {
                GameGrid[i, 0] = 0;
                GameGrid[i, 1] = 0;
            }
        }

        public void setGrid(int index, int val, int owner)
        {
            GameGrid[index, 0] = val;
            GameGrid[index, 1] = owner;
        }
        public void setGrid(int row,int col, int val, int owner)
        {
            int index = row * 3 + col;
            GameGrid[index, 0] = val;
            GameGrid[index, 1] = owner;
        }
        public int getGridValue(int index)
        {
            return GameGrid[index, 0];
        }
        public int GetGridOwner(int index)
        {
            return GameGrid[index, 1];
        }
        public bool IsUsed(int num)
        {
            if (num <= 0 || num > 9)
                return true;

            for (int i = 0; i < 9; i++)
            {
                if (GameGrid[i, 0] == num)
                {
                    return true;
                }
            }
            return false;
        }
        private int checkDup(int index, int a, int b, int c)
        {
            for (int i = 0; i < index; i++)
            {
                if (okSet[0,i] == a || okSet[1,i] == a || okSet[2,i] == a)
                {
                    if (okSet[0,i] == b || okSet[1,i] == b || okSet[2,i] == b)
                    {
                        if (okSet[0,i] == c || okSet[1,i] == c || okSet[2,i] == c)
                        {
                            return 1;
                        }
                    }
                }
            }
            return 0;
        }
        public void getNumUsed()
        {
            for (int i = 0; i < 10; i++)
                numUsed[i] = 0;

            for (int i = 0; i < 8; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    int n = okSet[j,i];
                    numUsed[n]++;
                }
            }
        }
        public void getOkSet()
        {
            int count = 0;
            for (int a = 1; a < 10; a++)
            {
                for (int b = 1; b < 10; b++)
                {
                    okSet[0,count] = a;
                    if (!(a == b))
                    {
                        okSet[1,count] = b;
                        int c = Target - okSet[0,count] - okSet[1,count];
                        if ((c > 0) && (c < 10) && (c != okSet[0,count]) && (c != okSet[1,count]))
                        {
                            if (checkDup(count, a, b, c) == 0)
                            {
                                okSet[2,count] = c;
                                count++;
                                if (count >= 8)
                                    return;
                            }
                        }
                    }
                }
            }
        }
        private int myRowCheck(int row, int owner)
        {
            int myNum = 0;
            for (int i = 0; i < 3; i++)
            {
                if (GameGrid[row * 3 + i, 1] == owner)
                {
                    if (GameGrid[row * 3 + i, 0] != 0)
                    {
                        myNum++;
                    }
                }
                else if(GameGrid[row * 3 + i, 0] != 0)
                {
                    return -1;
                }
            }
            return myNum;
        }
        private int myColCheck(int col, int owner)
        {
            int myNum = 0;
            for (int i = 0; i < 3; i++)
            {
                if (GameGrid[i * 3 + col, 1] == owner)
                {
                    if (GameGrid[i * 3 + col, 0] != 0)
                    {
                        myNum++;
                    }
                }
                else if (GameGrid[i * 3 + col, 0] != 0)
                {
                    return -1;
                }
            }
            return myNum;
        }
        private int myRightCrossCheck(int owner)
        {
            int myNum = 0;
            if (GameGrid[0, 1] == owner)
            {
                if (GameGrid[0, 0] != 0)
                {
                    myNum++;
                }
            }
            else if (GameGrid[0,0]!=0)
            {
                return -1;
            }
            if (GameGrid[4, 1] == owner)
            {
                if (GameGrid[4, 0] != 0)
                {
                    myNum++;
                }
            }
            else if (GameGrid[4, 0] != 0)
            {
                return -1;
            }
            if (GameGrid[8, 1] == owner)
            {
                if (GameGrid[8, 0] != 0)
                {
                    myNum++;
                }
            }
            else if (GameGrid[8, 0] != 0)
            {
                return -1;
            }

            return myNum;
        }
        private int myLeftCrossCheck(int owner)
        {
            int myNum = 0;
            if (GameGrid[2, 1] == owner)
            {
                if (GameGrid[2, 0] != 0)
                {
                    myNum++;
                }
            }
            else if(GameGrid[2, 0] != 0)
            {
                return -1;
            }
            if (GameGrid[4, 1] == owner)
            {
                if (GameGrid[4, 0] != 0)
                {
                    myNum++;
                }
            }
            else if( GameGrid[4, 0] != 0)
            {
                return -1;
            }
            if (GameGrid[6, 1] == owner)
            {
                if (GameGrid[6, 0] != 0)
                {
                    myNum++;
                }
            }
            else if (GameGrid[6,0] != 0)
            {
                return -1;
            }
            return myNum;
        }
        private int getNum2(int num1)
        {
            int num2 = -1;
            int count = 0;
            for (int i = 0; i < 8; i++)
            {
                int val;
                if (okSet[0, i] == num1)
                {
                    val = okSet[1, i];
                    if (numUsed[val] > count && !IsUsed(val))
                    {
                        num2 = val;
                        count = numUsed[val];
                    }
                    val = okSet[2, i];
                    if (numUsed[val] > count && !IsUsed(val))
                    {
                        num2 = val;
                        count = numUsed[val];
                    }
                }
                if (okSet[1, i] == num1)
                {
                    val = okSet[0, i];
                    if (numUsed[val] > count && !IsUsed(val))
                    {
                        num2 = val;
                        count = numUsed[val];
                    }
                    val = okSet[2, i];
                    if (numUsed[val] > count && !IsUsed(val))
                    {
                        num2 = val;
                        count = numUsed[val];
                    }
                }
                if (okSet[2, i] == num1)
                {
                    val = okSet[0, i];
                    if (numUsed[val] > count && !IsUsed(val))
                    {
                        num2 = val;
                        count = numUsed[val];
                    }
                    val = okSet[1, i];
                    if (numUsed[val] > count && !IsUsed(val))
                    {
                        num2 = val;
                        count = numUsed[val];
                    }
                }
            }
            return num2;
        }
        private int rowLastFill(int row, int owner)
        {
            int index = -1;
            int sum = 0;
            for (int i = 0; i < 3; i++)
            {
                if (GameGrid[row * 3 + i, 1] == owner)
                {
                    sum += GameGrid[row * 3 + i, 0];
                }
                if (GameGrid[row * 3 + i, 0] == 0)
                {
                    index = row * 3 + i;
                }
            }
            if (index != -1)
            {
                int val = this.Target - sum;
                if (val > 0 && val < 10)
                {
                    if (!IsUsed(val))
                    {
                        GameGrid[index, 0] = val;
                        GameGrid[index, 1] = owner;
                        return index;
                    }
                }
            }
            return -1;
        }
        private int row2Fill(int row,int owner)
        {
            int col = 0;
            int num1 = 0;
            for(int i=0;i<3;i++)
            {
                if (GameGrid[row * 3+i,0] != 0)
                {
                    num1 = GameGrid[row * 3 + i, 0];
                    col = i;
                    break;
                }
            }
            int num2 = getNum2(num1);

            if (num2 > 0)
            {
                int fillIndex;
                if (col == 0)
                {
                    fillIndex = row * 3 + 2;
                }
                else
                {
                    fillIndex = row * 3;
                }
                GameGrid[fillIndex, 0] = num2;
                GameGrid[fillIndex, 1] = owner;
                return fillIndex;
            }
            else
            {
                return -1;
            }
        }

        private int colLastFill(int col, int owner)
        {
            int index = -1;
            int sum = 0;
            for (int i = 0; i < 3; i++)
            {
                if (GameGrid[i * 3 + col, 1] == owner)
                {
                    sum += GameGrid[i * 3 + col, 0];
                }
                if (GameGrid[i * 3 + col, 0] == 0)
                {
                    index = i * 3 + col;
                }
            }
            if (index != -1)
            {
                int val = this.Target - sum;
                if (val > 0 && val < 10)
                {
                    if (!IsUsed(val))
                    {
                        GameGrid[index, 0] = val;
                        GameGrid[index, 1] = owner;
                        return index;
                    }
                }
            }
            return -1;
        }
        private int col2Fill(int col, int owner)
        {
            int num1 = 0;
            int row = 0;
            for(int i=0;i<3;i++)
            {
                if (GameGrid[i*3+col,0]!= 0)
                {
                    num1 = GameGrid[i * 3 + col,0];
                    row = i;
                    break;
                }
            }
            int num2 = getNum2(num1);
            if (num2 > 0)
            {
                int fillIndex;
                if (row == 0)
                {
                    fillIndex = 2 * 3 + col;
                }
                else
                {
                    fillIndex = col;
                }
                GameGrid[fillIndex, 0] = num2;
                GameGrid[fillIndex, 1] = owner;
                return fillIndex;
            }
            else
            {
                return -1;
            }
        }
        private int rightCrossLastFill(int owner)
        {
            int sum = 0;
            int index = -1;
            if (GameGrid[0, 0] == 0)
            {
                index = 0;
            }
            else if (GameGrid[0, 1] == owner)
            {
                sum += GameGrid[0, 0];
            }
            if (GameGrid[4, 0] == 0)
            {
                index = 4;
            }
            else if (GameGrid[4, 1] == owner)
            {
                sum += GameGrid[4, 0];
            }
            if (GameGrid[8, 0] == 0)
            {
                index = 8;
            }
            else if (GameGrid[8, 1] == owner)
            {
                sum += GameGrid[8, 0];
            }
            if (index != -1)
            {
                int val = this.Target - sum;
                if (val > 0 && val < 10)
                {
                    if (!IsUsed(val))
                    {
                        GameGrid[index, 0] = val;
                        GameGrid[index, 1] = owner;
                        return index;
                    }
                }
            }
            return -1;
        }
        private int rightCross2Fill(int owner)
        {
            int num1;
            int fillIndex ;
            if (GameGrid[0,0] != 0)
            {
                num1 = GameGrid[0, 0];
                fillIndex = 8;
            }else if (GameGrid[4,0]!= 0)
            {
                num1 = GameGrid[4, 0];
                fillIndex = 0;
            }
            else
            {
                num1 = GameGrid[8, 0];
                fillIndex = 0;
            }
            int num2 = getNum2(num1);
            if (num2 > 0)
            {
                GameGrid[fillIndex, 0] = num2;
                GameGrid[fillIndex, 1] = owner;
                return fillIndex;
            }
            else
            {
                return -1;
            }
        }
        private int leftCrossLastFill(int owner)
        {
            int sum = 0;
            int index = -1;
            if (GameGrid[2, 0] == 0)
            {
                index = 2;
            }
            else if (GameGrid[2, 1] == owner)
            {
                sum += GameGrid[2, 0];
            }
            if (GameGrid[4, 0] == 0)
            {
                index = 4;
            }
            else if (GameGrid[4, 1] == owner)
            {
                sum += GameGrid[4, 0];
            }
            if (GameGrid[6, 0] == 0)
            {
                index = 6;
            }
            else if (GameGrid[6, 1] == owner)
            {
                sum += GameGrid[6, 0];
            }
            if (index != -1)
            {
                int val = this.Target - sum;
                if (val > 0 && val < 10)
                {
                    if (!IsUsed(val))
                    {
                        GameGrid[index, 0] = val;
                        GameGrid[index, 1] = owner;
                        return index;
                    }
                }
            }
            return -1;
        }
        private int leftCross2Fill(int owner)
        {
            int num1;
            int fillIndex;
            if (GameGrid[2,0]!= 0)
            {
                num1 = GameGrid[2, 0];
                fillIndex = 6;
            }
            else if(GameGrid[4,0]!= 0)
            {
                num1 = GameGrid[4, 0];
                fillIndex = 2;
            }
            else
            {
                num1 = GameGrid[6, 0];
                fillIndex = 2;
            }
            int num2 = getNum2(num1);
            if (num2 >0)
            {
                GameGrid[fillIndex, 0] = num2;
                GameGrid[fillIndex, 1] = owner;
                return fillIndex;
            }
            else
            {
                return -1;
            }
        }
        public int AIGo()
        {
            //先看是否有一招制胜的可能
            for(int i=0;i<3;i++)
            {
                if (myRowCheck(i,1) == 2)
                {
                    if (rowLastFill(i,1) != -1)
                    {
                        aiWon = true;
                        gameOver = true;
                        return 999;
                    }
                }
                else if (myColCheck(i,1) == 2)
                {
                    if (colLastFill(i,1) != -1)
                    {
                        aiWon = true;
                        gameOver = true;
                        return 999;
                    }
                }
                else if(myRightCrossCheck(1) ==2)
                {
                    if (rightCrossLastFill(1) !=-1)
                    {
                        aiWon = true;
                        gameOver = true;
                        return 999;
                    }
                }
                else if(myLeftCrossCheck(1) == 2)
                {
                    if (leftCrossLastFill(1)!= -1)
                    {
                        aiWon = true;
                        gameOver = true;
                        return 999;
                    }
                }
            }
            //其次，已经有一个数字的对角线/行/列优先
            if (myRightCrossCheck(1) == 1)
            {
                if (rightCross2Fill(1) != -1)
                {
                    return 0;
                }
            }
            if (myLeftCrossCheck(1) ==1)
            {
                if (leftCross2Fill(1) != -1)
                {
                    return 0;
                }
            }
            for (int i=0;i<3;i++)
            {
                if (myRowCheck(i,1) == 1)
                {
                    if (row2Fill(i,1) != -1)
                    {
                        return 0;
                    }
                }
                else
                {
                    if (myColCheck(i,1) == 1)
                    {
                        if (col2Fill(i,1) != -1)
                        {
                            return 0;
                        }
                    }
                }
            }
            int goIndex = -1;

            if (GameGrid[4, 0] == 0) //先占中
            {
                goIndex = 4;
            }
            else  //否则选角
            {
                //如果中间被我方所占
                if (GameGrid[4, 1] == 1)
                {
                    //如果左上空，第一行第一列都空或者为我所占
                    if (GameGrid[0, 0] == 0 && (GameGrid[1, 0] == 0 || GameGrid[1, 1] == 1) && (GameGrid[2, 0] == 0 || GameGrid[2, 1] == 1))
                    {
                        if ((GameGrid[3, 0] == 0 || GameGrid[3, 1] == 1) && (GameGrid[6, 0] == 0 || GameGrid[6, 1] == 1))
                        {
                            goIndex = 0; //选左上
                        }
                    }
                    //如果右上空，第一行 第三列都空或者为我所占
                    else if (GameGrid[2, 0] == 0 && (GameGrid[0, 0] == 0 || GameGrid[0, 1] == 1) && (GameGrid[1, 0] == 0 || GameGrid[1, 1] == 1))
                    {
                        if ((GameGrid[5, 0] == 0 || GameGrid[5, 1] == 1) && (GameGrid[8, 0] == 0 || GameGrid[8, 1] == 1))
                        {
                            goIndex = 2; //选右上
                        }
                    }

                    else if ((GameGrid[6, 0] == 0) && (GameGrid[3, 0] == 0 || GameGrid[3, 1] == 1) && (GameGrid[0, 0] == 0 || GameGrid[0, 1] == 1))
                    {
                        if ((GameGrid[7, 0] == 0 || GameGrid[7, 1] == 1) && (GameGrid[8, 0] == 0 || GameGrid[8, 1] == 1))
                        {
                            goIndex = 6;
                        }
                    }
                    else if ((GameGrid[8, 0] == 0) && (GameGrid[6, 0] == 0 || GameGrid[6, 1] == 1) && (GameGrid[7, 0] == 0 || GameGrid[7, 1] == 1))
                    {
                        if ((GameGrid[2, 0] == 0 || GameGrid[2, 1] == 1) && (GameGrid[5, 0] == 0 || GameGrid[5, 1] == 1))
                        {
                            goIndex = 8;
                        }
                    }
                }
            }
            //查找一个整行整列空白或者都是我方可占
            if (goIndex == -1)
            {
                
                bool filled;
                for (int i=0;i<3;i++)
                    for(int j=0;j<3;j++)
                    {
                        filled = false;
                        if (GameGrid[i*3+j,0] == 0)
                        {
                            for(int r=0;r<3;r++)
                            {
                                if (GameGrid[i*3+r,0] != 0 && GameGrid[i * 3 + r, 1] != 1)
                                    filled = true;
                            }
                            if (!filled)
                            {
                                goIndex = i * 3 + j;
                                break;
                            }
                        }
                        if (filled)
                        {
                            filled = false;
                            for (int c = 0; c < 3; c++)
                            {
                                if (GameGrid[c * 3 + j, 1] != 1)
                                {
                                    filled = true;
                                }
                            }
                            if (!filled)
                            {
                                goIndex = i * 3 + j;
                            }
                        }
                    }
                //没有优势行，随便选
                if (goIndex == -1)
                {
                    for (int i = 0; i < 9; i++)
                    {
                        if (GameGrid[i, 0] == 0)
                        {
                            for (int r = 0; r < 3; i++)
                                goIndex = i;
                            break;
                        }
                    }
                }
            }
            if (goIndex != -1)
            {
                GameGrid[goIndex, 0] = getMostUnusedNum();
                GameGrid[goIndex, 1] = 1;
            }
            return 0;
        }
        public bool checkDeadlock()
        {
            for (int i = 0; i < 9; i++)
            {
                if (GameGrid[i, 0] == 0)
                {
                    int row = i / 3;
                    int col = i % 3;
                    //行
                    int sum0 = 0;
                    int sum1 = 0;
                    int count0 = 0;
                    int count1 = 0;
                    for (int j = 0; i < 3; i++)
                    {
                        if (GameGrid[row * 3 + j, 1] == 0)
                        {
                            sum0 += GameGrid[row * 3 + j, 0];
                            count0++;
                        }
                        else if (GameGrid[row * 3 + j, 1] == 1)
                        {
                            sum1 += GameGrid[row * 3 + j, 0];
                            count1++;
                        }
                    }
                    if (count0 > 0)
                    {
                        if (count0 ==2)
                        {
                            if (!IsUsed(Target - sum0)) //任何还有一个可解的组合都表明游戏还可以进一步玩下去
                                return false;
                        }
                        else
                        {
                            return false; //双位空也可能已经右僵局，先不管
                        }
                    }
                    if (count1 >0)
                    {
                        if (count1 == 2)
                        {
                            if (!IsUsed(Target - sum1))
                                return false;
                        }
                        else
                        {
                            return false;
                        }
                    }
                    //列
                    sum0 = 0;
                    sum1 = 0;
                    count0 = 0;
                    count1 = 0;
                    for (int j = 0; i < 3; i++)
                    {
                        if (GameGrid[i * 3 + col, 1] == 0)
                        {
                            sum0 += GameGrid[j * 3 + col, 0];
                            count0++;
                        }
                        else if (GameGrid[j * 3 + col, 1] == 1)
                        {
                            sum1 += GameGrid[j * 3 + col, 0];
                            count1++;
                        }
                    }
                    if (count0 >0)
                    {
                        if (count0 == 2)
                        {
                            if (!IsUsed(Target - sum0))
                                return false;
                        }
                        else
                        {
                            return false;
                        }
                    }
                    if (count1 >0)
                    {
                        if (count1 == 2)
                        {
                            if (!IsUsed(Target - sum1))
                                return false;
                        }
                        else
                        {
                            return false;
                        }
                    }
                    //右对角
                    if (i == 0 || i == 4 || i == 8)
                    {
                        sum0 = 0;
                        sum1 = 0;
                        count0 = 0;
                        count1 = 0;
                        if (GameGrid[0, 1] == 0)
                        {
                            sum0 += GameGrid[0, 0];
                            count0++;
                        }
                        else if (GameGrid[0, 1] == 1)
                        {
                            sum1 += GameGrid[0, 0];
                            count1++;
                        }
                        if (GameGrid[4, 1] == 0)
                        {
                            sum0 += GameGrid[4, 0];
                            count0++;
                        }
                        else if (GameGrid[4, 1] == 1)
                        {
                            sum1 += GameGrid[4, 0];
                            count1++;
                        }
                        if (GameGrid[8, 1] == 0)
                        {
                            sum0 += GameGrid[8, 0];
                            count0++;
                        }
                        else if (GameGrid[8, 1] == 1)
                        {
                            sum1 += GameGrid[8, 0];
                            count1++;
                        }
                        if (count0 >0)
                        {
                            if (count0 == 2)
                            {
                                if (!IsUsed(Target - sum0))
                                    return false;
                            }
                            else
                            {
                                return false;
                            }
                        }
                        if (count1 >0)
                        {
                            if (count1 == 2)
                            {
                                if (!IsUsed(Target - sum1))
                                    return false;
                            }
                            else
                            {
                                return false;
                            }
                        }

                    }
                    //左对角
                    if (i == 2 || i == 4 || i == 6)
                    {
                        sum0 = 0;
                        sum1 = 0;
                        count0 = 0;
                        count1 = 0;
                        if (GameGrid[2, 1] == 0)
                        {
                            sum0 += GameGrid[2, 0];
                            count0++;
                        }
                        else if (GameGrid[2, 1] == 1)
                        {
                            sum1 += GameGrid[2, 0];
                            count1++;
                        }
                        if (GameGrid[4, 1] == 0)
                        {
                            sum0 += GameGrid[4, 0];
                            count0++;
                        }
                        else if (GameGrid[4, 1] == 1)
                        {
                            sum1 += GameGrid[4, 0];
                            count1++;
                        }
                        if (GameGrid[6, 1] == 0)
                        {
                            sum0 += GameGrid[6, 0];
                            count0++;
                        }
                        else if (GameGrid[6, 1] == 1)
                        {
                            sum1 += GameGrid[6, 0];
                            count1++;
                        }
                        if (count0 >0)
                        {
                            if (count0 == 2)
                            {
                                if (!IsUsed(Target - sum0))
                                    return false;
                            }
                            else
                            {
                                return false;
                            }
                        }
                        if (count1 >0)
                        {
                            if (count1 == 2)
                            {
                                if (!IsUsed(Target - sum1))
                                    return false;
                            }
                            else
                            {
                                return false;
                            }
                        }
                    }
                }
            }
            gameOver = true;
            return true;
        }
        public bool checkWon(int row,int col)
        {
            bool leftCross = false;
            bool rightCross = false;
            if (row ==2 && col ==2)
            {
                leftCross = true;
                rightCross = true;
            }
            if ((row ==0 && col ==0) ||(row==2 && col ==2))
            {
                rightCross = true;
            }
            if ((row ==0 && col ==2) ||(row==2 && col ==0))
            {
                leftCross = true;
            }
            if (rightCross)
            {
                if (GameGrid[0,0] + GameGrid[4,0] + GameGrid[8,0] == this.Target)
                {
                    if (GameGrid[0, 1] == 0 && GameGrid[4, 1] == 0 && GameGrid[8, 1] == 0)
                    {
                        gameOver = true;
                        return true;
                    }
                }
            }
            if (leftCross)
            {
                if (GameGrid[2, 0] + GameGrid[4, 0] + GameGrid[6, 0] == this.Target)
                {
                    if (GameGrid[2, 1] == 0 && GameGrid[4, 1] == 0 && GameGrid[6, 1] == 0)
                    {
                        gameOver = true;
                        return true;
                    }
                }
            }
            //check row
            int sum = 0;
            for(int i = 0;i<3;i++)
            {
                if (GameGrid[row *3 + i,1] == 0)
                {
                    sum += GameGrid[row * 3 + i, 0];
                }
            }
            if (sum == this.Target)
            {
                gameOver = true;
                return true;
            }
            //check col
            sum = 0;
            for(int i=0;i<3;i++)
            {
                if (GameGrid[i * 3 + col, 1] == 0)
                {
                    sum += GameGrid[i * 3 + col, 0];
                }
            }
            if (sum == this.Target)
            {
                gameOver = true;
                return true;
            }
            return false;
        }
        public string getOkSetText()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("有效组合：{");
            for(int i=0;i<8;i++)
            {
                //sb.Append("{");
                sb.Append(okSet[0, i].ToString());
                sb.Append(",");
                sb.Append(okSet[1, i].ToString());
                sb.Append(",");
                sb.Append(okSet[2, i].ToString());
                sb.Append(" ");
            }
            sb.Append("}\n\r");
            return sb.ToString();
        }
        public string getNumUsedText()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("次数统计:{");
            for(int i=1;i<10;i++)
            {
                sb.Append(i.ToString());
                sb.Append(":");
                sb.Append(numUsed[i].ToString());
                sb.Append(" ");
            }
            sb.Append("}");
            return sb.ToString();
        }
        //查看四个边角空格是否最空
        public bool isCornerGridFree(int index)
        {
            switch (index)
            {
                case 0:
                    if (GameGrid[0, 0] == 0 && (GameGrid[1, 0] == 0 || GameGrid[1, 1] == 1) && (GameGrid[2, 0] == 0 || GameGrid[2, 1] == 1))
                    {
                        if ((GameGrid[3,0] == 0 || GameGrid[3,1] == 1) && (GameGrid[6,0] ==0 || GameGrid[6,1] == 1))
                        {
                            return true;
                        }
                    }
                    break;
                case 2:
                    if (GameGrid[2, 0] == 0 && (GameGrid[0, 0] == 0 || GameGrid[0, 1] == 1) && (GameGrid[1, 0] == 0 || GameGrid[1, 1] == 1))
                    {
                        if ((GameGrid[5, 0] == 0 || GameGrid[5, 1] == 1) && (GameGrid[8, 0] == 0 || GameGrid[8, 1] == 1))
                        {
                            return true;
                        }
                    }
                    break;
                case 6:
                    if ((GameGrid[6,0] == 0) &&(GameGrid[3,0] ==0 || GameGrid[3,1] ==1) &&(GameGrid[0,0] ==0 || GameGrid[0,1] ==1))
                    {
                        if ((GameGrid[7,0] ==0 || GameGrid[7,1] ==1) &&(GameGrid[8,0] ==0 || GameGrid[8,1] ==1))
                        {
                            return true;
                        }
                    }
                    break;
                case 8:
                    if ((GameGrid[8,0] ==0) && (GameGrid[6,0] ==0 || GameGrid[6,1] ==1) &&(GameGrid[7,0]==0 || GameGrid[7,1] ==1))
                    {
                        if ((GameGrid[2,0]==0 || GameGrid[2,1] ==1) &&(GameGrid[5,0] ==0 || GameGrid[5,1] ==1))
                        {
                            return true;
                        }
                    }
                    break;

            }
            return false;
        }
        public int getMostUnusedNum()
        {
            int num, count;
            count = 0;
            num = -1;
            for(int i=1;i<10;i++)
            {
                if (!IsUsed(i))
                {
                    if (numUsed[i] > count)
                    {
                        num = i;
                        count = numUsed[i];
                    }
                }
            }
            return num;
        }
        public void reset()
        {
            for(int i = 0;i<9;i++)
            {
                GameGrid[i, 0] = 0;
                GameGrid[i, 1] = -1;
            }
            aiWon = false;
            gameOver = false;
            getOkSet();
            getNumUsed();
            if (aiFirst)
            {
                AIGo();
            }
        }
    }
}