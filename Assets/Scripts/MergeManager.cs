using System.Collections;
using UnityEngine;
using System;

public class MergeManager : MonoBehaviour
{

    [Header(" Actions ")]
    // Merge iþlemi tamamlandýðýnda tetiklenen event
    public static Action<FruitType, Vector2> onMergeProcessed;


    //[Header(" Settings ")]
    //Fruit lastSender;


    void Start()
    {
        Fruit.onCollisionWithFruit += CollisionBetweenFruitsCallback;
    }

    private void OnDestroy()  //added
    {
        Fruit.onCollisionWithFruit -= CollisionBetweenFruitsCallback;
    }
   

    void Update()
    {

    }

    // Ýki meyve çarpýþtýðýnda çaðrýlan callback fonksiyonu
    private void CollisionBetweenFruitsCallback(Fruit sender, Fruit otherFruit)
    {
        /* if (lastSender != null )
         {
             return;
         }
         lastSender = sender;*/
        if (sender.GetInstanceID() >= otherFruit.GetInstanceID())
            return;

        if (!sender.gameObject.activeInHierarchy || !otherFruit.gameObject.activeInHierarchy)
            return;

        ProcessMerge(sender, otherFruit);

        Debug.Log("Collision detected by" + sender.name);
    }

    // Ýki meyveyi birleþtirip yeni meyveyi üretme iþlemi
    private void ProcessMerge(Fruit sender, Fruit otherFruit)
    {
       
        sender.gameObject.SetActive(false);
        otherFruit.gameObject.SetActive(false);
        
        FruitType mergeFruitType = sender.GetFruitType();
        mergeFruitType += 1;

        Vector2 fruitSpawnPos = (sender.transform.position + otherFruit.transform.position) / 2;


        /*Destroy(sender.gameObject);
        Destroy(otherFruit.gameObject);*/


        //StartCoroutine(ResetLastSenderCoroutine());
        onMergeProcessed?.Invoke(mergeFruitType, fruitSpawnPos);

        Destroy(sender.gameObject);
        Destroy(otherFruit.gameObject);
    }

    IEnumerator ResetLastSenderCoroutine()
    {
        yield return new WaitForEndOfFrame();
        //lastSender = null;
    }


}

