using UnityEngine;



public class FruitManager : MonoBehaviour
{
    [Header(" Elements ")]
    [SerializeField] private GameObject fruitPrefab;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
      
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            ManagePlayerInput();
        }
    }

    private void ManagePlayerInput()
    {
        Instantiate(fruitPrefab, GetClickedWorldPosition(), Quaternion.identity);
    }

    private Vector2 GetClickedWorldPosition()
    {
        return Camera.main.ScreenToWorldPoint(Input.mousePosition);
    }
}
