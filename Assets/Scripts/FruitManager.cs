using UnityEngine;



public class FruitManager : MonoBehaviour
{
    [Header(" Elements ")]
    [SerializeField] private Fruit[] fruitPrefabs;
    [SerializeField] private LineRenderer fruitDropLine;
    private Fruit currentFruit;

    [Header(" Settings ")]
    [SerializeField] private Transform fruitSpawnLine;
    private bool canControl;
    private bool isControlling;

    [Header(" Debug ")]
    [SerializeField] private bool enableGizmos;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        canControl = true;
        HideLine();
    }

    // Update is called once per frame
    void Update()
    {
        if (canControl)
            ManagePlayerInput();
       
    }

    private void ManagePlayerInput()
    {
        if (Input.GetMouseButtonDown(0))
        {
            MouseDownCallback();
        }
        else if (Input.GetMouseButton(0) )
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

    private void MouseDownCallback()
    {
        DisplayLine();
        PlaceLineAtClickedPosition();

        SpawnFruit();

        isControlling = true;
    }
    private void MouseDragCallback()
    {
        PlaceLineAtClickedPosition();

        currentFruit.MoveTo(new Vector2(GetSpawnPosition().x, fruitSpawnLine.position.y));
    }
    private void MouseUpCallback()
    {
        HideLine();
        currentFruit.EnablePhysics();
        
        canControl = false;
        StartControlTimer();

        isControlling = false;
    }

    private void SpawnFruit()
    {
        Vector2 spawnPosition = GetSpawnPosition();

        currentFruit = Instantiate(fruitPrefabs[Random.Range(0,fruitPrefabs.Length)], spawnPosition, Quaternion.identity);

    }

    private Vector2 GetClickedWorldPosition()
    {
        return Camera.main.ScreenToWorldPoint(Input.mousePosition);
    }

    private Vector2 GetSpawnPosition()
    {
        Vector2 worldClickedPosition = GetClickedWorldPosition();
        worldClickedPosition.y = fruitSpawnLine.position.y;
        return worldClickedPosition;
    }

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

    private void StartControlTimer()
    {
        Invoke("StopControlTimer", .5f);
    }

    private void StopControlTimer()
    {
        canControl = true;
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
