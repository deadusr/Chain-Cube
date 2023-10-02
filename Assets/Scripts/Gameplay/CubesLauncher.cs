using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Utils;

namespace Gameplay {
    public class CubesLauncher : Singleton<CubesLauncher> {

        [SerializeField]
        Transform cubePlaceholder;

        [SerializeField]
        Cube cubeToLaunchPrefab;

        [SerializeField]
        Camera mainCamera;

        Cube launchedCube;
        Rigidbody cubeRigidbody;
        Transform cubeTransform;

        bool isDragging;
        bool isLaunched;

        Vector3 launchCubeLastVelocity;

        void Awake() {
            GenerateCube();
        }
        void OnEnable() {
            InputController.Instance.click += OnClick;
            InputController.Instance.clickEnd += OnClickEnd;
        }

        void Update() {
            if (isDragging) {
                var ray = mainCamera.ScreenPointToRay(InputController.Instance.MousePosition);
                if (Physics.Raycast(ray, out var hit)) {
                    var newPosition = new Vector3(hit.point.x, cubeTransform.position.y, hit.point.z);
                    var vectorFromOrigin = newPosition - cubePlaceholder.position;
                    var distanceFromOrigin = vectorFromOrigin.magnitude;
                    if (distanceFromOrigin > 1)
                        newPosition = cubePlaceholder.position + vectorFromOrigin.normalized; // Cap it to only distance of 1 unit

                    cubeTransform.position = newPosition;
                }


            }
        }

        void FixedUpdate() {
            if (isLaunched) {
                if (launchCubeLastVelocity != Vector3.zero && (!cubeRigidbody || cubeRigidbody.velocity == Vector3.zero)) {
                    EndLaunch();
                }
                else {
                    launchCubeLastVelocity = cubeRigidbody.velocity;
                }
            }
        }

        void GenerateCube() {
            var placeholderTf = cubePlaceholder.transform;
            launchedCube = Instantiate(cubeToLaunchPrefab, placeholderTf.position, placeholderTf.rotation);
            cubeRigidbody = launchedCube.GetComponent<Rigidbody>();
            cubeTransform = launchedCube.transform;
        }


        void OnClick(Vector2 screenPosition) {
            if (!Camera.main || isLaunched)
                return;

            float maxDistance = 100f;
            var ray = mainCamera.ScreenPointToRay(screenPosition);
            if (Physics.Raycast(ray, out RaycastHit hit, maxDistance)) {
                if (hit.transform.GetInstanceID() == cubeTransform.GetInstanceID()) {
                    StartDrag();
                    return;
                }
            }

            if (isDragging)
                EndDrag();
        }

        void OnClickEnd() {
            if (isDragging)
                EndDrag();
        }

        void StartDrag() {
            isDragging = true;
            cubePlaceholder.gameObject.SetActive(true);
        }

        void EndDrag() {
            isDragging = false;
            cubePlaceholder.gameObject.SetActive(false);
            LaunchCube();
        }

        void LaunchCube() {
            Vector3 vecToOrigin = cubePlaceholder.transform.position - cubeTransform.position;
            Vector3 direction = vecToOrigin.normalized;
            float forceMultiplier = 10f;
            float maxForce = 30f;
            float force = vecToOrigin.magnitude * forceMultiplier;
            force = Mathf.Clamp(force, 0f, maxForce);

            cubeRigidbody.AddForce(direction * force, ForceMode.Impulse);
            isLaunched = true;
        }

        void EndLaunch() {
            isLaunched = false;
            launchCubeLastVelocity = Vector3.zero;
            GenerateCube();
        }
    }

}