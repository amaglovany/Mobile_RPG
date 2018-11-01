using UnityEngine;

namespace TDC.InputSystem
{
    public class InputManager : MonoBehaviourSingleton<InputManager>
    {
        #region Data

        public delegate void OnStartHold();

        public event OnStartHold startHold;

        public delegate void OnDelayHold();

        public event OnDelayHold delayHold;

        public delegate void OnEndHold();

        public event OnEndHold endHold;

        public delegate void OnSwap();

        public event OnSwap swap;

        public Vector2 touchPosition { get; private set; }
        public Vector2 updatePosition { get; private set; }
        public float deltaX { get; private set; }
        private bool stateSwap;

        #endregion

        #region Unity

        private void Start()
        {
            startHold += StartSwap;
            delayHold += DelaySwap;
            endHold += EndSwap;
        }

        private void Update()
        {
            StartHold();
            DelayHold();
            EndHold();
        }

        #endregion

        #region Core

        private void StartHold()
        {
            foreach (Touch touches in Input.touches)
                if (touches.phase == TouchPhase.Began)
                {
                    startHold();
                    return;
                }

            if (Input.GetKeyDown(KeyCode.Space))
            {
                startHold();
            }
        }

        private void DelayHold()
        {
            foreach (Touch touches in Input.touches)
                if (touches.phase != TouchPhase.Ended && touches.phase != TouchPhase.Canceled)
                {
                    delayHold();
                    return;
                }

            if (Input.GetKey(KeyCode.Space))
            {
                delayHold();
            }
        }

        private void EndHold()
        {
            foreach (Touch touches in Input.touches)
                if (touches.phase == TouchPhase.Ended || touches.phase == TouchPhase.Canceled)
                {
                    endHold();
                    return;
                }

            if (Input.GetKeyUp(KeyCode.Space))
            {
                endHold();
            }
        }

        #region Swap

        private void StartSwap()
        {
            foreach (Touch touches in Input.touches)
            {
                touchPosition = touches.position;
                return;
            }
        }

        private void DelaySwap()
        {
            if (stateSwap)
            {
                return;
            }

            foreach (Touch touches in Input.touches)
            {
                deltaX = touches.deltaPosition.magnitude / Time.deltaTime / 100;
                updatePosition = -(touches.position - touchPosition);
            }

            if (deltaX > 10)
            {
                swap();
                EndSwap();
                stateSwap = true;
            }
        }

        private void EndSwap()
        {
            touchPosition = new Vector2();
            updatePosition = new Vector2();
            deltaX = 0;
            stateSwap = false;
        }

        #endregion

        #endregion
    }
}