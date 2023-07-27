using UnityEngine;
using UnityEngine.InputSystem;

namespace Game.Hotfix
{
    public class CharacterLogic : MonoBehaviour
    {
        public float speed = 3.5f;
        public float turnSpeed = 10.0f;
        PallyInput inputAction;
        private CharacterController controller;
        private Vector3 dir;


        protected  void OnInit(object userData)
        {

            inputAction = new PallyInput();
            inputAction.Enable();
            inputAction.Character.Move.started += OnMoveStarted;
            inputAction.Character.Move.performed += OnMovePerformed;
            inputAction.Character.Move.canceled += OnMoveCancel;

            controller = GetComponent<CharacterController>();
        }

        protected void OnUpdate(float elapseSeconds, float realElapseSeconds)
        {
            Vector3 moveDirection = Camera.main.transform.TransformDirection(dir);
            moveDirection.y = 0;
            moveDirection.Normalize();

            controller.SimpleMove(dir * speed);

            if (moveDirection.magnitude != 0)
            {
                Quaternion rotation = Quaternion.LookRotation(moveDirection);
                transform.rotation = Quaternion.Slerp(transform.rotation, rotation, turnSpeed * Time.deltaTime);
            }
        }

        protected void OnHide(bool isShutdown, object userData)
        {
        }

        void OnMoveStarted(InputAction.CallbackContext context)
        {
            
        }

        void OnMovePerformed(InputAction.CallbackContext context)
        {
            //计算角色移动位置
            Vector2 readVector = context.ReadValue<Vector2>();
            dir = new Vector3(readVector.x, 0, readVector.y);
        }

        void OnMoveCancel(InputAction.CallbackContext context)
        {
            dir = Vector3.zero;
        }
        private Vector3 IsoVectorConvert(Vector3 vector)
        {
            Quaternion rotation = Quaternion.Euler(0, 45.0f, 0);
            Matrix4x4 isoMatrix = Matrix4x4.Rotate(rotation);
            return isoMatrix.MultiplyPoint3x4(vector);
        }
    }
}
