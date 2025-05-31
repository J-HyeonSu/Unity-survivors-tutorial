using Mono.Cecil;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    //무기ID, 프리펩ID, 데미지, 개수, 속도
    public int id;
    public int prefabId;
    public float damage;
    public int count;
    public float speed;

    private float timer;
    private Player player;

    void Awake()
    {
        player = GameManager.instance.player;
    }
    void Start()
    {
    }
    
    void Update()
    {
        switch (id)
        {
            case 0:
                transform.Rotate(Vector3.forward*(-speed*Time.deltaTime));
                break;
            default:
                timer += Time.deltaTime;
                if (timer > speed)
                {
                    timer = 0f;
                    Fire();
                }
                break;
        }

        if (Input.GetButtonDown("Jump"))
        {
            LevelUp(5, 1);
        }
    }

    public void LevelUp(float damage, int count)
    {
        this.damage += damage;
        this.count += count;

        if (id == 0)
        {
            Batch();
        }
        
        player.BroadcastMessage("ApplyGear", SendMessageOptions.DontRequireReceiver);
    }

    public void Init(ItemData data)
    {
        // Basic Set
        name = "Weapon" + data.itemName;
        transform.parent = player.transform;
        transform.localPosition = Vector3.zero;
        
        
        // Property Set
        id = data.itemId;
        damage = data.baseDamage;
        count = data.baseCount;

        for (int idx = 0; idx < GameManager.instance.pool.prefabs.Length; idx++)
        {
            if (data.projectile == GameManager.instance.pool.prefabs[idx])
            {
                prefabId = idx;
                break;
            }
        }
        
        switch (id)
        {
            case 0:
                speed = 150;
                Batch();
                break;
            default:
                speed = 0.3f;
                break;
        }
        
        // Hand Set
        Hand hand = player.hands[(int)data.itemType];
        hand.spriter.sprite = data.hand;
        hand.gameObject.SetActive(true);
        
        player.BroadcastMessage("ApplyGear", SendMessageOptions.DontRequireReceiver);
    }

    void Batch()
    {
        for (int idx = 0; idx < count; idx++)
        {
            Transform bullet;
            if (idx < transform.childCount)
            {
                bullet = transform.GetChild(idx);
            }
            else
            {
                bullet = GameManager.instance.pool.Get(prefabId).transform;   
                bullet.parent = transform;
            }
            
            bullet.localPosition = Vector3.zero;
            bullet.localRotation = Quaternion.identity;

            Vector3 rotVec = Vector3.forward * 360 * idx / count;
            bullet.Rotate(rotVec);
            bullet.Translate(bullet.up*1.5f,Space.World);
                
                
            bullet.GetComponent<Bullet>().Init(damage, -1, Vector3.zero); // -1 is infinity
            

        }
    }

    void Fire()
    {
        if (!player.scanner.nearestTarget) return;

        Vector3 targetPos = player.scanner.nearestTarget.position;
        Vector3 dir = (targetPos - transform.position).normalized;
        
        Transform bullet = GameManager.instance.pool.Get(prefabId).transform;
        bullet.position = transform.position;
        bullet.rotation = Quaternion.FromToRotation(Vector3.up, dir);
        
        bullet.GetComponent<Bullet>().Init(damage, count, dir);

    }


}
