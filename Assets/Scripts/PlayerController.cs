using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace OliverGame
{

    public class PlayerController : MonoBehaviour
    {
        [SerializeField] private Camera _Camera;
        [SerializeField] private float _Speed;
        [SerializeField] private float _Sensititvity;
        

        private const float MAX_VERTICAL_ROTATIONE = 90.0f;
        private const float MIN_VERTICAL_ROTATIONE = -90.0f;

        private float verticalMovement = 0.0f;

        private void Awake()
        {
            Time.timeScale = 1;
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }

        private void Update()
        {
            Move();
            Rotate();
        }

        private void Move()
        {
            if (Input.GetKey(KeyCode.W))
            {
                transform.Translate(Vector3.forward * _Speed * Time.deltaTime);
            }
            if (Input.GetKey(KeyCode.A))
            {
                transform.Translate(Vector3.left * _Speed * Time.deltaTime);
            }
            if (Input.GetKey(KeyCode.D))
            {
                transform.Translate(Vector3.right * _Speed * Time.deltaTime);
            }
            if (Input.GetKey(KeyCode.S))
            {
                transform.Translate(Vector3.back * _Speed * Time.deltaTime);
            }

            
        }

        private void Rotate()
        {
            float horizontalMovement = _Sensititvity * Input.GetAxis("Mouse X");
            verticalMovement += _Sensititvity * -Input.GetAxis("Mouse Y");

            verticalMovement = Mathf.Clamp(verticalMovement, MIN_VERTICAL_ROTATIONE, MAX_VERTICAL_ROTATIONE);

            transform.transform.Rotate(Vector3.up * horizontalMovement);

            _Camera.transform.localRotation = Quaternion.Euler(Vector3.right * verticalMovement);
        }
    }
}
