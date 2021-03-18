using System;
using System.Collections.Generic;

namespace Fillwords
{
    public static class PrinterLogicMetods
    {
        //Действия при движении курсора по полю
        static public void PlayerMoveAction(Field field, bool isEnter)
        {
            if (!isEnter)
            {
                Printer.DrawFieldItem(Player.PreX, Player.PreY, ColorsSet.ColorsList[field.CellColor[Player.PreX, Player.PreY],0],
                                             ColorsSet.ColorsList[field.CellColor[Player.PreX, Player.PreY],1], field);
                Printer.DrawFieldItem(Player.X, Player.Y, ColorsSet.ColorsList[Settings.UnderCursorColor, 0], 
                                                          ColorsSet.ColorsList[Settings.UnderCursorColor, 1], field);
            }
            else
            {
                Printer.DrawFieldItem(Player.PreX, Player.PreY, ColorsSet.ColorsList[Settings.PickedWordColor, 0],
                                                                ColorsSet.ColorsList[Settings.PickedWordColor, 1], field);
                Printer.DrawFieldItem(Player.X, Player.Y, ColorsSet.ColorsList[Settings.UnderCursorColor, 0],
                                                          ColorsSet.ColorsList[Settings.UnderCursorColor, 1], field);
                Player.WordNow += field.CellLetter[Player.X, Player.Y];
                Printer.DrawText(Player.WordNow, Player.WordsList.Count);
                Player.CoordStory.Add(new int[] { Player.X, Player.Y });
            }
        }

        //Действия при нажатии Enter
        static public void PlayerEnterAction(Field field, ref bool isEnter, string[] allWords)
        {
            if (!isEnter)
            {
                Printer.DrawText(new string(' ', Console.WindowWidth - (field.XSize * 4 + 2)), Player.WordsList.Count);

                if (field.CellColor[Player.X, Player.Y] == Settings.FieldColor)
                {
                    Printer.DrawFieldItem(Player.X, Player.Y, ColorsSet.ColorsList[Settings.PickedWordColor, 0],
                                                              ColorsSet.ColorsList[Settings.PickedWordColor, 1], field);
                    Player.WordNow += field.CellLetter[Player.X, Player.Y];
                    Printer.DrawText(Player.WordNow, Player.WordsList.Count);
                    Player.CoordStory.Add(new int[] { Player.X, Player.Y });
                }
                else
                    isEnter = !isEnter;
            }
            else
            {
                if (field.WordsList.Contains(Player.WordNow) &&
                    field.WordPos[field.WordsList.IndexOf(Player.WordNow)][Player.WordNow.Length - 1].X == Player.X)
                {
                    int color;
                    if (Settings.IsRandomGuessedWordColro) color = ColorsSet.GetRandomColor();
                    else                                   color = Settings.GuessedWordColor;

                    for (int i = 0; i < Player.CoordStory.Count; i++)
                    {
                        int x = Player.CoordStory[i][0];
                        int y = Player.CoordStory[i][1];

                        field.CellColor[x, y] = color;
                        field.CellColor[x, y] = color;
                    }

                    Player.WordsList.Add(Player.WordNow);
                    Player.Score += (int)Math.Pow(Player.WordNow.Length, 1.5);
                    Printer.DrawScore(Player.Score);
                }
                else
                {
                    if (field.WordsList.Contains(Player.WordNow))
                        Printer.DrawText("Попробуйте записать это слово наоборот или найти ещё одно такое же на поле",  Player.WordsList.Count);
                    else
                    if ((allWords as IList<string>).Contains(Player.WordNow))
                        Printer.DrawText("Это не одно из слов, которое вам нужно отгодать на данном поле ):", Player.WordsList.Count);
                    else
                        Printer.DrawText("Такого слова нет в словаре", Player.WordsList.Count);
                }

                BrakeFilling(field);
            }

            isEnter = !isEnter;
        }

        //Прекращение заполнения слова
        static public void BrakeFilling(Field field)
        {
            for (int i = 0; i < Player.CoordStory.Count; i++)
            {
                int x = Player.CoordStory[i][0];
                int y = Player.CoordStory[i][1];

                Printer.DrawFieldItem(x, y, ColorsSet.ColorsList[field.CellColor[x, y],0], ColorsSet.ColorsList[field.CellColor[x, y],1], field);
            }
            Printer.DrawFieldItem(Player.X, Player.Y, ColorsSet.ColorsList[Settings.UnderCursorColor, 0],
                                                      ColorsSet.ColorsList[Settings.UnderCursorColor, 1], field);

            Player.WordNow = string.Empty;
            Player.CoordStory.RemoveRange(0, Player.CoordStory.Count);
        }

    }
}
