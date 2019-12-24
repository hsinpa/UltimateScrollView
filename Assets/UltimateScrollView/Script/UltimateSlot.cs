using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Hsinpa.Ultimate.Scrollview
{
    public class UltimateSlot
    {
        public UltimateSlotObject slotObject { get { return _slotObject; } }
        private UltimateSlotObject _slotObject;

        public UltimateSlotStat slotStat { get { return _slotStat; } }
        private UltimateSlotStat _slotStat;

        public Vector2 Position { get { return _Position; } }
        private Vector2 _Position;

        public bool isEnable
        {
            get { return _slotObject != null; }
        }

        public UltimateSlot(UltimateSlotStat slotStat)
        {
            this._slotStat = slotStat;
        }

        public void SetPosition(Vector2 pos)
        {
            _Position = pos;
        }

        public void SetObject(UltimateSlotObject slotObject)
        {
            this._slotObject = slotObject;
        }

        public void DisableObj() {
            if (_slotObject != null) {
                _slotObject.Enable(false);
                _slotObject = null;
            }
        }

    }
}