using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Saper.Model
{
    class FileRecords : RecordsSource
    {
        public readonly string RecordsPath;

        public FileRecords(string path) 
        {
            RecordsPath = path;
        }

        /// <summary>
        /// Обновить рекорд
        /// </summary>
        /// <param name="record"></param>
        public override void UpdateRecords(List<string> record)
        {
            using (StreamWriter stw = new StreamWriter(RecordsPath))
            {
                foreach (string str in record) 
                {
                    stw.WriteLine(str);
                }
            }
        }

        /// <summary>
        /// Получить рекорды
        /// </summary>
        /// <returns></returns>
        public override IEnumerable<string> GetRecords()
        {
            if (File.Exists(RecordsPath))
            {
                using (StreamReader str = new StreamReader(RecordsPath))
                {
                    string line;
                    while ((line = str.ReadLine()) != null)
                    {
                        yield return line;
                    }
                }
            }
        }
    }
}
