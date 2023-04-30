using System.Collections.Generic;
using UnityEngine;

public class Node : MonoBehaviour
{
    public LayerMask obstacleLayer; // Esteiden kerros

    public List<Vector2> availableDirections { get; private set; } // Lista saatavilla olevista suunnista

    private void Start()
    {
        this.availableDirections = new List<Vector2>(); // Alustetaan saatavilla olevien suuntien lista

        // Tarkistetaan saatavilla oleva suunta
        CheckAvailableDirection(Vector2.up);
        CheckAvailableDirection(Vector2.down); 
        CheckAvailableDirection(Vector2.left);
        CheckAvailableDirection(Vector2.right);
    }

    private void CheckAvailableDirection(Vector2 direction)
    {
        // Tarkistetaan onko este suunnassa k‰ytt‰en fysiikkamoottoria
        RaycastHit2D hit = Physics2D.BoxCast(this.transform.position, Vector2.one * 0.5f, 0f, direction, 1.0f, this.obstacleLayer);

        if (hit.collider == null) // Jos este ei ole havaittu, lis‰t‰‰n suunta saatavilla olevien suuntien listaan
        {
            this.availableDirections.Add(direction);
        }
    }
}
