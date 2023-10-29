using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public CharacterController controller;

    public float speed = 6f;
    public float gravity = -9.81f;
    public float jumpForce = 1f;

    public Transform groundCheck;
    public float groundDistance = 0.4f;
    public LayerMask groundMask;

    private Vector3 velocity;

    public float crouchedHeight = 0.5f;
    public float standingHeight = 2f;

    
    private bool isGrounded;

    private PlayerState currentState;

    private interface PlayerState
    {
        void UpdateState();
    }

    private class NormalState : PlayerState
    {
        private PlayerMovement player;

        public NormalState(PlayerMovement player)
        {
            this.player = player;
        }

        public void UpdateState()
        {
            player.isGrounded = Physics.CheckSphere(player.groundCheck.position, player.groundDistance, player.groundMask);

            if (player.isGrounded && player.velocity.y < 0)
            {
                player.velocity.y = -2f;
            }

            float x = Input.GetAxis("Horizontal");
            float z = Input.GetAxis("Vertical");

            Vector3 move = player.transform.right * x + player.transform.forward * z;

            player.controller.Move(move * player.speed * Time.deltaTime);

            if (Input.GetButtonDown("Jump") && player.isGrounded)
            {
                player.velocity.y = Mathf.Sqrt(player.jumpForce * -2f * player.gravity);
            }

            player.velocity.y += player.gravity * Time.deltaTime;

            player.controller.Move(player.velocity * Time.deltaTime);

            if (Input.GetKey(KeyCode.LeftShift))
            {
                player.currentState = new RunningState(player);
            }

            if (Input.GetKeyDown(KeyCode.C))
            {
                player.currentState = new CrouchingState(player);
            }
        }
    }

    private class RunningState : PlayerState
    {
        private PlayerMovement player;

        public RunningState(PlayerMovement player)
        {
            this.player = player;
        }

        public void UpdateState()
        {
            player.isGrounded = Physics.CheckSphere(player.groundCheck.position, player.groundDistance, player.groundMask);

            if (player.isGrounded && player.velocity.y < 0)
            {
                player.velocity.y = -2f;
            }

            float x = Input.GetAxis("Horizontal");
            float z = Input.GetAxis("Vertical");

            Vector3 move = player.transform.right * x + player.transform.forward * z;

            player.controller.Move(move * player.speed * 2 * Time.deltaTime); // Double speed when running

            if (!Input.GetKey(KeyCode.LeftShift))
            {
                player.currentState = new NormalState(player);
            }

            if (Input.GetButtonDown("Jump") && player.isGrounded)
            {
                player.velocity.y = Mathf.Sqrt(player.jumpForce * -2f * player.gravity);
            }

            player.velocity.y += player.gravity * Time.deltaTime;
            player.controller.Move(player.velocity * Time.deltaTime);
        }
    }

    private class CrouchingState : PlayerState
    {
        private PlayerMovement player;
        private float originalControllerHeight;

        public CrouchingState(PlayerMovement player)
        {
            this.player = player;
        }

        public void UpdateState()
        {
            player.isGrounded = Physics.CheckSphere(player.groundCheck.position, player.groundDistance, player.groundMask);
            originalControllerHeight = player.controller.height;

            if (player.isGrounded && player.velocity.y < 0)
            {
                player.velocity.y = -2f;
            }

            float x = Input.GetAxis("Horizontal");
            float z = Input.GetAxis("Vertical");

            Vector3 move = player.transform.right * x + player.transform.forward * z;


            // 1/4 speed when crouching
            player.controller.Move(move * player.speed * 0.25f * Time.deltaTime);


            // Reducing height to mimic Crouching and the updating height to normal
            if (Input.GetKeyDown(KeyCode.C))
            {
                // Toggle crouching
                if (player.controller.height == originalControllerHeight)
                {
                    player.controller.height = 0.5f;
                }
                else
                {
                    player.controller.height = originalControllerHeight;
                }
            }

            // Cannot jump while crouching
            /*
            if (Input.GetButtonDown("Jump") && player.isGrounded)
            {
                player.velocity.y = Mathf.Sqrt(player.jumpForce * -2f * player.gravity);
            }

            player.velocity.y += player.gravity * Time.deltaTime;
            player.controller.Move(player.velocity * Time.deltaTime);
            */
        }
    }



    private void Start()
    {
        currentState = new NormalState(this);
    }

    private void Update()
    {
        currentState.UpdateState();
    }
}