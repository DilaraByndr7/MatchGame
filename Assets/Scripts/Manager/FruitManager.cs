using System;
using UnityEngine;




public class FruitManager : MonoBehaviour
{
    [Header(" Elements ")]
    [SerializeField] private Fruit[] fruitPrefabs;
    [SerializeField] private Fruit[] spawnableFruit;
    [SerializeField] private Transform fruitsParent;
    [SerializeField] private LineRenderer fruitDropLine;
    private Fruit currentFruit;

    [Header(" Settings ")]
    [SerializeField] private Transform fruitSpawnLine;
    [SerializeField] private float spawnDelay;
    private bool canControl;
    private bool isControlling;


    [Header(" Next Fruit Settings ")]
    private int nextFruitIndex;


    [Header(" Debug ")]
    [SerializeField] private bool enableGizmos;

    [Header(" Actions ")]
    public static Action onNextFruitIndexSet;

    // Ýki meyve birleþtiðinde yeni meyve üretmek için MergeManager'dan gelen event dinlenir
    private void Awake()
    {
        MergeManager.onMergeProcessed += MergeProcessedCallback;
    }
   private void OnDestroy()
    {
        MergeManager.onMergeProcessed -= MergeProcessedCallback;
    }

    void Start()
    {
        SetNextFruitIndex();

        canControl = true;
        HideLine();

    }

    void Update()
    {
        if (!GameManager.instance.IsGameState())
            return;
        if (canControl)
            ManagePlayerInput();

    }

    // Oyuncu mouse inputlarýný yöneten fonksiyon
    private void ManagePlayerInput()
    {
        if (Input.GetMouseButtonDown(0))
        {
            MouseDownCallback();
        }
        else if (Input.GetMouseButton(0))
        {
            if (isControlling)
                MouseDragCallback();
            else
                MouseDownCallback();
        }
        
        else if (Input.GetMouseButtonUp(0) && isControlling)
        {
            MouseUpCallback();
        }


    }

    // Mouse basýldýðýnda çalýþan fonksiyon
    private void MouseDownCallback()
    {
        DisplayLine();
        PlaceLineAtClickedPosition();

        SpawnFruit();

        isControlling = true;
    }
    // Mouse basýlý tutulup sürüklendiðinde çalýþan fonksiyon
    private void MouseDragCallback()
    {
        PlaceLineAtClickedPosition();

        currentFruit.MoveTo(new Vector2(GetSpawnPosition().x, fruitSpawnLine.position.y));
    }
    // Mouse býrakýldýðýnda çalýþan fonksiyon
    private void MouseUpCallback()
    {
        HideLine();

        if(currentFruit != null)
            currentFruit.EnablePhysics();

        canControl = false;
        StartControlTimer();

        isControlling = false;
    }

    // Yeni meyve üretme iþlemi
    private void SpawnFruit()
    {
        Vector2 spawnPosition = GetSpawnPosition();
        Fruit fruitToInstantiate = spawnableFruit[nextFruitIndex];

        currentFruit = Instantiate(fruitToInstantiate,
            spawnPosition,
            Quaternion.identity, 
            fruitsParent);

        SetNextFruitIndex();
    }

    private void SetNextFruitIndex()
    {
        nextFruitIndex = UnityEngine.Random.Range(0, spawnableFruit.Length);
      
        onNextFruitIndexSet?.Invoke();
    }

    public string GetNextFruitName()
    {
        return spawnableFruit[nextFruitIndex].name;
    }
    public Sprite GetNextFruitSprite()
    {
        Console.WriteLine(spawnableFruit[nextFruitIndex].GetSprite());
        return spawnableFruit[nextFruitIndex].GetSprite();
        
    }

    // Mouse'un dünya koordinatýndaki pozisyonunu döndürür
    private Vector2 GetClickedWorldPosition()
    {
        return Camera.main.ScreenToWorldPoint(Input.mousePosition);
    }
    // Meyvenin spawn edileceði pozisyonu hesaplar
    private Vector2 GetSpawnPosition()
    {
        Vector2 worldClickedPosition = GetClickedWorldPosition();
        worldClickedPosition.y = fruitSpawnLine.position.y;
        return worldClickedPosition;
    }

    // Düþüþ çizgisini mouse pozisyonuna göre ayarlar
    private void PlaceLineAtClickedPosition()
    {
        fruitDropLine.SetPosition(0, GetSpawnPosition());
        fruitDropLine.SetPosition(1, GetSpawnPosition() + Vector2.down * 15);
    }

    private void HideLine()
    {
        fruitDropLine.enabled = false;
    }
    private void DisplayLine()
    {
        fruitDropLine.enabled = true;
    }

    // Oyuncu kontrolünü kýsa süreliðine kapatan timer baþlatýr
    private void StartControlTimer()
    {
        Invoke("StopControlTimer", spawnDelay);
    }

    private void StopControlTimer()
    {
        canControl = true;
    }

    // Merge gerçekleþtiðinde çaðrýlýr, yeni meyveyi üretir
    private void MergeProcessedCallback(FruitType fruitType, Vector2 spawnPosition)
    {
        for (int i = 0; i < fruitPrefabs.Length; i++)
        {
            if (fruitPrefabs[i].GetFruitType() == fruitType)
            {
                SpawnMergedFruit(fruitPrefabs[i], spawnPosition);
                Debug.Log("Processing merge...");
                break; 
            }
        }
        
       
    }

    // Merge sonrasý oluþan yeni meyveyi sahneye yerleþtirir
    private void SpawnMergedFruit(Fruit fruit, Vector2 spawnPosition)
    {
       

        Fruit fruitInstance = Instantiate(fruit, spawnPosition, Quaternion.identity, fruitsParent);

        
        //fruitInstance.DisableMergeTemporarily(0.3f); //added
        fruitInstance.EnablePhysics();
    }








#if UNITY_EDITOR

    private void OnDrawGizmos()
    {
        if (!enableGizmos)
            return;

        Gizmos.color = Color.yellow;
        Gizmos.DrawLine(new Vector3(-50, fruitSpawnLine.position.y, 0), new Vector3(50, fruitSpawnLine.position.y, 0));
    }

#endif
}
