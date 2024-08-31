using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwipeDetection : MonoBehaviour
{
    public Dot player;
    private Vector3 startPos;
    public int pixelDistToDetect = 20;
    private bool fingerDown;
    
    void Update()
    { 
        if (!fingerDown && Input.touchCount > 0 && Input.touches[0].phase == TouchPhase.Began)
        {
            startPos = Input.touches[0].position;
            fingerDown = true;
        }

        if (fingerDown)
        {
            if (Input.touches[0].position.y >= startPos.y + pixelDistToDetect)
            {
                fingerDown = false;
                int upHowMany = player.Calculate(1);
                player.Move(new Vector3(0, upHowMany));
            }
            else if (Input.touches[0].position.y <= startPos.y - pixelDistToDetect)
            {
                fingerDown = false;
                int downHowMany = player.Calculate(2);
                player.Move(new Vector3(0, downHowMany));
            }
            else if (Input.touches[0].position.x <= startPos.x - pixelDistToDetect)
            {
                fingerDown = false;
                int leftHowMany = player.Calculate(3);
                player.Move(new Vector3(leftHowMany, 0));
            }
            else if (Input.touches[0].position.x >= startPos.x + pixelDistToDetect)
            {
                fingerDown = false;
                int rightHowMany = player.Calculate(4);
                player.Move(new Vector3(rightHowMany, 0));
            }
        }

        if (fingerDown && Input.touchCount > 0 && Input.touches[0].phase == TouchPhase.Ended)
        {
            fingerDown = false;
        }
    }    
}