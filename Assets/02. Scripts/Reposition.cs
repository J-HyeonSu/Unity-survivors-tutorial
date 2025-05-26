using System;
using UnityEngine;
using Random = UnityEngine.Random;

public class Reposition : MonoBehaviour
{
    private Collider2D coll;

    private void Awake()
    {
        coll = GetComponent<Collider2D>();
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Area"))
        {
            var playerPos = GameManager.instance.player.transform.position;
            var myPos = transform.position;

            float diffX = Mathf.Abs(playerPos.x - myPos.x);
            float diffY = Mathf.Abs(playerPos.y - myPos.y);

            Vector3 playDir = GameManager.instance.player.inputVec;
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
                    if (coll.enabled)
                    {
                        transform.Translate(playDir * 20 + new Vector3(Random.Range(-3f, 3f),Random.Range(-3f, 3f), 0f));
                    }
                    break;
            }
        }
    }
}
