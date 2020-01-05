using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Hsinpa.Ultimate.Scrollview.Utility;

namespace Hsinpa.Ultimate.Scrollview
{

    public class LeftRightOrientation : BaseOrientation
    {
        public float GetElementMainPos(int index, float latestHeight, Vector2 slotSize, float space)
        {
            Debug.Log("latestHeight " + latestHeight);

            float slotWidth = UtilityMethod.GetAxisValue(slotSize, UltimateScrollView.Direction.RightLeft);
            float positionValue = (latestHeight - (slotWidth * 0.5f));
            if (index > 0)
                positionValue -= space;

            return positionValue;
        }

        public Vector2 GetElementPosVector(float mainPosValue, Vector2 slotSize)
        {
            return new Vector2(mainPosValue,-slotSize.y * 0.5f);
        }

        public float GetStartPosition(float contentSize, float size) {
            return (contentSize * 0.5f) ;
        }

        public float GetMaxWidthHeightValue(UltimateSlot slot)
        {
            float positionValue = UtilityMethod.GetAxisValue(slot.Position, UltimateScrollView.Direction.RightLeft),
                  sizeValue = UtilityMethod.GetAxisValue(slot.slotStat.GetSize(), UltimateScrollView.Direction.RightLeft);

            return(positionValue - (sizeValue * 0.5f));
        }

        public float GetIndexMax(float slotPosValue, float slotSizeValue)
        {
            return -(slotPosValue - slotSizeValue * 0.5f);
        }

        public float GetIndexMin(float slotPosValue, float slotSizeValue)
        {
            return -(slotPosValue + slotSizeValue * 0.5f);
        }

        public Vector2 AlignTopTargetPos(bool withInLimit, float scrollViewLength, float viewportSize, float viewTrackPos)
        {
            float targetPos = 0;

            return UtilityMethod.GetDirectionVector((withInLimit) ? targetPos : Mathf.Lerp(viewTrackPos, targetPos, 0.1f),
                0, UltimateScrollView.Direction.RightLeft);
        }

        public Vector2 AlignBottomTargetPos(bool withInLimit, float scrollViewLength, float viewportSize, float viewTrackPos)
        {
            float targetPos = scrollViewLength - viewportSize;
            return UtilityMethod.GetDirectionVector(
                (withInLimit) ? targetPos : Mathf.Lerp(viewTrackPos, targetPos, 0.1f),
                0, UltimateScrollView.Direction.RightLeft);
        }
    }
}