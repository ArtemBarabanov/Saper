using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Saper.Model
{
    class RecordItem
    {
        [DisplayName("Сложность")]
        public Level Difficulty { get; set; }
        [DisplayName("Время, сек")]
        public string TotalSeconds { get; set; }
        [DisplayName("Имя")]
        public string Name { get; set; }
        [DisplayName("Дата")]
        public string Date { get; set; }

        public override string ToString()
        {
            return $"{Difficulty}|{TotalSeconds}|{Name}|{Date}";
        }
    }
}
