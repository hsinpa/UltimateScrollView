//Create and maintain by Hsinpa

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
namespace Hsinpa.Ultimate.Scrollview
{
    [RequireComponent(typeof(ScrollRect))]
    public class UltimateScrollView : MonoBehaviour, IBeginDragHandler, IEndDragHandler
    {
        #region Inspector Parameter
        [SerializeField]
        private float  horizontal_space;

        [SerializeField]
        private float vertical_space;

        [SerializeField]
        private UltimateSlotHolder statHolder;

        [SerializeField, Range(2,20)]
        private int retainObjectNum = 2;

        [SerializeField, Range(0, 3)]
        private int appendObjectNum = 2;

        #endregion

        #region Private Parameter
        private List<UltimateSlot> _slotList;
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

        private bool isDragging;
        #endregion

        #region Public API
        public void Setup() {
            _scrollRect = this.GetComponent<ScrollRect>();
            _slotStatList = new List<UltimateSlotStat>();
            _slotList = new List<UltimateSlot>();

            if (_scrollRect.viewport != null) {
                //Clear up
                Utility.UtilityMethod.ClearChildObject(_scrollRect.content);

                _scrollContent = _scrollRect.viewport;
                visibleSize = new Vector2(_scrollContent.rect.width, _scrollContent.rect.height);
            }
            
            ultimatePooling = new UltimatePooling(statHolder, _scrollRect.content);
        }

        public void AppendObject(UltimateSlotStat ultObject) {
                //RectTransform slotTransform = slotObject.GetComponent<RectTransform>();
                Vector2 slotSize = ultObject.GetSize();
                //Debug.Log("slotObject " + slotObject.slotStat.GetSize() + ", " + slotTransform.sizeDelta);

                //slotTransform.anchoredPosition = new Vector2(0, - (_scrollViewHeight + (slotSize.y *0.5f) ));
                var ultiSlot = new UltimateSlot(ultObject);
                ultiSlot.SetPosition(new Vector2(0, -(_scrollViewHeight + (slotSize.y * 0.5f))));

                _scrollViewHeight += slotSize.y;

                _slotStatList.Add(ultObject);

                _slotList.Add(ultiSlot);

                _slotListLength++;
            
        }

        public void InsertObject(UltimateSlotStat ultObject, int index) {
            _slotListLength++;
        }

        public void RemoveObject(int index) {
            _slotListLength--;
        }
        #endregion

        #region Private API
        private void DisableObject(UltimateSlot slot) {
            if (slot.isEnable) {
                ultimatePooling.RemoveObject(slot.slotObject);
                slot.DisableObj();
            }
        }

        private void UpdateElementVisibility() {
            for (int i = 0; i < _slotListLength; i++) {
                var slot = _slotList[i];

                if (currentIndex - i < retainObjectNum) {

                    //
                    if (currentIndex - appendObjectNum > i) {

                        DisableObject(slot);
                        continue;
                    }

                    if (i > currentIndex + retainObjectNum + appendObjectNum) {
                        DisableObject(slot);
                        continue;
                    }
                }

                if (!slot.isEnable)
                {
                    UltimateSlotObject createObj = ultimatePooling.GetObject(slot.slotStat._id);
                    slot.SetObject(createObj);
                    createObj.Enable(true);

                    slot.slotObject.rectTransform.anchoredPosition = slot.Position;
                }
                else {
                    slot.slotObject.Enable(true);
                    slot.slotObject.rectTransform.anchoredPosition = slot.Position;
                }
            }
        }

        private void UpdateLayoutPos() {
            if (isDragging) return;

            float viewYPos = _scrollRect.content.anchoredPosition.y;

            if (viewYPos < 0 ) {
                _scrollRect.content.anchoredPosition = new Vector2(0, Mathf.Lerp(viewYPos, 0, 0.1f));

                bool withInLimit = (viewYPos > -0.1);
                float targetPos = 0;

                _scrollRect.content.anchoredPosition = new Vector2(0, (withInLimit) ? targetPos : Mathf.Lerp(viewYPos, targetPos, 0.1f));
                return;
            }

            float viewportSize = _scrollRect.viewport.rect.height;
            if (_scrollViewHeight - viewYPos < viewportSize) {
                bool withInLimit = (_scrollViewHeight - viewportSize - viewYPos > 0.1);
                float targetPos = _scrollViewHeight - viewportSize;

                _scrollRect.content.anchoredPosition = new Vector2(0, (withInLimit) ? targetPos : Mathf.Lerp(viewYPos, targetPos, 0.1f));

                return;
            }

        }

        #endregion

        #region Monobehavior

        void Update()
        {
            if (this.gameObject.activeInHierarchy && _slotList != null) {
                float viewYPos = _scrollRect.content.anchoredPosition.y;
                if (currentIndex >= 0 && currentIndex < _slotListLength) {

                    currentIndexMin = - (_slotList[currentIndex].Position.y + _slotList[currentIndex].slotStat.GetSize().y * 0.5f);
                    currentIndexMax =- (_slotList[currentIndex].Position.y - _slotList[currentIndex].slotStat.GetSize().y * 0.5f);

                    if (viewYPos > currentIndexMax && currentIndex + 1 < _slotListLength) {
                        currentIndex++;
                        //Debug.Log("currentIndex " + currentIndex);
                    }

                    if (viewYPos < currentIndexMin && currentIndex - 1 >= 0) {
                        currentIndex--;
                        //Debug.Log("currentIndex " + currentIndex);
                    }

                    UpdateElementVisibility();
                    UpdateLayoutPos();
                }
            }
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            isDragging = false;
        }

        public void OnBeginDrag(PointerEventData eventData)
        {
            isDragging = true;
        }
        #endregion

    }
}