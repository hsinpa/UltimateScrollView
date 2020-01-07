using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Hsinpa.Ultimate.Scrollview.Utility;

namespace Hsinpa.Ultimate.Scrollview {
    public class TopDownOrientation : BaseOrientation
    {
        public float GetElementMainPos(int index, float latestHeight, Vector2 slotSize, float space)
        {
            float slotHeight = UtilityMethod.GetAxisValue(slotSize, UltimateScrollView.Direction.TopDown);

            float positionValue = (latestHeight - (slotHeight * 0.5f));
            if (index > 0)
                positionValue -= space;

            return positionValue;
        }

        public Vector2 GetElementPosVector(float mainPosValue, Vector2 slotSize)
        {
            return new Vector2(0, mainPosValue);
        }


        public float GetStartPosition(float contentSize, float size)
        {
            return 0;
        }


        public float GetMaxWidthHeightValue(UltimateSlot slot)
        {
            float positionValue = UtilityMethod.GetAxisValue(slot.Position, UltimateScrollView.Direction.TopDown),
                  sizeValue = UtilityMethod.GetAxisValue(slot.slotStat.GetSize(), UltimateScrollView.Direction.TopDown);

            return  (positionValue - (sizeValue * 0.5f));
        }

        public float GetIndexMax(float slotPosValue, float slotSizeValue)
        {
            return -(slotPosValue - slotSizeValue * 0.5f);
        }

        public float GetIndexMin(float slotPosValue, float slotSizeValue)
        {
            return -(slotPosValue + slotSizeValue * 0.5f);
        }

        public Vector2 AlignTopTargetPos(float scrollViewLength, float viewportSize, float viewTrackPos)
        {
            bool withInLimit = (Mathf.Pow(viewTrackPos, 2) < 0.1);
            float targetPos = 0;

            return UtilityMethod.GetDirectionVector((withInLimit) ? targetPos : Mathf.Lerp(viewTrackPos, targetPos, 0.1f), 
                0, UltimateScrollView.Direction.TopDown);
        }

        public Vector2 AlignBottomTargetPos(float scrollViewLength, float viewportSize, float viewTrackPos)
        {
            bool withInLimit = (scrollViewLength - viewportSize - viewTrackPos > 0.1);
            float targetPos = scrollViewLength - viewportSize;

            return UtilityMethod.GetDirectionVector(
                (withInLimit) ? targetPos : Mathf.Lerp(viewTrackPos, targetPos, 0.1f), 
                0, UltimateScrollView.Direction.TopDown);
        }

        public bool AlignTopValidation(float viewTrackPos, float scrollViewLength, float viewportSize)
        {
            return viewTrackPos < 0 || (scrollViewLength < viewportSize && viewTrackPos > 0);
        }

        public bool AlignBottomValidation(float viewTrackPos, float scrollViewLength, float viewportSize)
        {
            return scrollViewLength - viewTrackPos < viewportSize && scrollViewLength > viewportSize;
        }


    }
}