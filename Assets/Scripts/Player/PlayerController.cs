using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [Header("Movement")]
    public float moveSpeed;
    public float jumpPower;
    private Vector2 curMovementInput;  
    public LayerMask groundLayerMask;  
    
     [Header("Look")]
     public Transform cameraContainer;
     public float minXLook;  
     public float maxXLook; 
     private float camCurXRot;
     public float lookSensitivity; 
     private Vector2 mouseDelta;  
     public bool canLook = true;

     public Action inventory;
     private Rigidbody _rigidbody;
     // 마우스 변화값
    // public bool canLook = true;
    //
    // public Action inventory;
    // private Rigidbody rigidbody;
    //
    // // [HideInInspector]
    // // public bool canLook = true;
    // //
    // // 
    // //
    // //

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();
    }

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }

    void FixedUpdate()
    {
        Move();
    }

    private void LateUpdate()
    {
        if (canLook)
        {
            CameraLook();
        }
    }

    void Move()
    {
        Vector3 dir = transform.forward * curMovementInput.y + transform.right * curMovementInput.x;
        dir *= moveSpeed;  
        dir.y = _rigidbody.velocity.y;  

        _rigidbody.velocity = dir;  
    }
    
    void CameraLook()
    {
        camCurXRot += mouseDelta.y * lookSensitivity;
        camCurXRot = Mathf.Clamp(camCurXRot, minXLook, maxXLook);
        cameraContainer.localEulerAngles = new Vector3(-camCurXRot, 0, 0);

        transform.eulerAngles += new Vector3(0, mouseDelta.x * lookSensitivity, 0);
    }

    public void OnMove(InputAction.CallbackContext context)
     {
         if(context.phase == InputActionPhase.Performed)
         {
             curMovementInput = context.ReadValue<Vector2>();
         }
         else if(context.phase == InputActionPhase.Canceled)
         {
             curMovementInput = Vector2.zero;
         }
     }

    public void OnLook(InputAction.CallbackContext context)
    {
        mouseDelta = context.ReadValue<Vector2>();
    }
    
    public void OnJump(InputAction.CallbackContext context)
     {
         if(context.phase == InputActionPhase.Started && IsGrounded())
         {
             _rigidbody.AddForce(Vector2.up * jumpPower, ForceMode.Impulse);
         }
     }
    
    bool IsGrounded()
     {
         Ray[] rays = new Ray[4]
         {
             new Ray(transform.position + (transform.forward * 0.2f) + (transform.up * 0.01f), Vector3.down),
             new Ray(transform.position + (-transform.forward * 0.2f) + (transform.up * 0.01f), Vector3.down),
             new Ray(transform.position + (transform.right * 0.2f) + (transform.up * 0.01f), Vector3.down),
             new Ray(transform.position + (-transform.right * 0.2f) + (transform.up * 0.01f), Vector3.down)
         };
         
         for(int i = 0; i < rays.Length; i++)
         {
             if (Physics.Raycast(rays[i], 0.1f, groundLayerMask))
             {
                return true;
             }
         }
         return false;
     }
    
    public void OnInventory(InputAction.CallbackContext context)
    {
         if (context.phase == InputActionPhase.Started) 
         {
             inventory?.Invoke();
             ToggleCursor();
         }
    }
    
    void ToggleCursor()
    {
         bool toggle = Cursor.lockState == CursorLockMode.Locked;
         Cursor.lockState = toggle ? CursorLockMode.None : CursorLockMode.Locked;
         canLook = !toggle;
    }
		// 물리 연산
//     private void FixedUpdate()
//     {
//         Move();
//     }
//
// 		// 카메라 연산 -> 모든 연산이 끝나고 카메라 움직임
//     private void LateUpdate()
//     {
//         if (canLook)
//         {
//             CameraLook();
//         }
//     }
//     
//     public void OnInventoryButton(InputAction.CallbackContext callbackContext)
//     {
//         if (callbackContext.phase == InputActionPhase.Started)
//         {
//             inventory?.Invoke();
//             ToggleCursor();
//         }
//     }
//     
//     void ToggleCursor()
//     {
//         bool toggle = Cursor.lockState == CursorLockMode.Locked;
//         Cursor.lockState = toggle ? CursorLockMode.None : CursorLockMode.Locked;
//         canLook = !toggle;
//     }
//     
// 		// 입력값 처리
//     public void OnLookInput(InputAction.CallbackContext context)
//     {
//         mouseDelta = context.ReadValue<Vector2>();
//     }
//
// 		// 입력값 처리
//     public void OnMoveInput(InputAction.CallbackContext context)
//     {
//         if(context.phase == InputActionPhase.Performed)
//         {
//             curMovementInput = context.ReadValue<Vector2>();
//         }
//         else if(context.phase == InputActionPhase.Canceled)
//         {
//             curMovementInput = Vector2.zero;
//         }
//     }
//
//     public void OnJumpInput(InputAction.CallbackContext context)
//     {
//         if(context.phase == InputActionPhase.Started && IsGrounded())
//         {
//             rigidbody.AddForce(Vector2.up * jumpPower, ForceMode.Impulse);
//         }
//     }
//
//     private void Move()
//     {
// 		    // 현재 입력의 y 값은 z 축(forward, 앞뒤)에 곱한다.
// 		    // 현재 입력의 x 값은 x 축(right, 좌우)에 곱한다.
//         Vector3 dir = transform.forward * curMovementInput.y + transform.right * curMovementInput.x;
//         dir *= moveSpeed;  // 방향에 속력을 곱해준다.
//         dir.y = rigidbody.velocity.y;  // y값은 velocity(변화량)의 y 값을 넣어준다.
//
//         rigidbody.velocity = dir;  // 연산된 속도를 velocity(변화량)에 넣어준다.
//     }
//
//     void CameraLook()
//     {
// 		    // 마우스 움직임의 변화량(mouseDelta)중 y(위 아래)값에 민감도를 곱한다.
// 		    // 카메라가 위 아래로 회전하려면 rotation의 x 값에 넣어준다. -> 실습으로 확인
//         camCurXRot += mouseDelta.y * lookSensitivity;
//         camCurXRot = Mathf.Clamp(camCurXRot, minXLook, maxXLook);
//         cameraContainer.localEulerAngles = new Vector3(-camCurXRot, 0, 0);
//
// 		    // 마우스 움직임의 변화량(mouseDelta)중 x(좌우)값에 민감도를 곱한다.
// 		    // 카메라가 좌우로 회전하려면 rotation의 y 값에 넣어준다. -> 실습으로 확인
// 		    // 좌우 회전은 플레이어(transform)를 회전시켜준다.
// 		    // Why? 회전시킨 방향을 기준으로 앞뒤좌우 움직여야하니까.
//         transform.eulerAngles += new Vector3(0, mouseDelta.x * lookSensitivity, 0);
//     }
//
//     bool IsGrounded()
//     {
//         Ray[] rays = new Ray[4]
//         {
//             new Ray(transform.position + (transform.forward * 0.2f) + (transform.up * 0.01f), Vector3.down),
//             new Ray(transform.position + (-transform.forward * 0.2f) + (transform.up * 0.01f), Vector3.down),
//             new Ray(transform.position + (transform.right * 0.2f) + (transform.up * 0.01f), Vector3.down),
//             new Ray(transform.position + (-transform.right * 0.2f) + (transform.up * 0.01f), Vector3.down)
//         };
//         
//         for(int i = 0; i < rays.Length; i++)
//         {
//             if (Physics.Raycast(rays[i], 0.1f, groundLayerMask))
//             {
//                 return true;
//             }
//         }
//
//         return false;
//     }
//
//     public void OnInventory(InputAction.CallbackContext context)
//     {
//         if (context.phase == InputActionPhase.Started)
//         {
//             inventory?.Invoke();
//             ToggleCursor();
//         }
//     }
//
//     void ToggleCursor()
//     {
//         bool toggle = Cursor.lockState == CursorLockMode.Locked;
//         Cursor.lockState = toggle ? CursorLockMode.None : CursorLockMode.Locked;
//         canLook = !toggle;
//     }
}
