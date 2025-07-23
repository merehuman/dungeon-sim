using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TerminalInterface : MonoBehaviour
{
    [Header("UI References")]
    public Text outputText;
    public InputField inputField;
    public ScrollRect scrollRect;
    
    [Header("Character Creation")]
    public CharacterCreationSystem characterCreationSystem;
    public CharacterCreationTest characterTest;
    
    private List<string> terminalHistory = new List<string>();
    private List<Character> createdCharacters = new List<Character>();
    
    void Start()
    {
        if (characterCreationSystem == null)
            characterCreationSystem = FindObjectOfType<CharacterCreationSystem>();
        
        if (characterTest == null)
            characterTest = FindObjectOfType<CharacterCreationTest>();
        
        PrintWelcomeMessage();
        
        // Focus on input field
        if (inputField != null)
        {
            inputField.Select();
            inputField.ActivateInputField();
        }
    }
    
    void Update()
    {
        // Handle Enter key press
        if (Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.KeypadEnter))
        {
            ProcessInput();
        }
        
        // Handle Tab key to focus input field
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            if (inputField != null)
            {
                inputField.Select();
                inputField.ActivateInputField();
            }
        }
    }
    
    public void ProcessInput()
    {
        if (inputField == null) return;
        
        string input = inputField.text.Trim().ToLower();
        inputField.text = "";
        
        if (string.IsNullOrEmpty(input)) return;
        
        PrintToTerminal($"> {input}");
        
        switch (input)
        {
            case "start":
            case "create":
            case "create character":
                CreateCharacter();
                break;
                
            case "list":
            case "characters":
            case "show characters":
                ListCharacters();
                break;
                
            case "sheet":
            case "character sheet":
            case "show sheet":
                ShowCharacterSheet();
                break;
                
            case "help":
                ShowHelp();
                break;
                
            case "clear":
                ClearTerminal();
                break;
                
            case "quit":
            case "exit":
                PrintToTerminal("Exiting...");
                #if UNITY_EDITOR
                UnityEditor.EditorApplication.isPlaying = false;
                #else
                Application.Quit();
                #endif
                break;
                
            default:
                PrintToTerminal($"Unknown command: '{input}'. Type 'help' for available commands.");
                break;
        }
        
        // Focus back on input field
        inputField.Select();
        inputField.ActivateInputField();
    }
    
    private void CreateCharacter()
    {
        PrintToTerminal("Starting character creation...");
        
        if (characterCreationSystem == null)
        {
            PrintToTerminal("ERROR: Character creation system not found!");
            return;
        }
        
        Character newCharacter = characterCreationSystem.CreateCharacter();
        createdCharacters.Add(newCharacter);
        
        PrintToTerminal($"Character created successfully! Total characters: {createdCharacters.Count}");
        PrintToTerminal($"Name: {newCharacter.characterName}");
        PrintToTerminal($"Race: {newCharacter.race}, Class: {newCharacter.characterClass}");
        PrintToTerminal($"Stats: STR {newCharacter.strength}, DEX {newCharacter.dexterity}, WIS {newCharacter.wisdom}");
        PrintToTerminal($"Health: {newCharacter.currentHealth}/{newCharacter.maxHealth}");
        
        // Print creation log
        List<string> creationLog = characterCreationSystem.GetCreationLog();
        PrintToTerminal("--- Creation Log ---");
        foreach (string log in creationLog)
        {
            PrintToTerminal(log);
        }
        
        PrintToTerminal("--- End Creation Log ---");
    }
    
    private void ListCharacters()
    {
        if (createdCharacters.Count == 0)
        {
            PrintToTerminal("No characters created yet. Type 'start' to create one.");
            return;
        }
        
        PrintToTerminal($"--- Character List ({createdCharacters.Count} characters) ---");
        
        for (int i = 0; i < createdCharacters.Count; i++)
        {
            Character character = createdCharacters[i];
            PrintToTerminal($"{i + 1}. {character.characterName} - {character.race} {character.characterClass}");
            PrintToTerminal($"   Health: {character.currentHealth}/{character.maxHealth}, Gold: {character.gold}g");
        }
    }
    
    private void ShowCharacterSheet()
    {
        if (createdCharacters.Count == 0)
        {
            PrintToTerminal("No characters created yet. Type 'start' to create one.");
            return;
        }
        
        if (createdCharacters.Count == 1)
        {
            PrintCharacterSheet(createdCharacters[0]);
        }
        else
        {
            PrintToTerminal($"Multiple characters found. Please specify which one (1-{createdCharacters.Count}) or type 'all' for all sheets.");
        }
    }
    
    private void PrintCharacterSheet(Character character)
    {
        PrintToTerminal("--- Character Sheet ---");
        string[] lines = character.GetCharacterSheet().Split('\n');
        
        foreach (string line in lines)
        {
            if (!string.IsNullOrEmpty(line))
            {
                PrintToTerminal(line);
            }
        }
    }
    
    private void ShowHelp()
    {
        PrintToTerminal("--- Available Commands ---");
        PrintToTerminal("start/create - Create a new character");
        PrintToTerminal("list/characters - Show list of created characters");
        PrintToTerminal("sheet - Show character sheet(s)");
        PrintToTerminal("clear - Clear terminal output");
        PrintToTerminal("help - Show this help message");
        PrintToTerminal("quit/exit - Exit the application");
    }
    
    private void ClearTerminal()
    {
        terminalHistory.Clear();
        if (outputText != null)
        {
            outputText.text = "";
        }
        PrintWelcomeMessage();
    }
    
    private void PrintWelcomeMessage()
    {
        PrintToTerminal("=== DUNGEON SIMULATOR - CHARACTER CREATION ===");
        PrintToTerminal("Type 'start' to begin character creation");
        PrintToTerminal("Type 'help' for available commands");
        PrintToTerminal("=============================================");
    }
    
    private void PrintToTerminal(string message)
    {
        terminalHistory.Add(message);
        
        if (outputText != null)
        {
            outputText.text = string.Join("\n", terminalHistory);
            
            // Auto-scroll to bottom
            if (scrollRect != null)
            {
                Canvas.ForceUpdateCanvases();
                scrollRect.verticalNormalizedPosition = 0f;
            }
        }
        
        // Also print to Unity console for debugging
        Debug.Log($"[Terminal] {message}");
    }
    
    // Public method to get all created characters
    public List<Character> GetCreatedCharacters()
    {
        return new List<Character>(createdCharacters);
    }
    
    // Public method to add a character programmatically
    public void AddCharacter(Character character)
    {
        createdCharacters.Add(character);
        PrintToTerminal($"Character '{character.characterName}' added to terminal.");
    }
} 