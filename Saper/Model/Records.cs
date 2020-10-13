using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Resources;
using System.Text;
using System.Threading.Tasks;

namespace Saper.Model
{
    class Records
    {
        static readonly Lazy<Records> Instance = new Lazy<Records>(() => new Records(Source));
        static List<RecordItem> RecordItems;
        static RecordsSource Source;

        Records(RecordsSource source) { }

        public static Records GetInstance(RecordsSource source) 
        {
            Source = source;
            return Instance.Value;
        }

        public static List<RecordItem> LoadRecordsList() 
        {
            if (RecordItems == null)
            {
                RecordItems = new List<RecordItem>();
                foreach (string item in Source.GetRecords())
                {
                    string[] temp = item.Split('|');
                    RecordItems.Add(new RecordItem() { Difficulty = (Level)Enum.Parse(typeof(Level), temp[0], false), TotalSeconds = temp[1], Name = temp[2], Date = temp[3] });
                }
                return RecordItems;
            }
            else 
            {
                return RecordItems;
            }
        }

        public static void UpdateRecords(List<RecordItem> items) 
        {
            List<string> temp = new List<string>();
            foreach (RecordItem item in items) 
            {
                temp.Add(item.ToString());
            }
            Source.UpdateRecords(temp);
        }
    }
}
