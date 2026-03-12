using UnityEngine;

public class WallFixer : MonoBehaviour
{
    [Header("Elements")]
    [SerializeField] private Transform rightWall;
    [SerializeField] private Transform leftWall;


    void Start()
    {
        float aspectRatio = (float)Screen.height / Screen.width;

        Camera mainCamera = Camera.main;

        float halfScreenWidth = mainCamera.orthographicSize / aspectRatio;

        rightWall.transform.position = new Vector3(halfScreenWidth + .5f,0,0);
        leftWall.transform.position = -rightWall.transform.position;

    }

    
    void Update()
    {
        
    }
}
