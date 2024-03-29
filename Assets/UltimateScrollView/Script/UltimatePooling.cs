﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Hsinpa.Ultimate.Scrollview.Utility;

namespace Hsinpa.Ultimate.Scrollview
{
    public class UltimatePooling
    {

        private Dictionary<string, List<UltimateSlotObject>> _pooling;
        private UltimateSlotHolder _slotHolder;
        private Transform _parentTransform;


        public UltimatePooling(UltimateSlotHolder slotHolder, Transform parentTransform) {
            _pooling = new Dictionary<string, List<UltimateSlotObject>>();
            _slotHolder = slotHolder;
            _parentTransform = parentTransform;
        }

        public UltimateSlotObject GetObject(string id) {
            if (_pooling.TryGetValue(id, out List<UltimateSlotObject> objectList))
            {
                int length = objectList.Count;

                for (int i = 0; i < length; i++)
                {
                    if (!objectList[i].isEnable)
                    {
                        return objectList[i];
                    }
                }

                return CreateObject(id);

            }
            else {
                return CreateObject(id);
            }
        }

        public void RemoveObject(UltimateSlotObject p_object) {
            p_object.Enable(false);
        }

        public void Release() {
            _pooling.Clear();
        }

        private UltimateSlotObject CreateObject(string id) {
            if (_parentTransform != null && _slotHolder != null) {
                var slotStat = _slotHolder.FindObject(id);
                if (slotStat != null) {
                    var slot = UtilityMethod.CreateObjectToParent(_parentTransform, slotStat._prefab.gameObject).GetComponent<UltimateSlotObject>();

                    slot.SetUp();
                    slot.rectTransform.sizeDelta = slotStat.GetSize();
                    PushToPool(id, slot);
                    return slot;
                }
            }

            return null;
        }

        private void PushToPool(string id, UltimateSlotObject slotObject) {
            if (_pooling.ContainsKey(id))
                _pooling[id].Add(slotObject);
            else {
                _pooling.Add(id, new List<UltimateSlotObject>() { slotObject });
            }
        }


    }
}