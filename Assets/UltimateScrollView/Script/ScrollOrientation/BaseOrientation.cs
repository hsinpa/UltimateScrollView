using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Hsinpa.Ultimate.Scrollview
{
    public interface BaseOrientation
    {
        #region Element Position
        float GetElementMainPos(int index, float latestHeight, Vector2 slotSize, float space);
        Vector2 GetElementPosVector(float mainPosValue, Vector2 slotSize);
        #endregion


    }
}