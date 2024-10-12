using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Dogabeey
{
    [RequireComponent(typeof(Player))]
    public class PlayerJoystickController : MonoBehaviour
    {
        public InputSystem_Actions inputActions;
        public float speedMultiplier = 1f;
        public float rotationSpeed = 10f;
        [Header("Joystick Settings")]
        public string movementJoystickTag = "MovementJoystick";
        public string attackJoystickTag = "AttackJoystick";

        internal Vector3 direction;

        private Player player;
        private Joystick movementJoystick, attackJoystick;
        private Vector2 inputLeftAxis, inputRightAxis;

        private void OnEnable()
        {
            inputActions.Player.Move.performed += Move_performed;
            inputActions.Player.Look.performed += Look_performed;

        }
        private void OnDisable()
        {
            inputActions.Player.Move.performed -= Move_performed;
            inputActions.Player.Look.performed -= Look_performed;
        }

        private void Look_performed(UnityEngine.InputSystem.InputAction.CallbackContext obj)
        {
            inputRightAxis = obj.ReadValue<Vector2>();
        }

        private void Awake()
        {
            inputActions = new InputSystem_Actions();
            inputActions.Enable();
        }


        private void Move_performed(UnityEngine.InputSystem.InputAction.CallbackContext obj)
        {
            inputLeftAxis = obj.ReadValue<Vector2>();
        }


        private void Start()
        {
            player = GetComponent<Player>();
            movementJoystick = FindObjectsByType<Joystick>(FindObjectsSortMode.None).FirstOrDefault(j => j.gameObject.tag == movementJoystickTag);
            attackJoystick = FindObjectsByType<Joystick>(FindObjectsSortMode.None).FirstOrDefault(j => j.gameObject.tag == attackJoystickTag);
        }

        private void Update()
        {
            ControlUpdate();
            MovementUpdate();
            DirectionUpdate();
        }

        private void ControlUpdate()
        {
            if(movementJoystick.hasInput)
            {
                direction = new Vector3(movementJoystick.Horizontal, 0, movementJoystick.Vertical);
            }
            else if(inputLeftAxis.magnitude > 0.01f)
            {
                // Use new input system
                direction = new Vector3(inputLeftAxis.x, 0, inputLeftAxis.y);
            }
            else
            {
                direction = Vector3.zero;
            }

            if(attackJoystick.hasInput)
            {
                player.attackDirection = new Vector3(attackJoystick.Horizontal, 0, attackJoystick.Vertical);
            }
            else if(inputRightAxis.magnitude > 0.01f)
            {
                // Use new input system
                player.attackDirection = new Vector3(inputRightAxis.x, 0, inputRightAxis.y);
            }
            else
            {
                player.attackDirection = Vector3.zero;
            }
        }
        private void MovementUpdate()
        {
            player.rb.MovePosition(player.rb.position + direction.normalized * player.Speed * speedMultiplier * Time.deltaTime);

            //player.transform.DOLookAt(player.transform.position + direction, rotationSpeed);

            if (direction.magnitude > 0.01f)
            {
                player.State = Entity.EntityState.Run;

            }
            else
            {
                player.State = Entity.EntityState.Idle;

                player.rb.linearVelocity = Vector3.zero;
                player.rb.angularVelocity = Vector3.zero;
            }
        }
        private void DirectionUpdate()
        {
            if(direction.magnitude > 0.01f)
            {
                player.transform.DOLookAt(player.transform.position + direction, rotationSpeed);
            }
            else if(player.attackDirection.magnitude > 0.01f)
            {
                player.transform.DOLookAt(player.transform.position + player.attackDirection, rotationSpeed);
            }
        }
    }
}