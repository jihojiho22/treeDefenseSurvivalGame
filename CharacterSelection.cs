using UnityEngine;
using UnityEngine.SceneManagement;


public class CharacterSelection : MonoBehaviour
{
    public void SelectCharacter(int characterIndex)
    {
        GameManager.SelectedCharacterIndex = characterIndex;
        SceneManager.LoadScene("GameStartScene");
    }

    public void showSelectCharacter(){
        DestroyOnLoad.ClearPersistentObjects();
        SceneManager.LoadScene("CharacterSelectionScene");
        
    }
    
}