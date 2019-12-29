using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Hsinpa.Ultimate.Scrollview;

namespace Hsinpa.Ultimate.Scrollview.Sample {
    public class SimpleScrollTestSet : MonoBehaviour
    {
        [SerializeField]
        private TestSetStats[] testset;

        [SerializeField]
        private UltimateSlotStat insertSlot;

        [SerializeField]
        UltimateScrollView utimateScrollView;

        // Start is called before the first frame update
        void Start()
        {
            utimateScrollView.Setup();

            var organizedSet = GetTestSet(testset);

            if (utimateScrollView != null && organizedSet != null) {
                foreach (UltimateSlotStat slot in organizedSet) {
                    utimateScrollView.AppendObject(slot);
                }
            }
        }

        private List<UltimateSlotStat> GetTestSet(TestSetStats[] p_testSet) {
            List<UltimateSlotStat> organizedSet = new List<UltimateSlotStat>();

            if (testset != null) {
                foreach (TestSetStats set in p_testSet) {
                    for (int i = 0; i < set.length; i++)
                    {
                        organizedSet.Add(set.slot);
                    }
                }
            }
            return organizedSet;
        }

        public void Insert() {
            utimateScrollView.InsertObject(insertSlot, 0);
        }

        public void Delete()
        {
            utimateScrollView.RemoveObject(0);
        }

        [System.Serializable]
        private struct TestSetStats {
            public UltimateSlotStat slot;
            public int length;
        }


    }
}
