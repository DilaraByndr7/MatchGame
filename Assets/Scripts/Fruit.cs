using UnityEngine;
using System;

public class Fruit : MonoBehaviour
{
    [Header("Data")]
    [SerializeField] private FruitType fruitType;

    [Header("Actions")]
    // Fruit objeleri ile MergeManager arasýnda iletiþimi saðlayan event
    public static Action<Fruit, Fruit> onCollisionWithFruit;

    private bool canMerge = true;

    void Start()
    {
        
    }
    void Update()
    {
        
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
    }

    // Meyvenin belirli bir süre merge olmasýný engeller
    public void DisableMergeTemporarily(float duration) //added
    {
        canMerge = false;
        Invoke(nameof(EnableMerge), duration);
    }
    private void EnableMerge() //added
    {
        canMerge = true;
    }

    // Baþka bir meyveyle çarpýþma olduðunda merge kontrolü yapar
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (!canMerge) return; //added

        if (collision.collider.TryGetComponent(out Fruit otherFruit))
        {
            if (otherFruit.GetFruitType() != fruitType)
                return;


            onCollisionWithFruit?.Invoke(this, otherFruit);
        }
    }
    public FruitType GetFruitType()
    {
        return fruitType;
    }
  
}
