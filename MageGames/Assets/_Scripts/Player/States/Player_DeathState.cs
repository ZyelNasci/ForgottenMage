 using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Player_DeathState : Base_State
{
    private PlayerComponents components;

    bool loadingScene;

    public void InitializeState(PlayerController _player, PlayerComponents _components) 
    {
        player      = _player;
        components = _components;        
    }
    public override void EnterState()
    {
        components.body.velocity = Vector2.zero;
        components.anim.SetBool("idle", true);
        player.StartCoroutine(DeathDelay());
        SceneManager.sceneLoaded += OnSceneLoaded;
    }
    public override void ExitState()
    {
        components.anim.SetBool("idle", false);
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }
    public override void FixedUpdate()
    {

    }
    public override void Update()
    {
        
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        Debug.Log("OnSceneLoaded: " + scene.name);
        Debug.Log(mode);
        loadingScene = false;
    }

    public IEnumerator DeathDelay()
	{
        yield return new WaitForSeconds(1f);
        FadeManager.Instance.FadeIn(0.5f);
        yield return new WaitForSeconds(0.5f);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        loadingScene = true;
        player.transform.position = CheckpointManager.Instance.GetCheckpoint().position;
        while (loadingScene) yield return null;
        player.transform.position = CheckpointManager.Instance.GetCheckpoint().position;
        yield return new WaitForEndOfFrame();
        player.ResetPlayer();
        FadeManager.Instance.FadeOut(0.5f);
        yield return new WaitForSeconds(0.5f);
        player.SwitchState(player.idleState);
    }
}