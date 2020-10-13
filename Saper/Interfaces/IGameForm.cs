using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Saper.Interfaces
{
    interface IGameForm
    {
        event Action<int, int> LeftClickEvent;
        event Action<int, int> RightClickEvent;
        event Action<int, int> StartCustomGameEvent;
        event Action<Level> StartGameEvent;
        event Action TimerTick;
        event Action AgainClicked;

        void CreateField(int x, int y);
        void BlockField();
        void Boom(int x, int y);
        void Mark(int x, int y);
        void UnMark(int x, int y);
        void OpenBorder(int x, int y, int minesCount);
        void OpenEmpty(int x, int y);
        void Victory();
        void UpgradeTimer(int count);
        void UpgradeFlagsCounter(int count);
        void AddNewRecord(string difficulty, string timerCount, string date);
    }
}
