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
        public float speedMultiplier = 1f;
        public float rotationSpeed = 10f;
        [Header("Joystick Settings")]
        public string movementJoystickTag = "MovementJoystick";
        public string attackJoystickTag = "AttackJoystick";

        internal Vector3 direction;

        private Player player;
        private Joystick movementJoystick, attackJoystick;

        private void Start()
        {
            player = GetComponent<Player>();
            movementJoystick = FindObjectsByType<Joystick>(FindObjectsSortMode.None).FirstOrDefault(j => j.gameObject.tag == movementJoystickTag);
            attackJoystick = FindObjectsByType<Joystick>(FindObjectsSortMode.None).FirstOrDefault(j => j.gameObject.tag == attackJoystickTag);
        }

        private void Update()
        {
            MovementUpdate();
            AttackUpdate();
        }

        private void MovementUpdate()
        {
            direction = new Vector3(movementJoystick.Horizontal, 0, movementJoystick.Vertical);

            player.rb.MovePosition(player.rb.position + direction.normalized * player.Speed * speedMultiplier * Time.deltaTime);

            //player.transform.DOLookAt(player.transform.position + direction, rotationSpeed);

            if (movementJoystick.Horizontal != 0 || movementJoystick.Vertical != 0)
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
        public void AttackUpdate()
        {
            player.attackDirection = new Vector3(attackJoystick.Horizontal, 0, attackJoystick.Vertical);
        }
    }
}