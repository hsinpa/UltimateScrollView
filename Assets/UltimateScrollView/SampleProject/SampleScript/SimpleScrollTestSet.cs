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

        private List<FormedTestSet> completeSet;

        // Start is called before the first frame update
        void Start()
        {
            utimateScrollView.Setup();

            var organizedSet = GetTestSet(testset);
            completeSet = new List<FormedTestSet>();

            if (utimateScrollView != null && organizedSet != null) {
                int length = organizedSet.Count;
                for (int i = 0; i < length; i++) {
                    FormedTestSet formSet = new FormedTestSet();
                    formSet.custom_id = "id_" + i;
                    formSet.slot = organizedSet[i];
                    completeSet.Add(formSet);
                    utimateScrollView.AppendObject(organizedSet[i], "id_"+i);
                }
            }

            utimateScrollView.OnSlotCreateEvent += OnSlotCreateEvent;
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

        private void OnSlotCreateEvent(UltimateSlot slot) {

            var findSet = completeSet.Find(x => x.custom_id == slot.custom_id);
            if (findSet.custom_id != null && slot.slotStat._id == "type_1") {
                CustomSlotObject customObject = slot.slotObject.GetComponent<CustomSlotObject>();
                customObject.SetText(findSet.custom_id);
                customObject.SetButtonEvent(() => {
                    utimateScrollView.RemoveObject(slot.index);
                });
            }
        }

        [System.Serializable]
        private struct TestSetStats {
            public UltimateSlotStat slot;
            public int length;
        }

        private struct FormedTestSet
        {
            public UltimateSlotStat slot;
            public string custom_id;
        }


    }
}
