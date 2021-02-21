using System;
using System.Collections.Generic;

namespace Fillwords
{
    class Field
    {
        Random rnd = new Random();
        
        public List<string> wordsList = new List<string>();      //лист слов на поле
        public List<List<MyVector2>> wordPos = new List<List<MyVector2>>();  //лист координат каждой буквы каждого слова
        public int xSize, ySize;                                 //размер поля
        public char[,] cellLetter;                               //поле букв
        public ConsoleColor[,,] cellColor;
        public bool isLoaded = false;

        public void CreateNewField(int input1, int input2, WordsSet words)
        {
            xSize = input1;
            ySize = input2;
            cellLetter = new char[xSize, ySize];
            cellColor = new ConsoleColor[xSize, ySize, 2];

            //создание поля свободных ячеек
            bool[,] preField = new bool[xSize + 2, ySize + 2];
            for (int y = 0; y <= ySize + 1; y++)
                for (int x = 0; x <= xSize + 1; x++)
                    if (x == xSize + 1 || y == 0 || x == 0 || y == ySize + 1) preField[x, y] = false;
                    else preField[x, y] = true;

            //Заполнение поля шаблонами слов
            MyVector2 starcCoord = GetStartCoord();
            int dir = GetStartDirection(starcCoord.X, starcCoord.Y, preField);

            int[,] numField = new int[xSize, ySize];
            var coordList = new List<MyVector2>();
            int openCellNum = xSize * ySize;
            int actionMod = 0;
            {
                int x = starcCoord.X;
                int y = starcCoord.Y;
                while (openCellNum != 0)
                {
                    if (preField[x, y])
                    {
                        preField[x, y] = false;
                        openCellNum--;
                        numField[x - 1, y - 1] = xSize * ySize - openCellNum;
                        coordList.Add(new MyVector2( x - 1, y - 1 ));
                    }

                    MyVector2 coordLocal = GetNextCellCoord(x, y, dir);

                    if (!preField[coordLocal.X, coordLocal.Y])
                    {
                        int oldDir = dir;
                        dir = FindDirection(x, y, preField);
                        if (dir == 0)
                            if (openCellNum > 0)
                                throw new Exception("На поле остались пустые ячейки, до которыйх невозможно добраться");
                            else
                                break;

                        if (actionMod == 0)
                        {
                            if (rnd.Next(3) == 0) actionMod = 1;
                        }
                        else if (actionMod == 1)
                        {
                            if (rnd.Next(3) == 0) actionMod = 0;

                            MyVector2 coordNext = GetNextCellCoord(x, y, dir);
                            MyVector2 coordNext2 = GetNextCellCoord(coordNext.X, coordNext.Y, (oldDir + 1) % 4 + 1);

                            if (preField[coordNext2.X, coordNext2.Y])
                            {
                                preField[coordNext.X, coordNext.Y] = false;
                                openCellNum -= 1;
                                numField[coordNext.X - 1, coordNext.Y - 1] = xSize * ySize - openCellNum;
                                coordList.Add(new MyVector2( coordNext.X - 1, coordNext.Y - 1 ));
                                x = coordNext2.X;
                                y = coordNext2.Y;
                                dir = (oldDir + 1) % 4 + 1;
                            }
                            else
                            {
                                actionMod = 0;
                            }
                        }
                    }
                    else
                    {
                        x = coordLocal.X;
                        y = coordLocal.Y;
                    }
                }
            }

            //Разбиение червяка на слова
            int cellNum;
            List<int> wordsLenghtList;
            int tryNum = 0;
            do
            {
                wordsList = new List<string>();
                cellNum = xSize * ySize;
                List<int> wordsLenghtNum = new List<int>();
                wordsLenghtList = new List<int>();

                for (int i = 0; i < words.wordsSet.Count; i++)
                    wordsLenghtNum.Add(words.wordsSet[i].Count);

                do
                {
                    int wordLenght = 0;
                    int i = 0;
                    do
                    {
                        i++;
                        if (i >= 50 || cellNum <= 2)
                        {
                            wordLenght = 0;
                            break;
                        }
                        wordLenght = rnd.Next(Math.Min(wordsLenghtNum.Count - 1, cellNum) - 2)+3;
                        if (wordLenght < 5 || wordLenght > 8) wordLenght = rnd.Next(Math.Min(wordsLenghtNum.Count - 1, cellNum) - 2) + 3;
                        if (wordLenght < 5 || wordLenght > 8) wordLenght = rnd.Next(Math.Min(wordsLenghtNum.Count - 1, cellNum) - 2) + 3;
                    } while (wordsLenghtNum[wordLenght] == 0);

                    if (wordLenght == 0) break;

                    wordsLenghtNum[wordLenght]--;
                    cellNum -= wordLenght;
                    wordsLenghtList.Add(wordLenght);
                    wordsList.Add(words.wordsSet[wordLenght][rnd.Next(words.wordsSet[wordLenght].Count)]);
                } while (cellNum > 0);
                if (cellNum == 0 && wordsList.Count < Settings.wordNumMin) continue;
                tryNum++;
                if (tryNum > 10000)
                {
                    Printer.DrawPopupWindow("Словарь \"words.txt\" слишком маленький. Добавьте больше слов");
                    Environment.Exit(0);
                }
            } while (cellNum > 0);

            //Заполнение словами поля
            int step = 0;
            for(int wordNum = 0; wordNum < wordsList.Count; wordNum++)
            {
                wordPos.Add(new List<MyVector2>());
                for (int letterNum = 0; letterNum < wordsList[wordNum].Length; letterNum++)
                {
                    char letter = wordsList[wordNum][letterNum];
                    int x = coordList[step].X;
                    int y = coordList[step].Y;
                    wordPos[wordNum].Add(coordList[step]);
                    cellLetter[x, y] = letter;
                    cellColor[x, y, 0] = Settings.Colors[Settings.fieldColor, 0];
                    cellColor[x, y, 1] = Settings.Colors[Settings.fieldColor, 1];
                    step++;
                }
            }
        }

        private MyVector2 GetStartCoord()
        {
            MyVector2 output = new MyVector2();
            if (rnd.Next(2) == 0)
            {
                if (rnd.Next(2) == 0)
                {
                    output.X = 1;
                    output.Y = 1;
                }
                else
                {
                    output.X = 1;
                    output.Y = ySize;
                }
            }
            else
            {
                if (rnd.Next(2) == 0)
                {
                    output.X = xSize;
                    output.Y = 1;
                }
                else
                {
                    output.X = xSize;
                    output.Y = ySize;
                }
            }

            return output;
        }

        private int FindDirection(int x, int y, bool[,] field)
        {
            List<int> dirList = new List<int>() { 1, 2, 3, 4 };

            if (!field[x + 1, y    ]) dirList.Remove(1);
            if (!field[x    , y - 1]) dirList.Remove(2);
            if (!field[x - 1, y    ]) dirList.Remove(3);
            if (!field[x    , y + 1]) dirList.Remove(4);

            if (dirList.Count != 0) 
                return dirList[rnd.Next(dirList.Count)];
            else 
                return 0;
        }

        private int GetStartDirection(int x, int y, bool[,] field)
        {
            int output = 0;
            if (!field[x + 1, y]) output = (rnd.Next(2) == 0) ? 2 : 4;
            if (!field[x, y - 1]) output = (rnd.Next(2) == 0) ? 1 : 3;
            if (!field[x - 1, y]) output = (rnd.Next(2) == 0) ? 2 : 4;
            if (!field[x, y + 1]) output = (rnd.Next(2) == 0) ? 1 : 3;

            return output;
        }

        private MyVector2 GetNextCellCoord(int x, int y, int dir)
        {
            return new MyVector2( x + (-(dir - 2) % 2), y + ((dir - 3) % 2) );
        }
    }
}
