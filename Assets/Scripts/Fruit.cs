using UnityEngine;
using System;

public class Fruit : MonoBehaviour
{
    [Header("Elements")]
    [SerializeField] private SpriteRenderer spriteRenderer;

    [Header("Data")]
    [SerializeField] private FruitType fruitType;
    private bool hasCollided;
    private bool canBeMerged;

    [Header("Actions")]
    // Fruit objeleri ile MergeManager arasýnda iletiþimi saðlayan event
    public static Action<Fruit, Fruit> onCollisionWithFruit;

    //private bool canMerge = true;

    void Start()
    {
        Invoke("AllowMerge", .25f);
        
    }
    void Update()
    {
        
    }

    private void AllowMerge()
    {
        canBeMerged = true;
    }

    // Meyveyi verilen pozisyona taþýr
    public void MoveTo(Vector2 targetPosition)
    {
        transform.position = targetPosition;
    }

    // Meyvenin fiziðini aktif eder, düþmesini saðlar
    public void EnablePhysics()
    {
        GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Dynamic;
        GetComponent<Collider2D>().enabled = true;
    }

    // Meyvenin belirli bir süre merge olmasýný engeller
    /*public void DisableMergeTemporarily(float duration) //added
    {
        canMerge = false;
        Invoke(nameof(EnableMerge), duration);
    }
    private void EnableMerge() //added
    {
        canMerge = true;
    }*/

    // Baþka bir meyveyle çarpýþma olduðunda merge kontrolü yapar
    private void OnCollisionEnter2D(Collision2D collision)
    {
        ManageCollision(collision);
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        ManageCollision(collision); 
    }

    private void ManageCollision(Collision2D collision)
    {
        hasCollided = true;

        //if (!canMerge) return; //added

        if (!canBeMerged)
            return;

        if (collision.collider.TryGetComponent(out Fruit otherFruit))
        {
            if (otherFruit.GetFruitType() != fruitType)
                return;

            if (!otherFruit.CanBeMerged())
                return;

            onCollisionWithFruit?.Invoke(this, otherFruit);
        }
    }
    public FruitType GetFruitType()
    {
        return fruitType;
    }

    public Sprite GetSprite()
    {
        return spriteRenderer.sprite;
    }

    public bool HasCollided()
    { 
        return hasCollided; 
    }

    public bool CanBeMerged()
    {
        return canBeMerged;
    }
}
