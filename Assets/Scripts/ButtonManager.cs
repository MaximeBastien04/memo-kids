using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ButtonManager : MonoBehaviour
{
    public Button muteButton;       
    public Image muteButtonImage;       
    public Sprite unmutedSprite;    
    public Sprite mutedSprite;      

    private bool isMuted = false;   
    void Start()
    {
        if(SceneManager.GetActiveScene().name != "StartScene" && SceneManager.GetActiveScene().name != "EndScene") {
            isMuted = PlayerPrefs.GetInt("Muted", 0) == 1;
            AudioListener.volume = isMuted ? 0f : 1f;
            UpdateMuteButton();
        }
    }

    public void PlayGame()
    {
        SceneManager.LoadScene("Level1");
        
    }

    public void RestartLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void LoadStartScene()
    {
        SceneManager.LoadScene("StartScene");
    }

    public void LoadGameEnd()
    {
        SceneManager.LoadScene("EndScene");
    }

    public void MuteSound()
    {
        AudioListener.volume = 0f;
    }

    public void AmplifySound()
    {
        AudioListener.volume = 1f;
    }
  
     public void ToggleMute()
    {
        isMuted = !isMuted;
        AudioListener.volume = isMuted ? 0f : 1f;

        PlayerPrefs.SetInt("Muted", isMuted ? 1 : 0);
        PlayerPrefs.Save();

        UpdateMuteButton();
    }

    void UpdateMuteButton()
    {
        muteButtonImage.sprite = isMuted ? mutedSprite : unmutedSprite;
    }
}