using System.Collections.Generic;
using UnityEngine;

public class CharacterCreationTest : MonoBehaviour
{
    [Header("Test Settings")]
    public bool createCharacterOnStart = true;
    public int numberOfCharactersToCreate = 1;
    
    [Header("Character Creation System")]
    public CharacterCreationSystem characterCreationSystem;
    
    private List<Character> createdCharacters = new List<Character>();
    
    void Start()
    {
        if (createCharacterOnStart)
        {
            CreateTestCharacters();
        }
    }
    
    void Update()
    {
        // Press Space to create a new character
        if (Input.GetKeyDown(KeyCode.Space))
        {
            CreateTestCharacters();
        }
        
        // Press C to print all character sheets
        if (Input.GetKeyDown(KeyCode.C))
        {
            PrintAllCharacterSheets();
        }
        
        // Press L to print creation logs
        if (Input.GetKeyDown(KeyCode.L))
        {
            PrintCreationLogs();
        }
    }
    
    public void CreateTestCharacters()
    {
        if (characterCreationSystem == null)
        {
            characterCreationSystem = FindObjectOfType<CharacterCreationSystem>();
            if (characterCreationSystem == null)
            {
                Debug.LogError("No CharacterCreationSystem found in scene!");
                return;
            }
        }
        
        for (int i = 0; i < numberOfCharactersToCreate; i++)
        {
            Debug.Log($"\n=== CREATING CHARACTER {i + 1} ===");
            
            Character newCharacter = characterCreationSystem.CreateCharacter();
            createdCharacters.Add(newCharacter);
            
            Debug.Log($"\n=== CHARACTER {i + 1} COMPLETE ===");
            characterCreationSystem.PrintCharacterSheet(newCharacter);
        }
        
        Debug.Log($"\nCreated {numberOfCharactersToCreate} character(s). Total characters: {createdCharacters.Count}");
        Debug.Log("Press SPACE to create more characters, C to print all sheets, L to print logs");
    }
    
    public void PrintAllCharacterSheets()
    {
        Debug.Log($"\n=== ALL CHARACTER SHEETS ({createdCharacters.Count} characters) ===");
        
        for (int i = 0; i < createdCharacters.Count; i++)
        {
            Debug.Log($"\n--- CHARACTER {i + 1} ---");
            Debug.Log(createdCharacters[i].GetCharacterSheet());
        }
    }
    
    public void PrintCreationLogs()
    {
        if (characterCreationSystem != null)
        {
            List<string> logs = characterCreationSystem.GetCreationLog();
            Debug.Log($"\n=== CREATION LOGS ({logs.Count} entries) ===");
            
            foreach (string log in logs)
            {
                Debug.Log(log);
            }
        }
    }
    
    // Public method to get all created characters
    public List<Character> GetCreatedCharacters()
    {
        return new List<Character>(createdCharacters);
    }
    
    // Public method to get a specific character by index
    public Character GetCharacter(int index)
    {
        if (index >= 0 && index < createdCharacters.Count)
        {
            return createdCharacters[index];
        }
        return null;
    }
    
    // Public method to clear all characters
    public void ClearAllCharacters()
    {
        createdCharacters.Clear();
        Debug.Log("All characters cleared.");
    }
} 