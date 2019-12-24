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

        public Vector2 GetSize() {
            if ((_height == 0 || _width == 0 )&& _prefab != null) {
                return new Vector2(_prefab.rect.width, _prefab.rect.height);
            }

            return new Vector2(_width, _height);
        }
    }
}
