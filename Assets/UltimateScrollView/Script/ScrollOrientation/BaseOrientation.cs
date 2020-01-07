using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Hsinpa.Ultimate.Scrollview
{
    public interface BaseOrientation
    {
        #region Element Position
        float GetStartPosition(float contentSize, float size);

        float GetElementMainPos(int index, float latestHeight, Vector2 slotSize, float space);
        Vector2 GetElementPosVector(float mainPosValue, Vector2 slotSize);
        #endregion

        #region SlotObject Function
        float GetMaxWidthHeightValue(UltimateSlot slot);
        #endregion

        #region Current Slot Index
        float GetIndexMin(float slotPosValue, float slotSizeValue);
        float GetIndexMax(float slotPosValue, float slotSizeValue);
        #endregion

        #region UpdateViewportPos
        Vector2 AlignTopTargetPos(float scrollViewLength, float viewportSize, float viewTrackPos);
        Vector2 AlignBottomTargetPos(float scrollViewLength, float viewportSize, float viewTrackPos);

        bool AlignTopValidation(float viewTrackPos, float scrollViewLength, float viewportSize);
        bool AlignBottomValidation(float viewTrackPos, float scrollViewLength, float viewportSize);

        #endregion
    }
}