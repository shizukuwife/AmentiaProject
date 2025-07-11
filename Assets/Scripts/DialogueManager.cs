using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Collections;

[System.Serializable]
public class DialogueLine
{
    public string speakerName;
    public string sentence;
    public Sprite portrait;
}

public class DialogueManager : MonoBehaviour
{
    [Header("UI")]
    public GameObject dialoguePanel;
    public TextMeshProUGUI nameText;
    public TextMeshProUGUI dialogueText;
    public Image portraitImage;

    [Header("Dialogue Data")]
    public DialogueLine[] dialogueLines;

    [Header("Player Control")]
    public GameObject player;

    private int currentIndex = 0;
    private bool isDialogueActive = false;
    private CanvasGroup portraitCanvasGroup;

    void Start()
    {
        portraitCanvasGroup = portraitImage.GetComponent<CanvasGroup>();
        StartDialogue(); // ลบถ้าไม่อยากให้เริ่มทันที
    }

    void Update()
    {
        if (isDialogueActive && Input.GetKeyDown(KeyCode.Z))
        {
            NextLine();
        }
    }

    public void StartDialogue()
    {
        currentIndex = 0;
        isDialogueActive = true;
        dialoguePanel.SetActive(true);
        if (player != null) player.SetActive(false);

        ShowLine();
    }

    void ShowLine()
    {
        DialogueLine line = dialogueLines[currentIndex];
        nameText.text = line.speakerName;
        dialogueText.text = line.sentence;

        if (line.portrait != null)
        {
            portraitImage.sprite = line.portrait;
            portraitImage.enabled = true;
            StopAllCoroutines();
            StartCoroutine(FadeInPortrait());
        }
        else
        {
            portraitImage.enabled = false;
            portraitCanvasGroup.alpha = 0f;
        }
    }

    void NextLine()
    {
        currentIndex++;
        if (currentIndex < dialogueLines.Length)
        {
            ShowLine();
        }
        else
        {
            EndDialogue();
        }
    }

    void EndDialogue()
    {
        dialoguePanel.SetActive(false);
        isDialogueActive = false;
        if (player != null) player.SetActive(true);
    }

    IEnumerator FadeInPortrait()
    {
        portraitCanvasGroup.alpha = 0f;
        float duration = 0.3f;
        float elapsed = 0f;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            portraitCanvasGroup.alpha = Mathf.Clamp01(elapsed / duration);
            yield return null;
        }

        portraitCanvasGroup.alpha = 1f;
    }
}
