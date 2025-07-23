using UnityEngine;

public class GameSetup : MonoBehaviour
{
    [Header("Required Components")]
    public CSVDataLoader csvDataLoader;
    public CharacterCreationSystem characterCreationSystem;
    public TerminalInterface terminalInterface;
    
    [Header("Auto Setup")]
    public bool autoSetupOnStart = true;
    
    void Start()
    {
        if (autoSetupOnStart)
        {
            SetupGame();
        }
    }
    
    [ContextMenu("Setup Game")]
    public void SetupGame()
    {
        Debug.Log("Setting up Dungeon Simulator...");
        
        // Find or create CSV Data Loader
        if (csvDataLoader == null)
        {
            csvDataLoader = FindObjectOfType<CSVDataLoader>();
            if (csvDataLoader == null)
            {
                GameObject csvLoaderGO = new GameObject("CSV Data Loader");
                csvDataLoader = csvLoaderGO.AddComponent<CSVDataLoader>();
                Debug.Log("Created CSV Data Loader");
            }
        }
        
        // Find or create Character Creation System
        if (characterCreationSystem == null)
        {
            characterCreationSystem = FindObjectOfType<CharacterCreationSystem>();
            if (characterCreationSystem == null)
            {
                GameObject creationSystemGO = new GameObject("Character Creation System");
                characterCreationSystem = creationSystemGO.AddComponent<CharacterCreationSystem>();
                Debug.Log("Created Character Creation System");
            }
        }
        
        // Find or create Terminal Interface
        if (terminalInterface == null)
        {
            terminalInterface = FindObjectOfType<TerminalInterface>();
            if (terminalInterface == null)
            {
                GameObject terminalGO = new GameObject("Terminal Interface");
                terminalInterface = terminalGO.AddComponent<TerminalInterface>();
                Debug.Log("Created Terminal Interface");
            }
        }
        
        // Connect components
        if (characterCreationSystem != null && csvDataLoader != null)
        {
            characterCreationSystem.csvDataLoader = csvDataLoader;
            Debug.Log("Connected CSV Data Loader to Character Creation System");
        }
        
        if (terminalInterface != null && characterCreationSystem != null)
        {
            terminalInterface.characterCreationSystem = characterCreationSystem;
            Debug.Log("Connected Character Creation System to Terminal Interface");
        }
        
        // Initialize data-driven system
        if (csvDataLoader != null)
        {
            DataDrivenTableData.Initialize(csvDataLoader);
            Debug.Log("Initialized Data-Driven Table System");
        }
        
        Debug.Log("Game setup complete! You can now create characters.");
        Debug.Log("If using the terminal interface, type 'start' to create a character.");
        Debug.Log("If using the test system, press SPACE to create characters.");
    }
    
    [ContextMenu("Reload CSV Data")]
    public void ReloadCSVData()
    {
        if (csvDataLoader != null)
        {
            csvDataLoader.ReloadCSVFiles();
            Debug.Log("CSV data reloaded");
        }
        else
        {
            Debug.LogWarning("No CSV Data Loader found. Run Setup Game first.");
        }
    }
    
    [ContextMenu("Test Character Creation")]
    public void TestCharacterCreation()
    {
        if (characterCreationSystem != null)
        {
            Character testCharacter = characterCreationSystem.CreateCharacter();
            Debug.Log("Test character created: " + testCharacter.characterName);
            Debug.Log(testCharacter.GetCharacterSheet());
        }
        else
        {
            Debug.LogWarning("No Character Creation System found. Run Setup Game first.");
        }
    }
    
    void OnValidate()
    {
        // Auto-assign components if they're null
        if (csvDataLoader == null)
            csvDataLoader = FindObjectOfType<CSVDataLoader>();
        
        if (characterCreationSystem == null)
            characterCreationSystem = FindObjectOfType<CharacterCreationSystem>();
        
        if (terminalInterface == null)
            terminalInterface = FindObjectOfType<TerminalInterface>();
    }
} 