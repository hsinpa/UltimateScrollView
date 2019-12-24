using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Hsinpa.Ultimate.Scrollview
{
    [RequireComponent(typeof(CanvasGroup))]
    public class UltimateSlotObject : MonoBehaviour
    {
        private UltimateSlotStat _slotStat;
        public UltimateSlotStat slotStat { get { return _slotStat; } }

        private CanvasGroup _canvasGroup;
        public CanvasGroup canvasGroup { get { return _canvasGroup; } }

        private RectTransform _rectTransform;
        public RectTransform rectTransform { get { return _rectTransform; } }


        public bool isEnable {
            get { return _canvasGroup.alpha == 1; }
        }

        public void Enable(bool enable)
        {
            canvasGroup.alpha = (enable) ? 1 : 0;
            canvasGroup.blocksRaycasts = enable;
            canvasGroup.interactable = enable;
        }

        public void SetUp(UltimateSlotStat slotStat)
        {
            this._slotStat = slotStat;
            this._rectTransform = GetComponent<RectTransform>();

            if (_canvasGroup == null)
                _canvasGroup = GetComponent<CanvasGroup>();
        }

    }
}