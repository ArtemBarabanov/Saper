using Saper.Interfaces;
using Saper.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Saper
{
    class GamePresenter
    {
        GameSession game;
        IGameForm view;

        public GamePresenter(IGameForm view)
        {
            this.view = view;
            view.LeftClickEvent += View_LeftClickEvent;
            view.RightClickEvent += View_RightClickEvent;
            view.StartGameEvent += View_LevelEvent;
            view.StartCustomGameEvent += View_StartCustomGameEvent;
            view.TimerTick += View_TimerTick;
            view.AgainClicked += View_AgainClicked;
        }

        private void View_AgainClicked()
        {
            game.RestartGame();
            view.UpgradeTimer(game.TimerCount);
        }

        private void View_TimerTick()
        {
            view.UpgradeTimer(game.TimerCount);
        }

        private void Game_OpenMineEvent(int x, int y)
        {
            view.Boom(x, y);
            view.BlockField();
        }

        private void Game_OpenEmptyEvent(int x, int y)
        {
            view.OpenEmpty(x, y);
        }

        private void Game_OpenBorderEvent(int x, int y, int minesCount)
        {
            view.OpenBorder(x, y, minesCount);
        }

        private void Game_UnmarkEvent(int x, int y)
        {
            view.UnMark(x, y);
        }

        private void Game_MarkEvent(int x, int y)
        {
            view.Mark(x, y);
        }

        private void View_LevelEvent(Level level)
        {
            StartGame(level);
            view.UpgradeTimer(game.TimerCount);
        }

        private void View_StartCustomGameEvent(int width, int height)
        {           
            StartGame(Level.Индивидуальная, width, height);
            view.UpgradeTimer(game.TimerCount);
        }

        private void StartGame(Level difficulty, int width = 0, int height = 0)
        {
            if (game != null)
            {
                Deregister();
            }
            if (difficulty == Level.Индивидуальная)
            {
                game = new GameSession(difficulty, width, height);
            }
            else
            {
                game = new GameSession(difficulty);
            }
            Register();
            game.StartGame();
        }

        private void Game_SessionCreated(int width, int height)
        {
            view.CreateField(width, height);
        }

        private void Game_FlagsCountChange(int count)
        {
            view.UpgradeFlagsCounter(count);
        }

        private void Game_NewRecordEvent(string difficulty, string timerCount, string date)
        {
            view.AddNewRecord(difficulty, timerCount, date);
        }

        private void Register() 
        {
            game.VictoryEvent += Game_VictoryEvent;
            game.MarkEvent += Game_MarkEvent;
            game.UnmarkEvent += Game_UnmarkEvent;
            game.OpenBorderEvent += Game_OpenBorderEvent;
            game.OpenEmptyEvent += Game_OpenEmptyEvent;
            game.OpenMineEvent += Game_OpenMineEvent;
            game.NewRecordEvent += Game_NewRecordEvent;
            game.FlagsCountChange += Game_FlagsCountChange;
            game.SessionCreated += Game_SessionCreated;
        }

        private void Deregister()
        {
            game.VictoryEvent -= Game_VictoryEvent;
            game.MarkEvent -= Game_MarkEvent;
            game.UnmarkEvent -= Game_UnmarkEvent;
            game.OpenBorderEvent -= Game_OpenBorderEvent;
            game.OpenEmptyEvent -= Game_OpenEmptyEvent;
            game.OpenMineEvent -= Game_OpenMineEvent;
            game.NewRecordEvent -= Game_NewRecordEvent;
            game.FlagsCountChange -= Game_FlagsCountChange;
            game.SessionCreated -= Game_SessionCreated;
        }

        private void Game_VictoryEvent()
        {
            view.Victory();
            view.BlockField();
        }

        private void View_RightClickEvent(int x, int y)
        {
            game.RightButtonClick(x, y);
        }

        private void View_LeftClickEvent(int x, int y)
        {
            game.LeftButtonClick(x, y);
        }
    }
}
