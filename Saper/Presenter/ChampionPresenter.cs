using Saper.Interfaces;
using Saper.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Saper.Presenter
{
    class ChampionPresenter
    {
        IChampionForm _IChampionForm;
        string Difficulty;
        string TimerCount;
        string Date;

        public ChampionPresenter(IChampionForm _IChampionForm, string difficulty, string timerCount, string date) 
        {
            Difficulty = difficulty;
            TimerCount = timerCount;
            Date = date;
            this._IChampionForm = _IChampionForm;
            _IChampionForm.OKClicked += _IChampionForm_OKClicked;
        }

        private void _IChampionForm_OKClicked(string name)
        {
            List<RecordItem> temp = Records.LoadRecordsList();
            if (temp == null || temp.Count == 0)
            {
                temp.Add(new RecordItem() { Difficulty = (Level)Enum.Parse(typeof(Level), Difficulty), Date = Date, Name = name, TotalSeconds = TimerCount });
                Records.UpdateRecords(temp);
            }
            else
            {
                var previousRecord = (from record in temp where record.Difficulty == (Level)Enum.Parse(typeof(Level), Difficulty) select record).ToList();
                if (previousRecord.Count != 0 && int.Parse(previousRecord[0].TotalSeconds) > int.Parse(TimerCount))
                {
                    temp.Remove(previousRecord[0]);
                    temp.Add(new RecordItem() { Difficulty = (Level)Enum.Parse(typeof(Level), Difficulty), Date = Date, Name = name, TotalSeconds = TimerCount });
                    Records.UpdateRecords(temp);
                }
                else
                {
                    temp.Add(new RecordItem() { Difficulty = (Level)Enum.Parse(typeof(Level), Difficulty), Date = Date, Name = name, TotalSeconds = TimerCount });
                    Records.UpdateRecords(temp);
                }
            }

            _IChampionForm.OKClicked -= _IChampionForm_OKClicked;
        }
    }
}
