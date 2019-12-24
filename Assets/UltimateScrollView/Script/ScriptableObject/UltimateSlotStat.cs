using UnityEngine;

namespace Hsinpa.Ultimate.Scrollview {

    [CreateAssetMenu(fileName = "UltimateSlotStat", menuName = "Tools/UtimateScrollview/SlotStat", order = 1)]
    public class UltimateSlotStat : ScriptableObject
    {
        public string _id;

        public RectTransform _prefab;

        [SerializeField]
        private float _height;

        [SerializeField]
        private float _width;

        private Vector2 Size;

        public Vector2 GetSize() {
            if ((_height == 0 || _width == 0 )&& _prefab != null) {
                Size.x = _prefab.rect.width;
                Size.y = _prefab.rect.height;

                return Size;
            }

            Size.x = _width;
            Size.y = _width;

            return Size;
        }
    }
}
