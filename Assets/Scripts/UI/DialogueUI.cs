using UnityEngine;
using Lyr.Dialogue;
using TMPro;
using UnityEngine.UI;

namespace Lyr.UI
{
    public class DialogueUI : MonoBehaviour
    {
        PlayerConversant playerConversant;
        [SerializeField] TextMeshProUGUI AIText;
        [SerializeField] Button nextButton;
        [SerializeField] GameObject AIPanel;
        [SerializeField] GameObject PlayerPanel;
        [SerializeField] GameObject choicePrefab;
        [SerializeField] GameObject choiceRoot;
        [SerializeField] GameObject goodbyeText;
        [SerializeField] GameObject DialogueScreen;

        void Start()
        {
            playerConversant = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerConversant>();
            nextButton.onClick.AddListener(Next);

            UpdateUI();
        }

        void Next()
        {
            if(playerConversant.HasNext())
            {
                playerConversant.Next();
                UpdateUI();
            }
        }

        private void Update() 
        {
            if(Input.GetKeyDown(KeyCode.Space) && !playerConversant.HasNext())
            {
                DialogueScreen.SetActive(false);
            }
        }
        
        void UpdateUI()
        {
            AIPanel.SetActive(!playerConversant.IsChoosing());
            PlayerPanel.SetActive(playerConversant.IsChoosing());

            if(playerConversant.IsChoosing())
            {
                BuildChoiceList();

            }
            else
            {
                AIText.text = playerConversant.GetText();
                nextButton.gameObject.SetActive(playerConversant.HasNext());
                goodbyeText.gameObject.SetActive(!playerConversant.HasNext());
            }
        }

        private void RemoveListOptions()
        {
            foreach (Transform item in choiceRoot.transform)
            {
                Destroy(item.gameObject);
            }
        }

        private void BuildChoiceList()
        {
            RemoveListOptions();

            foreach (DialogueNode node in playerConversant.GetChoices())
            {
                GameObject playerChoice = Instantiate(choicePrefab, choiceRoot.transform);
                var buttonText = playerChoice.GetComponentInChildren<TextMeshProUGUI>();
                buttonText.text = node.GetText();
                
                Button button = playerChoice.GetComponentInChildren<Button>();
                button.onClick.AddListener(() => 
                {
                    playerConversant.SelectChoice(node);
                    UpdateUI();
                });
            }

        }

    }
}

