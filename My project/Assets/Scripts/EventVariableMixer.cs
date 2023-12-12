using UnityEngine;
using FMOD.Studio;

public class EventVariableMixer : MonoBehaviour
{
    [SerializeField]
    private FMODUnity.EventReference musicEvent;
    private EventInstance musicEventInstance;
    private static EventVariableMixer instance_;
    public GameObject yesSound, noSound, paperSound, tokenSound;
    public static EventVariableMixer Instance
    {
        get
        {
            if (instance_ == null)
            {
                GameObject singleton = new GameObject("VariableMixerSingleton");
                instance_ = singleton.AddComponent<EventVariableMixer>();
                DontDestroyOnLoad(singleton);
            }
            return instance_;
        }
    }
    private void Awake()
    {
        if (instance_ == null)
        {
            instance_ = this;
        }
        else
        {
            Destroy(gameObject);
        }

        DontDestroyOnLoad(gameObject);
    }

    void Start()
    {
        musicEventInstance = FMODUnity.RuntimeManager.CreateInstance(musicEvent);

        musicEventInstance.start();
    }

    void OnDestroy()
    {
        // Det�n y libera la instancia del evento al destruir el objeto
        musicEventInstance.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);

        musicEventInstance.release();
    }
    public void setMusicParameter(string parameterName, int number)
    {
        musicEventInstance.setParameterByName(parameterName, number);
    }
}