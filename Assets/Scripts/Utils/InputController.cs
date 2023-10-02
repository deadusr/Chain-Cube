using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

namespace Utils {
    public class InputController : Singleton<InputController> {
        public Action<Vector2> click;
        public Action clickEnd;

        InputActions inputActions;

        public Vector2 MousePosition => inputActions.Gameplay.Point.ReadValue<Vector2>();

        void Start() {
            Initialize();
        }

        void Initialize() {
            inputActions = new InputActions();
            inputActions.Gameplay.Enable();
            inputActions.Gameplay.Click.performed += OnClick;
            inputActions.Gameplay.Click.canceled += OnCLickEnd;
        }

        void OnClick(InputAction.CallbackContext context) {

            Vector2 currentPosition = inputActions.Gameplay.Point.ReadValue<Vector2>();
            click?.Invoke(currentPosition);
        }

        void OnCLickEnd(InputAction.CallbackContext context) {
            clickEnd?.Invoke();
        }
    }

}