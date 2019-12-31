using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Hsinpa.Ultimate.Scrollview.Utility;

namespace Hsinpa.Ultimate.Scrollview {
    public class TopDownOrientation : BaseOrientation
    {
        public float GetElementMainPos(int index, float latestHeight, Vector2 slotSize, float space)
        {
            float positionValue = (latestHeight - (slotSize.y * 0.5f));
            if (index > 0)
                positionValue -= space;

            return positionValue;
        }

        public Vector2 GetElementPosVector(float mainPosValue, Vector2 slotSize)
        {
            return new Vector2(0, mainPosValue);
        }
    }
}