using UnityEngine;
using TMPro;
using System;

public class CommandManager : MonoBehaviour
{
    public TMP_InputField inputField;
    public GameObject cubePrefab;
    void Start()  // Add a listener to the input field's "onEndEdit" event
    {
        inputField.onEndEdit.AddListener(HandleInput);
    }

    void HandleInput(string userText) // This method will be called when the user finishes editing the input field
    {
        string[] cliInput = userText.Split( ' ');
        
        switch (cliInput[0]) {
            case "summon":
                HandleSummon(cliInput);
                break;
            case "give":
                HandleGive(cliInput);
                break;
            case "set":
                break;
            case "modify":
                break;
            case "help":
                break;
        }
    }
    
    private void HandleSummon(string[] cliInput) // summon cube <- { prefab name }  1,1,1 <- { scale } 1,1,1 <- { offset }
    {
        if (cliInput.Length == 4) {
            string[] scale = cliInput[2].Split(",");
            string[] offset = cliInput[3].Split(",");
            
            try {
                switch (cliInput[1]) {
                    case "cube":
                        cubePrefab.transform.localScale = new Vector3(Int32.Parse(scale[0]), Int32.Parse(scale[1]), Int32.Parse(scale[2]));
                        Instantiate(cubePrefab, transform.position + new Vector3(Int32.Parse(offset[0]), Int32.Parse(offset[1]), Int32.Parse(offset[2])), transform.rotation);
                        break;
                }
            } catch (FormatException e) {
                Debug.Log(e.Message);
            }
        } else
            Debug.Log("Looks like something went wrong maybe you have not given all the arguments or a argument has been formatted incorrectly please use :: summon cube <- { prefab name }  1,1,1 <- { scale } 1,1,1 <- { offset }");
    }

    private void HandleGive(string[] cliInput) // give player <- { obj / player }  walkSpeed <- { effect } 20 <- { value }
    {
        if (cliInput.Length == 4) {
            try {
                switch (cliInput[1]) {
                    case "player":
                        switch (cliInput[2]) {
                            case "sprintSpeed":
                                GetComponent<PlayerMovement>().sprintSpeed = Int32.Parse(cliInput[3]);
                                break;
                            case "walkSpeed":
                                GetComponent<PlayerMovement>().walkSpeed = Int32.Parse(cliInput[3]);
                                break;
                            case "crouchSpeed":
                                GetComponent<PlayerMovement>().crouchSpeed = Int32.Parse(cliInput[3]);
                                break;
                            case "maxHealth":
                                GetComponent<PlayerMovement>().maxHealth = Int32.Parse(cliInput[3]);
                                break;
                            case "currentHealth":
                                GetComponent<PlayerMovement>().currentHealth = Int32.Parse(cliInput[3]);
                                break;
                            case "groundDrag":
                                GetComponent<PlayerMovement>().groundDrag = Int32.Parse(cliInput[3]);
                                break;
                            case "jumpForce":
                                GetComponent<PlayerMovement>().jumpForce = Int32.Parse(cliInput[3]);
                                break;
                            case "jumpCooldown":
                                GetComponent<PlayerMovement>().jumpCooldown = Int32.Parse(cliInput[3]);
                                break;
                        }
                        break;
                }
            } catch (FormatException e) {
                Debug.Log(e.Message);
            }
        }
        else
            Debug.Log("Looks like something went wrong maybe you have not given all the arguments or a argument has been formatted incorrectly please use ::give player <- { obj / player }  walkSpeed <- { effect } 20 <- { value }");
    }
}