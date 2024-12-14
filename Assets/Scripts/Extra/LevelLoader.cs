using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class LevelLoader : MonoBehaviour
{
    private static readonly int LoadNext = Animator.StringToHash("LoadNext");
    [SerializeField] private Animator animator;
    
    public void OnPlayButtonClicked(int buildIndex)
    {
        StartCoroutine(LoadNextScene(buildIndex));
    }

    private IEnumerator LoadNextScene(int sceneIndexToShow)
    {
        animator.SetTrigger(LoadNext);
        
        yield return new WaitForSeconds(1f);
        
        SceneManager.LoadScene(sceneIndexToShow);
    }
}
