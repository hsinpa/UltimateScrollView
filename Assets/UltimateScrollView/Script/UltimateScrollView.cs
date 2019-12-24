//Create and maintain by Hsinpa

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Hsinpa.Ultimate.Scrollview
{
    [RequireComponent(typeof(ScrollRect))]
    public class UltimateScrollView : MonoBehaviour
    {
        #region Inspector Parameter
        [SerializeField]
        private float  horizontal_space;

        [SerializeField]
        private float vertical_space;

        [SerializeField]
        private UltimateSlotHolder statHolder;

        [SerializeField, Range(0,3)]
        private int retainObjectNum = 2;
        #endregion

        #region Private Parameter
        private List<UltimateSlotObject> _slotObjectList;
        private List<UltimateSlotStat> _slotStatList;

        private int _slotListLength;
        private float _scrollViewHeight;

        private int currentIndex = 0;
        private float currentIndexMax = 0;
        private float currentIndexMin = 0;

        private RectTransform _scrollContent;
        private Vector2 visibleSize;

        private ScrollRect _scrollRect;
        private UltimatePooling ultimatePooling;
        #endregion

        #region Public API
        public void Setup() {
            _scrollRect = this.GetComponent<ScrollRect>();
            _slotObjectList = new List<UltimateSlotObject>();
            _slotStatList = new List<UltimateSlotStat>();

            if (_scrollRect.viewport != null) {
                //Clear up
                Utility.UtilityMethod.ClearChildObject(_scrollRect.content);

                _scrollContent = _scrollRect.viewport;
                visibleSize = new Vector2(_scrollContent.rect.width, _scrollContent.rect.height);
            }
            
            ultimatePooling = new UltimatePooling(statHolder, _scrollRect.content);
        }

        public void AppendObject(UltimateSlotStat ultObject) {

            UltimateSlotObject slotObject = ultimatePooling.GetObject(ultObject._id);
            if (slotObject != null) {
                RectTransform slotTransform = slotObject.GetComponent<RectTransform>();
                Vector2 slotSize = slotObject.slotStat.GetSize();
                Debug.Log("slotObject " + slotObject.slotStat.GetSize() + ", " + slotTransform.sizeDelta);

                slotTransform.anchoredPosition = new Vector2(0, - (_scrollViewHeight + (slotSize.y *0.5f) ));

                _scrollViewHeight += slotSize.y;

                _slotStatList.Add(ultObject);
                _slotObjectList.Add(slotObject);

                _slotListLength++;

            }
        }

        public void InsertObject(UltimateSlotStat ultObject, int index) {
            _slotListLength++;
        }

        public void RemoveObject(int index) {
            _slotListLength--;
        }
        #endregion

        #region Private API
          

        #endregion

        #region Monobehavior

        void Update()
        {
            if (this.gameObject.activeInHierarchy && _slotObjectList != null) {
                Debug.Log("currentIndex " + currentIndex);
                float viewYPos = _scrollRect.content.anchoredPosition.y;
                if (currentIndex >= 0 && currentIndex < _slotListLength) {
                    currentIndexMin =- (_slotObjectList[currentIndex].rectTransform.anchoredPosition.y + _slotStatList[currentIndex].GetSize().y * 0.5f);
                    currentIndexMax =- (_slotObjectList[currentIndex].rectTransform.anchoredPosition.y - _slotStatList[currentIndex].GetSize().y * 0.5f);

                    if (viewYPos > currentIndexMax && currentIndex + 1 < _slotListLength) {
                        currentIndex++;
                    }

                    if (viewYPos < currentIndexMin && currentIndex - 1 >= 0) {
                        currentIndex--;
                    }


                }
            }
        }
        #endregion

    }
}