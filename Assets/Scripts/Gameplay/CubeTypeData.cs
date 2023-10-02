using UnityEngine;

namespace Gameplay {
    
    public enum DefaultCubeType {
        Red,
        Green,
        Blue
    }
    
    [CreateAssetMenu(fileName = "Assets/CubeTypes/CubeType", menuName = "Assets/CubeType", order = 0)]
    public class CubeTypeData : ScriptableObject {
        [SerializeField]
        string cubeType;

        [SerializeField]
        Material material;

        public string CubeType => cubeType;
        public Material Material => material;
    }
}