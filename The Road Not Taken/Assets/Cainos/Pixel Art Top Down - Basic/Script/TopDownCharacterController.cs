using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Cainos.PixelArtTopDown_Basic
{
    public class TopDownCharacterController : MonoBehaviour
    {
        public float speed;

        private Animator animator;

        private void Start()
        {
            animator = GetComponent<Animator>();
        }


        private void Update()
        {
            // int dir =  Random.Range(0, 4);
            // if (dir ==)
            // {

            // }
            Vector2 dir = Vector2.zero; // Default direction for the AI bot

            int randomDir = Random.Range(0, 4); // Generate a random number between 0 and 3

            // Set the direction based on the random number
            if (randomDir == 0)
            {
                dir = new Vector2(1, 0); // Right
            }
            else if (randomDir == 1)
            {
                dir = new Vector2(-1, 0); // Left
            }
            else if (randomDir == 2)
            {
                dir = new Vector2(0, 1); // Up
            }
            else if (randomDir == 3)
            {
                dir = new Vector2(0, -1); // Down
            }

            float randomDuration = Random.Range(1f, 3f); // Generate a random duration between 1 and 3 seconds

            StartCoroutine(MoveInDirection(dir, randomDuration));

            IEnumerator MoveInDirection(Vector2 direction, float duration)
            {
                animator.SetInteger("Direction", randomDir);
                dir.Normalize();
                animator.SetBool("IsMoving", dir.magnitude > 0);
                GetComponent<Rigidbody2D>().velocity = speed * direction;

                yield return new WaitForSeconds(duration);

                GetComponent<Rigidbody2D>().velocity = Vector2.zero;
            }
            // // Set the animation direction for the AI bot
            // animator.SetInteger("Direction", randomDir);

            // dir.Normalize();
            // animator.SetBool("IsMoving", dir.magnitude > 0);

            GetComponent<Rigidbody2D>().velocity = speed * dir;
        }
    }
}
