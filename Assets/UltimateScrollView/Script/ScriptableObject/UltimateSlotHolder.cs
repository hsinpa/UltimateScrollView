using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Hsinpa.Ultimate.Scrollview
{

    [CreateAssetMenu(fileName = "UltimateSlotHolder", menuName = "Tools/UtimateScrollview/SlotHolder", order = 2)]
    public class UltimateSlotHolder : ScriptableObject
    {
        [SerializeField]
        private List<UltimateSlotStat> statList;

        public UltimateSlotStat FindObject(string id)
        {
            return statList.Find(x => x._id == id);
        }

    }
}
