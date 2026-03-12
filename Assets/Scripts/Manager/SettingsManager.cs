using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System;
using System.Collections;

public class SettingsManager : MonoBehaviour
{
    [Header("Elements")]
    [SerializeField] private GameObject resetProgressPrompt;
    [SerializeField] private Slider pushMagnitudeSlider;
    [SerializeField] private Toggle sfxToggle;
    private bool canSave;

    [Header("Actions")]
    public static Action<float> onPushMagnitudeChanged;
    public static Action<bool> onSFXValueChanged;

    [Header("Data")]
    private const string lastPushMagnitudeKey = "lastPushMagnitude";
    private const string sfxActiveKey = "sfxActiveKey";
    private void Awake()
    {
        LoadData();
    }
    IEnumerator Start()
    {
        Initialize();

        yield return new WaitForSeconds(.5f);

        canSave = true;
    }

    private void Initialize()
    {
        onPushMagnitudeChanged?.Invoke(pushMagnitudeSlider.value);
        onSFXValueChanged?.Invoke(sfxToggle.isOn);
    }

    void Update()
    {
        
    }
    public void ResetProgressButtonCallback()
    {
        resetProgressPrompt.SetActive(true);
        SaveData();
    }
    public void ResetProgressYes()
    {
        PlayerPrefs.DeleteAll();
        SceneManager.LoadScene(0);
     
    }
    public void ResetProgressNo()
    {
        resetProgressPrompt.SetActive(false);
    }

    public void SliderValueChangedCallback()
    {
        onPushMagnitudeChanged?.Invoke(pushMagnitudeSlider.value);
        SaveData();
    }

    public void ToggleCallback(bool sfxActive)
    {
        
        onSFXValueChanged?.Invoke(sfxActive);
        SaveData();
    }
    private void LoadData()
    {
        pushMagnitudeSlider.value = PlayerPrefs.GetFloat(lastPushMagnitudeKey);
        sfxToggle.isOn = PlayerPrefs.GetInt(sfxActiveKey) == 1;
        
       
    }
    private void SaveData()
    {
        if(!canSave)
            return;

        PlayerPrefs.SetFloat(lastPushMagnitudeKey, pushMagnitudeSlider.value);
        int sfxValue = sfxToggle.isOn ? 1 : 0;
        PlayerPrefs.SetInt(sfxActiveKey, sfxValue);    
    }
}
