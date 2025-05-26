using System;
using UnityEngine;

public class Reposition : MonoBehaviour
{
    private void Start()
    {
        Debug.Log("Reposition");
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Area"))
        {
            var playerPos = GameManager.instance.player.transform.position;
            var myPos = transform.position;

            float diffX = Mathf.Abs(playerPos.x - myPos.x);
            float diffY = Mathf.Abs(playerPos.y - myPos.y);

            var playDir = GameManager.instance.player.inputVec;
            switch (transform.tag)
            {
                case "Ground":
                    if (diffX > diffY)
                    {
                        transform.Translate(Vector3.right * playDir.x * 40);
                    }
                    else if (diffX < diffY)
                    {
                        transform.Translate(Vector3.up * playDir.y * 40);
                    }
                    
                    break;
                case "Enemy":
                    break;
            }
        }
    }
}
