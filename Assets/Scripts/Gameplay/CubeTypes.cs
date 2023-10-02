using System.Collections.Generic;
using UnityEngine;

namespace Gameplay {
    [CreateAssetMenu(fileName = "Assets/CubeTypes/CubeTypes", menuName = "Assets/CubeTypes", order = 0)]
    public class CubeTypes : ScriptableObject {
        [SerializeField]
        List<CubeTypeData> cubeTypes;


        public void UpgradeCubeType(Cube cube) {
            var idx = cubeTypes.FindIndex(el => el.CubeType == cube.CubeType);
            if (idx >= cubeTypes.Count - 1) {
                return; // TODO Add Win State
            }

            var newType = cubeTypes[idx + 1];
            cube.UpdateCubeType(newType.CubeType);
            cube.UpdateMaterial(newType.Material);
        }
    }
}