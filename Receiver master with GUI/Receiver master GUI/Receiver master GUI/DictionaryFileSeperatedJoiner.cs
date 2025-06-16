using Newtonsoft.Json;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Receiver_master_GUI
{
    internal class DictionaryFileSeperatedJoiner : DictionaryJoiner
    {
        public List<Tuple<string, KeyValuePair<string, int>>> MasterList;
         protected void JoinDictionaries(Dictionary<string, int> AdditionalDict)
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

        private void JoinLists(string fileName, List<KeyValuePair<string, int>> AdditionalList)
        {
            foreach(KeyValuePair<string, int> daznis in AdditionalList)
            {
                MasterList.Add(new Tuple<string, KeyValuePair<string, int>>(fileName, new KeyValuePair<string, int>(daznis.Key, daznis.Value)));
            }
        }

        public void IjungtiDictionaryJoinerSuFailuVardais(BlockingCollection<Tuple<string, Dictionary<string, int>>> PriiemimoEile2, BlockingCollection<List<Tuple<string, KeyValuePair<string,int>>>> AtnaujinimoEile2)
        {
            MasterList = new List<Tuple<string, KeyValuePair<string, int>>>();
            foreach (Tuple<string, Dictionary<string, int>> Dictionary in PriiemimoEile2.GetConsumingEnumerable())
            {
                List<KeyValuePair<string, int>> RikiuotasList = SortDictionaryByValue(Dictionary.Item2);
                JoinLists(Dictionary.Item1, RikiuotasList);
                AtnaujinimoEile2.Add(MasterList);

            }
        }

    }
}
