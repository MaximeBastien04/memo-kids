using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public class LogicManager : MonoBehaviour
{
    [System.Serializable]
    public class ImageSet
    {
        public Sprite fullImage;
        public Sprite missingImage;
        public Sprite correctItem;
        public Sprite[] wrongItems;
    }

    public Image pictureDisplay;
    public Button[] optionButtons;
    public Button playButton; // Play button to start the round
    public Button nextLevelButton; // Next level button to proceed

    public List<ImageSet> imageSets;
    private ImageSet currentSet;
    private Sprite correctAnswer;
    private int levelCount = 0;
    [SerializeField] private AudioManager audioManager;

    void Start()
    {
        SetupLevel();
    }

    void SetupLevel()
    {
        Debug.Log("Level: " + levelCount + ", imageset count: " + imageSets.Count.ToString());
        if (levelCount <= imageSets.Count)
        {
            currentSet = imageSets[levelCount];
        }
        else
        {
            SceneManager.LoadScene("EndScene");
        }

        // Start with full image visible
        pictureDisplay.sprite = currentSet.fullImage;

        // Hide buttons at start
        foreach (Button btn in optionButtons) btn.gameObject.SetActive(false);
        nextLevelButton.gameObject.SetActive(false);

        // Show play button
        playButton.gameObject.SetActive(true);
        playButton.onClick.RemoveAllListeners();
        playButton.onClick.AddListener(StartGame);
    }

    void StartGame()
    {
        // Hide play button, show answer choices
        playButton.gameObject.SetActive(false);
        foreach (Button btn in optionButtons) btn.gameObject.SetActive(true);

        // Show the missing item image
        pictureDisplay.sprite = currentSet.missingImage;

        LoadOptions();
    }

    void LoadOptions()
    {
        correctAnswer = currentSet.correctItem;

        // Create randomized answer choices
        List<Sprite> choices = new List<Sprite>(currentSet.wrongItems);
        choices.Add(correctAnswer);
        choices = ShuffleList(choices);

        // Assign images to the child Image component inside each button
        for (int i = 0; i < optionButtons.Length; i++)
        {
            Image buttonImage = optionButtons[i].transform.Find("Image").GetComponent<Image>();
            buttonImage.preserveAspect = true;
            buttonImage.sprite = choices[i]; // Assign sprite to child Image component

            Sprite selectedAnswer = choices[i]; // Store the answer choice
            optionButtons[i].onClick.RemoveAllListeners();
            optionButtons[i].onClick.AddListener(() => CheckAnswer(selectedAnswer));
        }
    }

    void CheckAnswer(Sprite chosenAnswer)
    {
        if (chosenAnswer == correctAnswer)
        {
            Debug.Log("Correct!");
            audioManager.WinSound();
            levelCount++;

            // Show Next Level button
            nextLevelButton.gameObject.SetActive(true);
            nextLevelButton.onClick.RemoveAllListeners();
            nextLevelButton.onClick.AddListener(SetupLevel);
        }
        else
        {
            audioManager.Wrongsound();
            Debug.Log("Wrong!");
        }
    }

    List<T> ShuffleList<T>(List<T> list)
    {
        for (int i = 0; i < list.Count; i++)
        {
            T temp = list[i];
            int randomIndex = Random.Range(i, list.Count);
            list[i] = list[randomIndex];
            list[randomIndex] = temp;
        }
        return list;
    }
}
