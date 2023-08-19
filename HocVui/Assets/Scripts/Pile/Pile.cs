using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Pile : MonoBehaviour
{
    public float collectionRadius = 2.0f; // Adjust this value to your desired collection radius.
    public GameObject collectedEffect; // Particle effect or other visual/audio cue.

    private void OnMouseDown()
    {
        if (IsPlayerCloseEnough())
        {
            CollectWood();
        }
    }

    private bool IsPlayerCloseEnough()
    {
        // Calculate the distance between the player and the wood.
        float distance = Vector3.Distance(transform.position, GameManager.Instance.PlayerManager.transform.position);

        // Return true if the distance is within the collection radius.
        return distance <= collectionRadius;
    }

    private void CollectWood()
    {
        GameManager.Instance.PlayerManager.GetComponent<Player_Base>().CollectPile();

        // Destroy the wood GameObject.
        Destroy(gameObject);
    }
}
