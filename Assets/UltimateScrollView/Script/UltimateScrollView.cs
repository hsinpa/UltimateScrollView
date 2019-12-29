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

                var ultiSlot = new UltimateSlot(ultObject);
                //ultiSlot.SetPosition(new Vector2(0, -(_scrollViewHeight + (slotSize.y * 0.5f))));
                //Debug.Log("_scrollViewHeight " + _scrollViewHeight);


                //_scrollViewHeight += slotSize.y;

                _slotList.Add(ultiSlot);

                _slotListLength++;

                UpdateElementPos(_slotListLength-1);
        }

        public void InsertObject(UltimateSlotStat ultObject, int index) {
            _slotList.Insert(index, new UltimateSlot(ultObject));
            _slotListLength++;

            for (int i = index; i < _slotListLength; i++) {
                UpdateElementPos(i);
            }
        }

        public void RemoveObject(int index) {
            _slotList.RemoveAt(index);
            _slotListLength--;

            for (int i = index; i < _slotListLength; i++)
            {
                UpdateElementPos(i);
            }
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

                    //Max Top
                    if (currentIndex - appendObjectNum > i) {

                        DisableObject(slot);
                        continue;
                    }

                    //Max Bottom
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
                }
                else {
                    slot.slotObject.Enable(true);
                }

                slot.slotObject.rectTransform.anchoredPosition = slot.Position;

            }
        }

        private void UpdateElementPos(int startIndex) {
            if (startIndex < 0 || startIndex >= _slotListLength) return;

            var ultiSlot = _slotList[startIndex];
            var slotSize = ultiSlot.slotStat.GetSize();
            float latestHeight = ((startIndex == 0) ? 0 : _slotList[startIndex-1].GetBorderPosition());

            ultiSlot.SetPosition(new Vector2(0, (latestHeight - (slotSize.y * 0.5f))));

            _scrollViewHeight = Mathf.Abs(latestHeight - slotSize.y);
            Debug.Log("_scrollViewHeight " + _scrollViewHeight);

        }

        private void UpdateViewportPos() {
            if (isDragging) return;

            float viewYPos = _scrollRect.content.anchoredPosition.y;
            float viewportSize = _scrollRect.viewport.rect.height;

            if (viewYPos < 0 || (_scrollViewHeight < viewportSize && viewYPos > 0)) {
                //_scrollRect.content.anchoredPosition = new Vector2(0, Mathf.Lerp(viewYPos, 0, 0.1f));

                bool withInLimit = (Mathf.Pow(viewYPos, 2) < 0.1);
                float targetPos = 0;

                _scrollRect.content.anchoredPosition = new Vector2(0, (withInLimit) ? targetPos : Mathf.Lerp(viewYPos, targetPos, 0.1f));
                return;
            }

            if (_scrollViewHeight - viewYPos < viewportSize && _scrollViewHeight > viewportSize) {
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
                    UpdateViewportPos();
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