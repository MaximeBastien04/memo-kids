using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using Unity.VisualScripting;
using TMPro;

public class LogicManager : MonoBehaviour
{
    [System.Serializable]
    public class ImageSet
    {
        public Sprite fullImage;
        public Sprite missingImage1;
        public Sprite correctItem1;
        public Sprite[] wrongItems1;

        public Sprite missingImage2;
        public Sprite correctItem2;
        public Sprite[] wrongItems2;
    }

    public Image pictureDisplay;
    public Button[] optionButtons;
    public Button playButton;
    public Button nextLevelButton;

    public List<ImageSet> imageSets;
    private ImageSet currentSet;
    private Sprite correctAnswer;
    private int levelCount = 0;
    [SerializeField] private AudioManager audioManager;
    [SerializeField] private GameObject readyText;

    void Start()
    {
        SetupLevel();
    }

    void SetupLevel()
    {
        if (levelCount < imageSets.Count)
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
        readyText.SetActive(true);
        playButton.onClick.RemoveAllListeners();
        playButton.onClick.AddListener(StartGame);
    }

    void StartGame()
    {
        // Hide play button, show answer choices
        playButton.gameObject.SetActive(false);
        readyText.SetActive(false);
        foreach (Button btn in optionButtons) btn.gameObject.SetActive(true);

        // Show the missing item image
        bool useFirstMissingImage = Random.value > 0.5f;
        pictureDisplay.sprite = useFirstMissingImage ? currentSet.missingImage1 : currentSet.missingImage2;

        LoadOptions(useFirstMissingImage);
    }

    void LoadOptions(bool useFirstSet)
    {
        if (useFirstSet)
        {
            correctAnswer = currentSet.correctItem1;
            AssignOptions(currentSet.correctItem1, currentSet.wrongItems1);
        }
        else
        {
            correctAnswer = currentSet.correctItem2;
            AssignOptions(currentSet.correctItem2, currentSet.wrongItems2);
        }
    }

    void AssignOptions(Sprite correct, Sprite[] wrongItems)
    {
        List<Sprite> choices = new List<Sprite>(wrongItems);
        choices.Add(correct);
        choices = ShuffleList(choices);

        // Assign images to the child Image component inside each button
        for (int i = 0; i < optionButtons.Length; i++)
        {
            Image buttonImage = optionButtons[i].transform.Find("Image").GetComponent<Image>();
            buttonImage.sprite = choices[i]; // Assign sprite to child Image component

            Sprite selectedAnswer = choices[i]; // Store the answer choice
            optionButtons[i].onClick.RemoveAllListeners();
            optionButtons[i].onClick.AddListener(() => CheckAnswer(selectedAnswer));
        }
    }

    void CheckAnswer(Sprite chosenAnswer)
    {
        Button clickedButton = null;
        // Find the clicked button by comparing its image
        foreach (Button btn in optionButtons)
        {
            Image buttonImage = btn.transform.Find("Image").GetComponent<Image>();
            if (buttonImage.sprite == chosenAnswer)
            {
                clickedButton = btn;
                break;
            }
        }

        if (chosenAnswer == correctAnswer)
        {
            audioManager.WinSound();
            levelCount++;
            pictureDisplay.sprite = currentSet.fullImage;

            // Show Next Level button
            nextLevelButton.gameObject.SetActive(true);
            nextLevelButton.onClick.RemoveAllListeners();
            nextLevelButton.onClick.AddListener(SetupLevel);

            // Hide Option buttons
            foreach (Button btn in optionButtons) btn.gameObject.SetActive(false);
        }
        else
        {
            audioManager.Wrongsound();

            // Play shake animation on the clicked button
            Animator animator = clickedButton.transform.Find("Image").GetComponent<Animator>();
            animator.SetTrigger("shake");
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
