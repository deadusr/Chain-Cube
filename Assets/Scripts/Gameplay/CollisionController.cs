using System.Collections.Generic;
using UnityEngine;
using Utils;

namespace Gameplay {

    struct CollisionData {
        public Vector3 velocity;
        public Cube cube;

        public CollisionData(Vector3 velocity, Cube cube) {
            this.velocity = velocity;
            this.cube = cube;
        }
    }

    public class CollisionController : Singleton<CollisionController> {
        [SerializeField]
        CubeTypes cubeTypes;

        Dictionary<int, CollisionData> collisions = new Dictionary<int, CollisionData>();

        public void Collide(Vector3 cubeVelocity, Cube cube, Cube otherCube) {
            if(cube.CubeType != otherCube.CubeType)
                return;

            float magnitude = cubeVelocity.magnitude;
            
            if (collisions.TryGetValue(otherCube.GetInstanceID(), out var existingCollision)) {
                bool isExistingVelocityBigger = existingCollision.velocity.magnitude > magnitude;
                
                var cubeToDestroy = isExistingVelocityBigger ? cube : existingCollision.cube;
                var cubeToUpgrade = isExistingVelocityBigger ? existingCollision.cube : cube;
                
                Destroy(cubeToDestroy.gameObject);
                cubeTypes.UpgradeCubeType(cubeToUpgrade);
                
                cubeToUpgrade.Push(Vector3.up * 2f);
                collisions.Remove(otherCube.GetInstanceID());
            }
            else {
                collisions[cube.GetInstanceID()] = new CollisionData(cubeVelocity, cube);
            }
        }
    }
}