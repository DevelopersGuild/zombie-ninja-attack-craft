using UnityEngine;
using System.Collections;

public class Key:Pickup
{
    public bool isBossKey;
    public Sprite bossSprite;
    private SpriteRenderer sr;

    public void Start()
    {
        sr = GetComponent<SpriteRenderer>();
        if (isBossKey)
            sr.sprite = bossSprite;
    }

    public override void AddItemToInventory(Collider2D player, int value)
    {
        if (isBossKey)
            GameManager.addBossKeys(1);
        else
            GameManager.addKeys(1);
    }

    public override void sendPickupMessage()
    {
        GameManager.Notifications.PostNotification(this, "KeyPickedUp");
    }
}
