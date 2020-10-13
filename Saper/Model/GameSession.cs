using Saper.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Saper
{
    public enum Level
    {
        Легкая,
        Средняя,
        Тяжелая,
        Индивидуальная
    }
    class GameSession
    {
        int mineCount;
        int flagsCount;
        readonly int mineDensity = 5; //Плотность мин - одна мина/количество клеток
        int foundMinesCount;
        int Width { get; set; }
        int Height { get; set; }

        Level Difficulty;

        int timerCount;
        Cell[,] cellField;
        Random rand = new Random();
        public event Action VictoryEvent;
        public event Action<int> FlagsCountChange;
        public event Action<int, int> SessionCreated;
        public event Action<int, int> MarkEvent;
        public event Action<int, int> UnmarkEvent;
        public event Action<int, int, int> OpenBorderEvent;
        public event Action<int, int> OpenMineEvent;
        public event Action<int, int> OpenEmptyEvent;
        public event Action<string, string, string> NewRecordEvent;

        public GameSession(Level difficulty, int width = 0, int height = 0)
        {
            Difficulty = difficulty;

            if (Difficulty == Level.Легкая)
            {
                Width = 9;
                Height = 9;
            }
            else if (Difficulty == Level.Средняя)
            {
                Width = 16;
                Height = 16;
            }
            else if (Difficulty == Level.Тяжелая)
            {
                Width = 16;
                Height = 30;
            }
            else if (Difficulty == Level.Индивидуальная) 
            {
                Width = width;
                Height = height;
            }

            mineCount = Width * Height / mineDensity;
            flagsCount = mineCount;
            cellField = new Cell[Width, Height];
            CreateCellField();
        }

        #region Создание сессии игры
        private void CreateCellField()
        {
            for (int i = 0; i < Width; i++)
            {
                for (int j = 0; j < Height; j++)
                {
                    cellField[i, j] = new Cell() { PositionX = i, PositionY = j };
                }
            }
        }

        private void AddMines()
        {
            for (int i = 0; i < mineCount;)
            {
                int x = rand.Next(0, Width);
                int y = rand.Next(0, Height);

                if (!cellField[x, y].IsMined)
                {
                    cellField[x, y].IsMined = true;
                    i++;
                }
            }
        }

        private void PrepareFieldInfo()
        {
            for (int i = 0; i < Width; i++)
            {
                for (int j = 0; j < Height; j++)
                {
                    int count = 0;

                    if (!cellField[i, j].IsMined)
                    {
                        if (i + 1 < Width && cellField[i + 1, j].IsMined)
                        {
                            count++;
                            cellField[i, j].IsBorder = true;
                        }
                        if (i - 1 >= 0 && cellField[i - 1, j].IsMined)
                        {
                            count++;
                            cellField[i, j].IsBorder = true;
                        }
                        if (j + 1 < Height && cellField[i, j + 1].IsMined)
                        {
                            count++;
                            cellField[i, j].IsBorder = true;
                        }
                        if (j - 1 >= 0 && cellField[i, j - 1].IsMined)
                        {
                            count++;
                            cellField[i, j].IsBorder = true;
                        }
                        if (i + 1 < Width && j + 1 < Height && cellField[i + 1, j + 1].IsMined)
                        {
                            count++;
                            cellField[i, j].IsBorder = true;
                        }
                        if (i + 1 < Width && j - 1 >= 0 && cellField[i + 1, j - 1].IsMined)
                        {
                            count++;
                            cellField[i, j].IsBorder = true;
                        }
                        if (i - 1 >= 0 && j - 1 >= 0 && cellField[i - 1, j - 1].IsMined)
                        {
                            count++;
                            cellField[i, j].IsBorder = true;
                        }
                        if (i - 1 >= 0 && j + 1 < Height && cellField[i - 1, j + 1].IsMined)
                        {
                            count++;
                            cellField[i, j].IsBorder = true;
                        }

                        cellField[i, j].MinesNear = count;
                    }
                }
            }
        }
        #endregion

        public void StartGame()
        {
            AddMines();
            PrepareFieldInfo();
            timerCount = -1;
            FlagsCountChange(flagsCount);
            SessionCreated(Width, Height);
        }

        public void RestartGame() 
        {
            CreateCellField();
            AddMines();
            PrepareFieldInfo();
            timerCount = -1;
            foundMinesCount = 0;
            flagsCount = mineCount;
            FlagsCountChange(flagsCount);
            SessionCreated(Width, Height);
        }

        /// <summary>
        /// Алгоритм очистки поля при клике по пустой клетке
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        public void ClearAlgorithm(int x, int y)
        {
            List<Cell> TempList = new List<Cell>();
            
            if (!cellField[x, y].IsMined)
            {
                if (!cellField[x, y].IsBorder)
                {
                    cellField[x, y].IsVisited = true;
                    TempList.Add(cellField[x, y]);
                }
                else
                {
                    cellField[x, y].IsVisited = true;
                }
            }

            #region Старая версия
            //for (int i = 0; i < TempList.Count;)
            //{
            //    x = TempList[i].PositionX;
            //    y = TempList[i].PositionY;

            //    if (x + 1 < 20 && !cellField[x + 1, y].IsVisited)
            //    {
            //        if (!cellField[x + 1, y].IsBorder)
            //        {
            //            cellField[x + 1, y].IsVisited = true;
            //            TempList.Add(cellField[x + 1, y]);
            //        }
            //        else
            //        {
            //            cellField[x + 1, y].IsVisited = true;
            //        }
            //    }

            //    if (x - 1 >= 0 && !cellField[x - 1, y].IsVisited)
            //    {
            //        if (!cellField[x - 1, y].IsBorder)
            //        {
            //            cellField[x - 1, y].IsVisited = true;
            //            TempList.Add(cellField[x - 1, y]);
            //        }
            //        else
            //        {
            //            cellField[x - 1, y].IsVisited = true;
            //        }
            //    }

            //    if (y + 1 < 10 && !cellField[x, y + 1].IsVisited)
            //    {
            //        if (!cellField[x, y + 1].IsBorder)
            //        {
            //            cellField[x, y + 1].IsVisited = true;
            //            TempList.Add(cellField[x, y + 1]);
            //        }
            //        else
            //        {
            //            cellField[x, y + 1].IsVisited = true;
            //        }
            //    }

            //    if (y - 1 >= 0 && !cellField[x, y - 1].IsVisited)
            //    {
            //        if (!cellField[x, y - 1].IsBorder)
            //        {
            //            cellField[x, y - 1].IsVisited = true;
            //            TempList.Add(cellField[x, y - 1]);
            //        }
            //        else
            //        {
            //            cellField[x, y - 1].IsVisited = true;
            //        }
            //    }

            //    if (x + 1 < 20 && y + 1 < 10 && !cellField[x + 1, y + 1].IsVisited)
            //    {
            //        if (!cellField[x + 1, y + 1].IsBorder)
            //        {
            //            cellField[x + 1, y + 1].IsVisited = true;
            //            TempList.Add(cellField[x + 1, y + 1]);
            //        }
            //        else
            //        {
            //            cellField[x + 1, y + 1].IsVisited = true;
            //        }
            //    }
            //    if (x + 1 < 20 && y - 1 >= 0 && !cellField[x + 1, y - 1].IsVisited)
            //    {
            //        if (!cellField[x + 1, y - 1].IsBorder)
            //        {
            //            cellField[x + 1, y - 1].IsVisited = true;
            //            TempList.Add(cellField[x + 1, y - 1]);
            //        }
            //        else
            //        {
            //            cellField[x + 1, y - 1].IsVisited = true;
            //        }
            //    }
            //    if (x - 1 >= 0 && y - 1 >= 0 && !cellField[x - 1, y - 1].IsVisited)
            //    {
            //        if (!cellField[x - 1, y - 1].IsBorder)
            //        {
            //            cellField[x - 1, y - 1].IsVisited = true;
            //            TempList.Add(cellField[x - 1, y - 1]);
            //        }
            //        else
            //        {
            //            cellField[x - 1, y - 1].IsVisited = true;
            //        }
            //    }
            //    if (x - 1 >= 0 && y + 1 < 10 && !cellField[x - 1, y + 1].IsVisited)
            //    {
            //        if (!cellField[x - 1, y + 1].IsBorder)
            //        {
            //            cellField[x - 1, y + 1].IsVisited = true;
            //            TempList.Add(cellField[x - 1, y + 1]);
            //        }
            //        else
            //        {
            //            cellField[x - 1, y + 1].IsVisited = true;
            //        }
            //    }

            //    TempList.Remove(TempList[i]);
            //    i = 0;
            //}
            #endregion

            #region Вторая версия
            while (TempList.Count != 0)
            {
                x = TempList[0].PositionX;
                y = TempList[0].PositionY;

                if (x + 1 < Width && !cellField[x + 1, y].IsVisited)
                {
                    if (!cellField[x + 1, y].IsBorder && !TempList.Contains(cellField[x + 1, y]))
                    {
                        cellField[x + 1, y].IsVisited = true;
                        TempList.Add(cellField[x + 1, y]);
                    }
                    else
                    {
                        cellField[x + 1, y].IsVisited = true;
                    }
                }

                if (x - 1 >= 0 && !cellField[x - 1, y].IsVisited)
                {
                    if (!cellField[x - 1, y].IsBorder && !TempList.Contains(cellField[x - 1, y]))
                    {
                        cellField[x - 1, y].IsVisited = true;
                        TempList.Add(cellField[x - 1, y]);
                    }
                    else
                    {
                        cellField[x - 1, y].IsVisited = true;
                    }
                }

                if (y + 1 < Height && !cellField[x, y + 1].IsVisited)
                {
                    if (!cellField[x, y + 1].IsBorder && !TempList.Contains(cellField[x, y + 1]))
                    {
                        cellField[x, y + 1].IsVisited = true;
                        TempList.Add(cellField[x, y + 1]);
                    }
                    else
                    {
                        cellField[x, y + 1].IsVisited = true;
                    }
                }

                if (y - 1 >= 0 && !cellField[x, y - 1].IsVisited)
                {
                    if (!cellField[x, y - 1].IsBorder && !TempList.Contains(cellField[x, y - 1]))
                    {
                        cellField[x, y - 1].IsVisited = true;
                        TempList.Add(cellField[x, y - 1]);
                    }
                    else
                    {
                        cellField[x, y - 1].IsVisited = true;
                    }
                }

                if (x + 1 < Width && y + 1 < Height && !cellField[x + 1, y + 1].IsVisited)
                {
                    if (!cellField[x + 1, y + 1].IsBorder && !TempList.Contains(cellField[x + 1, y + 1]))
                    {
                        cellField[x + 1, y + 1].IsVisited = true;
                        TempList.Add(cellField[x + 1, y + 1]);
                    }
                    else
                    {
                        cellField[x + 1, y + 1].IsVisited = true;
                    }
                }
                if (x + 1 < Width && y - 1 >= 0 && !cellField[x + 1, y - 1].IsVisited)
                {
                    if (!cellField[x + 1, y - 1].IsBorder && !TempList.Contains(cellField[x + 1, y - 1]))
                    {
                        cellField[x + 1, y - 1].IsVisited = true;
                        TempList.Add(cellField[x + 1, y - 1]);
                    }
                    else
                    {
                        cellField[x + 1, y - 1].IsVisited = true;
                    }
                }
                if (x - 1 >= 0 && y - 1 >= 0 && !cellField[x - 1, y - 1].IsVisited)
                {
                    if (!cellField[x - 1, y - 1].IsBorder && !TempList.Contains(cellField[x - 1, y - 1]))
                    {
                        cellField[x - 1, y - 1].IsVisited = true;
                        TempList.Add(cellField[x - 1, y - 1]);
                    }
                    else
                    {
                        cellField[x - 1, y - 1].IsVisited = true;
                    }
                }
                if (x - 1 >= 0 && y + 1 < Height && !cellField[x - 1, y + 1].IsVisited)
                {
                    if (!cellField[x - 1, y + 1].IsBorder && !TempList.Contains(cellField[x - 1, y + 1]))
                    {
                        cellField[x - 1, y + 1].IsVisited = true;
                        TempList.Add(cellField[x - 1, y + 1]);
                    }
                    else
                    {
                        cellField[x - 1, y + 1].IsVisited = true;
                    }
                }

                TempList.Remove(TempList[0]);
            }
            #endregion
        }

        public bool GoodMove(int x, int y)
        {
            if (cellField[x, y].IsMined)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        /// <summary>
        /// Ппростановка/снятие флажка
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        public void RightButtonClick(int x, int y) 
        {
            if (!cellField[x, y].IsVisited && !cellField[x, y].IsMarked)
            {
                if (flagsCount > 0)
                {
                    cellField[x, y].IsMarked = true;
                    MarkEvent(x, y);
                    flagsCount--;
                    FlagsCountChange(flagsCount);
                    if (cellField[x, y].IsMined)
                    {
                        foundMinesCount++;
                    }
                }
            }
            else if (cellField[x, y].IsMarked) 
            {
                cellField[x, y].IsMarked = false;
                UnmarkEvent(x, y);
                flagsCount++;
                FlagsCountChange(flagsCount);
                if (cellField[x, y].IsMined)
                {
                    foundMinesCount--;
                }
            }

            Victory();
        }

        /// <summary>
        /// Клик левой кнопкой по клетке
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        public void LeftButtonClick(int x, int y)
        {
            if (!cellField[x, y].IsMarked)
            {
                if (!GoodMove(x, y))
                {
                    OpenMineEvent(x, y);
                }
                else
                {
                    ClearAlgorithm(x, y);
                    foreach (Cell cell in cellField)
                    {
                        if (cell.IsBorder && cell.IsVisited)
                        {
                            OpenBorderEvent(cell.PositionX, cell.PositionY, cell.MinesNear);
                        }
                        else if (!cell.IsBorder && cell.IsVisited)
                        {
                            OpenEmptyEvent(cell.PositionX, cell.PositionY);
                        }
                    }
                }
            }
        }

        public void Victory()
        {
            if (foundMinesCount == mineCount)
            {
                VictoryEvent();
                IsRecord(); 
            }
        }

        /// <summary>
        /// Обновление данных таймера
        /// </summary>
        public int TimerCount 
        {
            get 
            {
                return ++timerCount;
            }
        }

        /// <summary>
        /// Установка рекорда
        /// </summary>
        private void IsRecord()
        {
            List<RecordItem> temp = Records.LoadRecordsList();
            if (temp == null || temp.Count == 0)
            {
                NewRecordEvent(Difficulty.ToString(), timerCount.ToString(), DateTime.Now.ToShortDateString());
            }
            else 
            {
                var previousRecord = (from record in temp where record.Difficulty == Difficulty select record).ToList();
                if (previousRecord.Count != 0 && int.Parse(previousRecord[0].TotalSeconds) > timerCount)
                {
                    NewRecordEvent(Difficulty.ToString(), timerCount.ToString(), DateTime.Now.ToShortDateString());
                }
                else if (previousRecord.Count == 0) 
                {
                    NewRecordEvent(Difficulty.ToString(), timerCount.ToString(), DateTime.Now.ToShortDateString());
                }
            }
        }
    }
}
