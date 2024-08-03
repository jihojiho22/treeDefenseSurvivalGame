using UnityEngine;

public class CharacterSpawner : MonoBehaviour
{
    public GameObject[] characterPrefabs;
    public Camera mainCamera;
    public float spawnDistance = 5f;

    void Start()
    {
        if (mainCamera == null)
        {
            mainCamera = Camera.main;
        }
        SpawnCharacter();
    }

    void SpawnCharacter()
    {
        if (GameManager.instance == null)
        {
            Debug.LogError("GameManager instance not found!");
            return;
        }

        Debug.Log($"Attempting to spawn character. Selected index: {GameManager.SelectedCharacterIndex}");
        Debug.Log($"Number of character prefabs: {characterPrefabs.Length}");

        if (characterPrefabs.Length == 0)
        {
            Debug.LogError("No character prefabs assigned!");
            return;
        }

        if (GameManager.SelectedCharacterIndex < 0 || GameManager.SelectedCharacterIndex >= characterPrefabs.Length)
        {
            Debug.LogWarning($"Invalid character index {GameManager.SelectedCharacterIndex}. Defaulting to first character.");
            GameManager.SelectedCharacterIndex = 0;
        }

        Vector3 spawnPosition = mainCamera.transform.position + mainCamera.transform.forward * spawnDistance;

        GameObject spawnedCharacter = GameManager.instance.SpawnOrGetCharacter(characterPrefabs[GameManager.SelectedCharacterIndex], spawnPosition);
        Player playerScript = spawnedCharacter.GetComponent<Player>();
    
        if (playerScript != null)
        {
            playerScript.InitializeHPBar();
            UpgradeManager.Instance.setPlayerScript(playerScript);
        }

        Weapon weaponScript = spawnedCharacter.GetComponentInChildren<Weapon>();
        if (weaponScript != null)
        {
            UpgradeManager.Instance.setWeaponScript(weaponScript);
        }

        Debug.Log($"Character spawned at position: {spawnPosition}");
    }
}