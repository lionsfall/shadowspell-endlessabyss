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

        internal Vector3 direction;

        private Player player;
        private Joystick joystick;

        private void Start()
        {
            player = GetComponent<Player>();
            joystick = FindFirstObjectByType<Joystick>();
        }

        private void Update()
        {
            if (joystick.Horizontal != 0 || joystick.Vertical != 0)
            {
                player.State = Entity.EntityState.Run;

                direction = new Vector3(joystick.Horizontal, 0, joystick.Vertical);

                player.rb.MovePosition(player.rb.position + direction.normalized * player.Speed * speedMultiplier * Time.deltaTime);
                if(player.targetCreature.GetCreature(player))
                {
                    player.transform.DOLookAt(player.targetCreature.GetCreature(player).transform.position, rotationSpeed);
                }
                else
                {
                    player.transform.DOLookAt(player.transform.position + direction, rotationSpeed);
                }
                // Look at direction
            }
            else
            {
                player.State = Entity.EntityState.Idle;

                player.rb.linearVelocity = Vector3.zero;
                player.rb.angularVelocity = Vector3.zero;
            }
        }
    }
}