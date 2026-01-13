using UnityEngine;
using Cinemachine;
using UnityEngine.UI;

[System.Serializable]
public class PlayerComponents
{
    [Header("Components")]
    public InventoryManager inventory;
    public BaseWeapon weapon;
    public Dialog dialogSystem;
    public PlayerSpellbookBase spellbook;

    public SpriteRenderer spRender;

    [Header("Physics Components")]
    public Collider2D collider;
    public Rigidbody2D body;
    public PlayerDetections detection;

    private CinemachineBrain curBrain;
    public CinemachineBrain cinemachineBrain 
    {
        get
        {
            if (curBrain == null && MainCamera.Instance != null)
            {                
                if(MainCamera.Instance.GetCamera != null)
                    curBrain = MainCamera.Instance.GetCamera.GetComponent<CinemachineBrain>(); //GetCamera.GetComponent<CinemachineBrain>();
            }
            return curBrain;
        }
    }
    [Header("Cinemachine Components")]
    public CinemachineVirtualCamera virtualCamera;

    [HideInInspector]
    public CinemachineFramingTransposer virtualCameraTrasnposer
    {
		get
		{

            var brain = cinemachineBrain;
            if (brain == null) return null;
            if (cameraTransposer == null || cameraTransposer.gameObject != brain.ActiveVirtualCamera.VirtualCameraGameObject)
                cameraTransposer = cinemachineBrain.ActiveVirtualCamera.VirtualCameraGameObject.GetComponent<CinemachineVirtualCamera>().GetCinemachineComponent<CinemachineFramingTransposer>();

            return cameraTransposer;
        }
	}
    public CinemachineBasicMultiChannelPerlin virtualCameraNoise
    {
        get
        {
            if (cameraNoise == null || cameraNoise.gameObject != cinemachineBrain.ActiveVirtualCamera.VirtualCameraGameObject)
                cameraNoise = cinemachineBrain.ActiveVirtualCamera.VirtualCameraGameObject.GetComponent<CinemachineVirtualCamera>().GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
            return cameraNoise;
        }
    }
    public CinemachineBasicMultiChannelPerlin cameraNoise { get; set; }
    public CinemachineFramingTransposer cameraTransposer { get; set; }



    [Header("Effects Component")]
    public ParticleSystem footstepSmoke;
    public TrailRenderer lightBoltTrail;
    public ParticleSystem teleportParticle;
    public ParticleSystem energizingParticle;
    public ParticleSystem energizingPosParcile;

    [Header("Animators Components")]
    public Animator anim;
    public Animator teleportAnim;
    public AnimationClip preEnergizingClip;
    public AnimationClip[] teleportClip;
}
