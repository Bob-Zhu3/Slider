using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WinCon : MonoBehaviour
{
    public Dot player;
    private void OnTriggerEnter2D(Collider2D other)
    {
        player.advanceLevel();
    }
}