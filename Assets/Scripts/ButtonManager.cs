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
        
        if(SceneManager.GetActiveScene().name != "MenuScene" && SceneManager.GetActiveScene().name != "EndGameScene") {
            isMuted = PlayerPrefs.GetInt("Muted", 0) == 1;
            AudioListener.volume = isMuted ? 0f : 1f;
            UpdateMuteButton();
        } 
   
    }

    void Update()
    {
       
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
        SceneManager.LoadScene("MenuScene");
    }

    public void LoadLevel2()
    {
        SceneManager.LoadScene("Level2");
    }

    public void LoadLevel3()
    {
        SceneManager.LoadScene("Level3");
    }

    public void LoadLevel4()
    {
        SceneManager.LoadScene("Level4");
    }
    public void LoadGameEnd()
    {
        SceneManager.LoadScene("EndGameScene");
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

    // Save  sound state
        PlayerPrefs.SetInt("Muted", isMuted ? 1 : 0);
        PlayerPrefs.Save();

        // Update the button appearance
        UpdateMuteButton();
    }

    void UpdateMuteButton()
    {
        // Change button sprite based on mute state
        muteButtonImage.sprite = isMuted ? mutedSprite : unmutedSprite;
    }
}