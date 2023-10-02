using System;
using UnityEngine;
using UnityEngine.Serialization;

namespace Gameplay {

    public class Cube : MonoBehaviour {

        [SerializeField]
        DefaultCubeType defaultCubeType;

        MeshRenderer meshRenderer;
        Rigidbody rg;

        string cubeType;

        public string CubeType => cubeType;

        void Awake() {
            meshRenderer = GetComponent<MeshRenderer>();
            rg = GetComponent<Rigidbody>();

            cubeType = defaultCubeType.ToString();
        }

        void OnCollisionEnter(Collision other) {
            if (other.gameObject.TryGetComponent<Cube>(out var otherCube)) {
                CollisionController.Instance.Collide(rg.velocity, this, otherCube);
            }
        }

        public void Push(Vector3 vector) {
            rg.AddForce(vector, ForceMode.Impulse);
        }


        public void UpdateCubeType(string newCubeType) {
            cubeType = newCubeType;
        }

        public void UpdateMaterial(Material material) {
            meshRenderer.material = material;
        }
    }
}