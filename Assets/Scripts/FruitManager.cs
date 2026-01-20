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
    private bool canControl;
    private bool isControlling;

    [Header(" Debug ")]
    [SerializeField] private bool enableGizmos;

    // Ýki meyve birleþtiðinde yeni meyve üretmek için MergeManager'dan gelen event dinlenir
    private void Awake()
    {
        MergeManager.onMergeProcessed += MergeProcessedCallback;
    }
   /*private void OnDestroy()
    {
        MergeManager.onMergeProcessed -= MergeProcessedCallback;
    }*/

    void Start()
    {
        canControl = true;
        HideLine();

    }

    void Update()
    {
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
        currentFruit.EnablePhysics();

        canControl = false;
        StartControlTimer();

        isControlling = false;
    }

    // Yeni meyve üretme iþlemi
    private void SpawnFruit()
    {
        Vector2 spawnPosition = GetSpawnPosition();
        Fruit fruitToInstantiate = spawnableFruit[Random.Range(0, spawnableFruit.Length)];

        currentFruit = Instantiate(fruitToInstantiate,
            spawnPosition,
            Quaternion.identity, 
            fruitsParent);

        currentFruit.name = "Fruit_" + Random.Range(0, 1000);
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
        Invoke("StopControlTimer", .5f);
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

        
        fruitInstance.DisableMergeTemporarily(0.5f); //added
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
