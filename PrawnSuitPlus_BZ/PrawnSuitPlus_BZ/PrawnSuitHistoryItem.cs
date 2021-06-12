using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PrawnSuitPlus_BZ
{
    /// <summary>
    /// This class allows us to keep track of Prawn Suits that we've modded so we can dynamically
    /// change the modifier in real time.
    /// </summary>
    /// 
    internal class PrawnSuitHistoryItem
    {
        public Exosuit PrawnSuitInstance;
        public ModType ModType;
        public float OriginalValue;

        public PrawnSuitHistoryItem(Exosuit prawnSuitInstance, ModType modType, float originalValue)
        {
            PrawnSuitInstance = prawnSuitInstance;
            OriginalValue = originalValue;
            ModType = modType;
        }
    }
}
