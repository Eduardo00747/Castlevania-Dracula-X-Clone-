using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StairsMoviment : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("ClimbUp"))
        {
            PlayerController playerController = other.GetComponent<PlayerController>();

            if (playerController != null)
            {
                //playerController.StartClimbingAnimation();
            }
        }
    }
}