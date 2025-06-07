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
                if (MasterDictionary.ContainsKey(zodis.Key))
                {
                    MasterDictionary[zodis.Key] += zodis.Value;
                }
                else
                {
                    MasterDictionary[zodis.Key] = zodis.Value;
                }
            }
        }
        public void IjungtiDictionaryJoiner(BlockingCollection<Dictionary<string, int>> PriiemimoEile, BlockingCollection<Dictionary<string, int>> AtnaujinimoEile)
        {
            MasterDictionary = new Dictionary<string, int>();
            foreach (Dictionary<string, int> Dictionary in PriiemimoEile.GetConsumingEnumerable())
            {
                JoinDictionaries(Dictionary);
                AtnaujinimoEile.Add(MasterDictionary);
            }
        }
    }
}
