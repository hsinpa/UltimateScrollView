//Create and maintain by Hsinpa

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using Hsinpa.Ultimate.Scrollview.Utility;

namespace Hsinpa.Ultimate.Scrollview
{
    [RequireComponent(typeof(ScrollRect))]
    public class UltimateScrollView : MonoBehaviour, IBeginDragHandler, IEndDragHandler
    {
        #region Inspector Parameter
        [SerializeField]
        private float space;

        [SerializeField]
        private UltimateSlotHolder statHolder;

        [SerializeField, Range(2,50)]
        private int retainObjectNum = 2;

        [SerializeField, Range(0, 3)]
        private int appendObjectNum = 2;

        [SerializeField]
        private Direction directionStat;
        #endregion

        #region Public Event
        public System.Action<UltimateSlot> OnSlotCreateEvent;
        #endregion

        #region Private Parameter
        private List<UltimateSlot> _slotList;

        private int _slotListCount;
        private float _scrollViewLength;

        private int currentIndex = 0;
        private float currentIndexMax = 0;
        private float currentIndexMin = 0;

        private RectTransform _scrollContent;
        private Vector2 visibleSize;

        private ScrollRect _scrollRect;
        private UltimatePooling ultimatePooling;

        private bool isDragging;

        public enum Direction {TopDown, RightLeft }

        private BaseOrientation _baseOrientation;
        #endregion

        #region Public API
        public void Setup() {
            _baseOrientation = GetOrientation(directionStat);
            _scrollRect = this.GetComponent<ScrollRect>();
            _slotList = new List<UltimateSlot>();

            if (_scrollRect.viewport != null) {
                //Clear up
                Utility.UtilityMethod.ClearChildObject(_scrollRect.content);

                _scrollContent = _scrollRect.viewport;
                visibleSize = new Vector2(_scrollContent.rect.width, _scrollContent.rect.height);
                _scrollRect.vertical = directionStat == Direction.TopDown;
                _scrollRect.horizontal = directionStat == Direction.RightLeft;
            }

            ultimatePooling = new UltimatePooling(statHolder, _scrollRect.content);
        }

        public void AppendObject(UltimateSlotStat ultObject, string custom_id = "") {
                var ultiSlot = new UltimateSlot(ultObject, custom_id);

                _slotList.Add(ultiSlot);

                _slotListCount++;

                UpdateElementPos(_slotListCount-1);
        }

        public void InsertObject(UltimateSlotStat ultObject, int index, string custom_id = "") {
            _slotList.Insert(index, new UltimateSlot(ultObject, custom_id));
            _slotListCount++;

            if (index < currentIndex)
                currentIndex++;

            for (int i = index; i < _slotListCount; i++) {
                UpdateElementPos(i);
            }
        }

        public void RemoveObject(int index) {
            DisableObject(_slotList[index]);
            _slotList.RemoveAt(index);
            _slotListCount--;

            if (index < currentIndex)
                currentIndex--;

            for (int i = index; i < _slotListCount; i++)
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
            for (int i = 0; i < _slotListCount; i++) {
                var slot = _slotList[i];

                //if (currentIndex - i < retainObjectNum) {

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
                //}

                if (!slot.isEnable)
                {
                    UltimateSlotObject createObj = ultimatePooling.GetObject(slot.slotStat._id);
                    createObj.rectTransform.sizeDelta = new Vector2(visibleSize.x, createObj.rectTransform.sizeDelta.y);

                    slot.SetObject(createObj);
                    createObj.Enable(true);

                    if (OnSlotCreateEvent != null)
                        OnSlotCreateEvent(slot);

                } else if (!slot.slotObject.isEnable) {
                    slot.slotObject.Enable(true);

                    if (OnSlotCreateEvent != null)
                        OnSlotCreateEvent(slot);
                }

                slot.slotObject.rectTransform.anchoredPosition = slot.Position;
            }
        }

        private void UpdateElementPos(int startIndex) {
            if (startIndex < 0 || startIndex >= _slotListCount) return;

            var ultiSlot = _slotList[startIndex];
            var slotSize = ultiSlot.slotStat.GetSize();

            float slotSizeValue = UtilityMethod.GetAxisValue(slotSize, directionStat);
            float visibleSizeValue = UtilityMethod.GetAxisValue(visibleSize, directionStat);

            float startPosition = _baseOrientation.GetStartPosition(visibleSizeValue, slotSizeValue);

            //float latestHeight = ((startIndex == 0) ? 0 : _slotList[startIndex-1].GetBorderPosition(directionStat));

            float latestHeight = (startIndex == 0) ? startPosition : _baseOrientation.GetMaxWidthHeightValue(_slotList[startIndex - 1]);

            float positionValue = _baseOrientation.GetElementMainPos(startIndex, latestHeight, slotSize, this.space);
            //float positionValue = (latestHeight - (slotSizeValue * 0.5f));
            //if (startIndex > 0)
            //    positionValue -= space;

            //ultiSlot.SetPosition(UtilityMethod.GetDirectionVector(positionValue, 0, directionStat));
            ultiSlot.SetPosition(_baseOrientation.GetElementPosVector(positionValue, slotSize));
            ultiSlot.SetIndex(startIndex);

            _scrollViewLength = Mathf.Abs(latestHeight - slotSizeValue - startPosition);
        }

        private void UpdateViewportPos() {
            if (isDragging) return;

            float viewTrackPos = UtilityMethod.GetAxisValue(_scrollRect.content.anchoredPosition, directionStat);
            float viewportSize = UtilityMethod.GetRectValue(_scrollRect.viewport.rect, directionStat);

            //Debug.Log("_scrollViewLength " + _scrollViewLength);
            //Debug.Log("viewTrackPos " + viewTrackPos);
            //Debug.Log("viewportSize " + viewportSize);

            if (viewTrackPos < 0 || (_scrollViewLength < viewportSize && viewTrackPos > 0)) {
                bool withInLimit = (Mathf.Pow(viewTrackPos, 2) < 0.1);
                //float targetPos = 0;

                //_scrollRect.content.anchoredPosition = UtilityMethod.GetDirectionVector(
                //    (withInLimit) ? targetPos : Mathf.Lerp(viewTrackPos, targetPos, 0.1f),
                //    0, 
                //    directionStat);

                _scrollRect.content.anchoredPosition = _baseOrientation.AlignTopTargetPos(withInLimit, _scrollViewLength, viewportSize, viewTrackPos);
                return;
            }

            if (_scrollViewLength - viewTrackPos < viewportSize && _scrollViewLength > viewportSize) {
                bool withInLimit = (_scrollViewLength - viewportSize - viewTrackPos > 0.1);
                //float targetPos = _scrollViewLength - viewportSize;

                //_scrollRect.content.anchoredPosition = UtilityMethod.GetDirectionVector(
                //    (withInLimit) ? targetPos : Mathf.Lerp(viewTrackPos, targetPos, 0.1f),
                //    0,
                //    directionStat);

                _scrollRect.content.anchoredPosition = _baseOrientation.AlignBottomTargetPos(withInLimit, _scrollViewLength, viewportSize, viewTrackPos);
                return;
            }
        }

        private BaseOrientation GetOrientation(Direction p_direction)
        {
            Dictionary<Direction, BaseOrientation> orientationTable = new Dictionary<Direction, BaseOrientation>() {
                { Direction.TopDown, new TopDownOrientation() },
                { Direction.RightLeft, new LeftRightOrientation() }
            };

            if (orientationTable.TryGetValue(p_direction, out BaseOrientation targetOrientation)) {
                return targetOrientation;
            }

            return orientationTable[Direction.TopDown];
        }

        #endregion

        #region Monobehavior

        void Update()
        {
            if (this.gameObject.activeInHierarchy && _slotList != null) {
                float viewNavPoint = UtilityMethod.GetAxisValue(_scrollRect.content.anchoredPosition, directionStat);

                if (currentIndex >= 0 && currentIndex < _slotListCount) {
                    float slotPosValue = UtilityMethod.GetAxisValue(_slotList[currentIndex].Position, directionStat);
                    float slotSizeValue = UtilityMethod.GetAxisValue(_slotList[currentIndex].slotStat.GetSize(), directionStat);

                    //currentIndexMin = - (slotPosValue + slotSizeValue * 0.5f);
                    //currentIndexMax =- (slotPosValue - slotSizeValue * 0.5f);

                    currentIndexMin = _baseOrientation.GetIndexMin(slotPosValue, slotSizeValue);
                    currentIndexMax = _baseOrientation.GetIndexMax(slotPosValue, slotSizeValue);

                    if (viewNavPoint > currentIndexMax && currentIndex + 1 < _slotListCount) {
                        currentIndex++;
                        //Debug.Log("currentIndex " + currentIndex);
                    }

                    if (viewNavPoint < currentIndexMin && currentIndex - 1 >= 0) {
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