using UnityEngine;

public class AudioManager : MonoBehaviour
{
    [Header("Elements")]
    [SerializeField] private AudioSource mergeSource;

    private void Awake()
    {
        MergeManager.onMergeProcessed += MergeProcessedCallback;
    }
    private void OnDestroy()
    {
        MergeManager.onMergeProcessed -= MergeProcessedCallback;
    }
    private void MergeProcessedCallback(FruitType fruitType, Vector2 mergePos)
    {
        PlayMergeSound();
    }
    public void PlayMergeSound()
    {
        mergeSource.pitch = Random.Range(.9f, 1.1f);
        mergeSource.Play();
    }


}
