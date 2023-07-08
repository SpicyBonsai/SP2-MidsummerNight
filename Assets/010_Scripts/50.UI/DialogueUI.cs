using UnityEngine;
using Lyr.Dialogue;
using TMPro;
using UnityEngine.UI;
using System.Collections;
using System.Linq;

namespace Lyr.UI
{
    public class DialogueUI : MonoBehaviour
    {
        PlayerConversant playerConversant;
        [SerializeField] TextMeshProUGUI AIText;
        private Button nextButton;
        [SerializeField] GameObject AIPanel;
        [SerializeField] GameObject PlayerPanel;
        [SerializeField] GameObject choicePrefab;
        [SerializeField] GameObject choiceRoot;
        [SerializeField] GameObject goodbyeText;
        [SerializeField] GameObject DialogueScreen;
        [SerializeField] GameObject continueText;
        [SerializeField] private float typingSpeed = 0.1f;
        [SerializeField] private float timeBetweenLines = 0.3f;
        private Coroutine displayLine;
        private bool canContinueToNextLine = false;
        private bool _skippingLine;
        [SerializeField] GameOptions gameOptions;

        public void InitiateDialogue()
        {
            playerConversant = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerConversant>();
            nextButton = AIPanel.GetComponent<Button>();
            nextButton.onClick.AddListener(ButtonClick);
            InputManager.GetInstance().SwitchToUI();
            playerConversant.TriggerEnterAction();
            UpdateUI();
        }

        void ButtonClick()
        {  
            if(canContinueToNextLine && !playerConversant.HasNext())
            {
                ExitDialogue();
            }

            if(canContinueToNextLine) 
            {
                Next();
            }
            else
            {
                _skippingLine = true;
            }
            
        }

        void Next()
        {
            if(playerConversant.HasNext() && canContinueToNextLine)
            {
                playerConversant.Next();
                UpdateUI();
            }
        }

        private void Update() 
        {
            if (InputManager.GetInstance().ResetDialogue)
            {
                playerConversant.ResetDialogue();
                UpdateUI();
            }

            if (InputManager.GetInstance().Submit && !canContinueToNextLine)
            {
                _skippingLine = true;
            }
            
            //If line is animating, get out of update loop
            if(!canContinueToNextLine) return;

            //proceed to next line if player hits space
            if(InputManager.GetInstance().Submit && playerConversant.HasNext())
            {
                playerConversant.Next();
                UpdateUI();
            }  

            //if there are no more dialogue options, get out of dialogue when player hits the submit button
            if(InputManager.GetInstance().Submit && !playerConversant.HasNext())
            {
                ExitDialogue();
            }
        }

        private void ExitDialogue()
        {
            DialogueScreen.SetActive(false);
            InputManager.GetInstance().SwitchToGameplay();
            playerConversant.TriggerExitAction();
            playerConversant.Quit();
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
                //replace the TMPro text with the current node's text

                if(displayLine != null)
                {
                    StopCoroutine(displayLine);
                }
                displayLine = StartCoroutine(DisplayAILine(playerConversant.GetText()));

            
                // nextButton.gameObject.SetActive(playerConversant.HasNext());
                // goodbyeText.gameObject.SetActive(!playerConversant.HasNext());
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
            playerConversant.TriggerEnterAction();
            nextButton.enabled = false;
            RemoveListOptions();

            foreach (DialogueNode node in playerConversant.GetChoices())
            {
                GameObject playerChoice = Instantiate(choicePrefab, choiceRoot.transform);
                var buttonText = playerChoice.GetComponentInChildren<TextMeshProUGUI>();

                //replace the TMPro text with the current node's text
                buttonText.text = node.GetText();
                
                Button button = playerChoice.GetComponentInChildren<Button>();
                button.onClick.AddListener(() => 
                {
                    
                    playerConversant.SelectChoice(node);
                    nextButton.enabled = true;
                    UpdateUI();
                });
            }

        }

        private IEnumerator DisplayAILine(string line)
        {
            continueText.SetActive(false);
            goodbyeText.gameObject.SetActive(false);
            playerConversant.TriggerEnterAction();

            //empty the dialogue text
            canContinueToNextLine = false;

            AIText.text = "";
            foreach(char letter in line.ToCharArray())
            {
                if(_skippingLine)
                {
                    AIText.text = new string(line.Where(x => x != '#').ToArray());
                    _skippingLine = false;
                    break;
                }

                if(letter == '#')
                {
                    playerConversant.TriggerOnTime();
                    continue;
                }

                if(letter == '\n')
                {
                    yield return new WaitForSeconds(timeBetweenLines);
                }

                AIText.text += letter;
                yield return new WaitForSeconds(typingSpeed * (1 - gameOptions.TextSpeed));
            }

            canContinueToNextLine = true;
            continueText.SetActive(playerConversant.HasNext());
            playerConversant.TriggerExitAction();
            goodbyeText.gameObject.SetActive(!playerConversant.HasNext());
        }


    }
}

