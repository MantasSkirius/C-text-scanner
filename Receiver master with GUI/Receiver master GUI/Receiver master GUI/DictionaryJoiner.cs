using Newtonsoft.Json;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Receiver_master_GUI
{

    internal class DictionaryJoiner
    {
        public Dictionary<string, int> MasterDictionary;
        
        private void JoinDictionaries(Dictionary<string, int> AdditionalDict)
        {
            //Prie MasterDictionary Prideda visus AdditionalDict žodžius.
            foreach (KeyValuePair<string, int> zodis in AdditionalDict)
            {
                string tikrinamasZodis = zodis.Key.ToLower();
                if (MasterDictionary.ContainsKey(zodis.Key))
                {
                    MasterDictionary[tikrinamasZodis] += zodis.Value;
                }
                else
                {
                    MasterDictionary[tikrinamasZodis] = zodis.Value;
                }
            }
        }

        private List<KeyValuePair<string, int>> SortMasterDictionaryByValue()
        {
            //Privalau pakeistį į masyvą, kad galėčiau surikiuoti pagal dažnį.
            List<KeyValuePair<string, int>> rikiuotiDazniai = MasterDictionary.ToList();
            rikiuotiDazniai = rikiuotiDazniai.OrderByDescending(daznis => daznis.Value).ToList();
            return rikiuotiDazniai;
        }
        public void IjungtiDictionaryJoiner(BlockingCollection<Dictionary<string, int>> PriiemimoEile, BlockingCollection<List<KeyValuePair<string, int>>> AtnaujinimoEile)
        {
            MasterDictionary = new Dictionary<string, int>();
            foreach (Dictionary<string, int> Dictionary in PriiemimoEile.GetConsumingEnumerable())
            {
                JoinDictionaries(Dictionary);
                AtnaujinimoEile.Add(SortMasterDictionaryByValue());
            }
        }
    }
}
