using UnityEngine;
using UnityEngine.SceneManagement;

public class Player_Car : MonoBehaviour
{

    public AudioSource audioSource; // Drag your AudioSource here
    public AudioSource oneShotSource; // Drag the second AudioSource here
    public AudioClip tune1;
    public AudioClip tune2;
    public AudioClip tune3;
    public AudioClip tune4;
    public AudioClip tune5;
    public AudioClip tune6;
    public AudioClip tune7;

    public static float acceleration = 3;
    public static float deceleration = 10f;
    public static float maxSpeed = 30f;
    public static float horizontalSpeed = 4f;

    public float tiltAmount = 10f;
    public float tiltSpeed = 5f;

    public Transform carModel; // drag the CarModel child here

    public Renderer baseCarRenderer; // Drag the Base_Car object here
    public Material normalStopSignalMat;
    public Material brakeStopSignalMat;

    public ParticleSystem fireParticleSystem; // Drag your fire particle system here
    public int switchSceneIndex; // Index of the scene to switch to after collision

    private float fireDuration = 2f; // Duration of fire particle system
    public LayerMask trafficLayer; // Assign to "Traffic" layer in Inspector
    public float currentSpeed = 0f;
    private bool wasBraking = false;

    private bool hasCollided = false; // Prevent multiple triggers
    private enum SoundState { None, AccelLow, AccelMid, AccelHigh, Brake, Release, Idle }
    private SoundState currentSoundState = SoundState.None;
    private bool wasAccelerating = false;

    void Update()
    {
        bool accelerating = Input.GetKey(KeyCode.W);
        bool braking = Input.GetKey(KeyCode.S);

        // Handle engine sound logic
        if (braking && currentSpeed!=0)
        {
            SwitchSound(SoundState.Brake, tune5);
            
        }
        else if (accelerating)
        {
            if (currentSpeed < 8f)
                SwitchSound(SoundState.AccelLow, tune1);
            else if (currentSpeed < 20f)
                SwitchSound(SoundState.AccelMid, tune2);
            else
                SwitchSound(SoundState.AccelHigh, tune3);
        }
        else if (wasAccelerating && !accelerating)
        {
            oneShotSource.PlayOneShot(tune4); // Just play release sound once
        }
        else //if (!accelerating && !braking)
        {
            SwitchSound(SoundState.Idle, tune6);
            currentSpeed = currentSpeed - 0.006666f;
        }

        wasAccelerating = accelerating;


        if (braking && !wasBraking)
        {
            // Get all current materials
            Material[] mats = baseCarRenderer.materials;
            // Replace stop signal materials at Element 4 and 5
            mats[4] = brakeStopSignalMat;
            mats[5] = brakeStopSignalMat;
            baseCarRenderer.materials = mats;
        }
        else if (!braking && wasBraking)
        {
            Material[] mats = baseCarRenderer.materials;
            mats[4] = normalStopSignalMat;
            mats[5] = normalStopSignalMat;
            baseCarRenderer.materials = mats;
        }
        wasBraking = braking;
        bool turningLeft = Input.GetKey(KeyCode.A);
        bool turningRight = Input.GetKey(KeyCode.D);

        // Move the root GameObject FORWARD in world space
        currentSpeed += (accelerating ? acceleration : 0f) * Time.deltaTime;
        currentSpeed -= (braking ? deceleration : 0f) * Time.deltaTime;
        currentSpeed = Mathf.Clamp(currentSpeed, 0f, maxSpeed);

        // Always move forward in world space (not local/tilted space)
        transform.Translate(Vector3.forward * currentSpeed * Time.deltaTime, Space.World);

        // Move left/right in world space
        if (turningLeft)
            transform.Translate(Vector3.left * horizontalSpeed * Time.deltaTime, Space.World);
        if (turningRight)
            transform.Translate(Vector3.right * horizontalSpeed * Time.deltaTime, Space.World);

        // --- VISUAL TILT (affects only the child model) ---
        float targetZ = 0f; // roll
        float targetX = 0f; // pitch

        if (turningLeft && !accelerating && !braking) targetZ = -tiltAmount;
        if (turningRight && !accelerating && !braking) targetZ = tiltAmount;

        if (turningLeft && accelerating) { targetZ = -tiltAmount; targetX = -tiltAmount; }
        if (turningRight && accelerating) { targetZ = tiltAmount; targetX = -tiltAmount; }

        if (turningLeft && braking) { targetZ = -tiltAmount; targetX = tiltAmount; }
        if (turningRight && braking) { targetZ = tiltAmount; targetX = tiltAmount; }

        if (!turningLeft && !turningRight && !accelerating && !braking)
        {
            targetZ = 0f;
            targetX = 0f;
        }

        if (accelerating && !turningLeft && !turningRight) targetX = -tiltAmount;
        if (braking && !turningLeft && !turningRight) targetX = tiltAmount;

        Quaternion targetRot = Quaternion.Euler(targetX, 0f, targetZ);
        carModel.localRotation = Quaternion.Lerp(carModel.localRotation, targetRot, tiltSpeed * Time.deltaTime);
    }

    void SwitchSound(SoundState newState, AudioClip clip)
    {
        if (currentSoundState == newState) return; // Already playing, do nothing

        audioSource.clip = clip;
        if (newState == SoundState.Release) { audioSource.loop = false; }
        else { audioSource.loop = true; }
        
        audioSource.Play();
        currentSoundState = newState;
    }

    void OnCollisionEnter(Collision collision)
    {
        // Check if collision is with an object on the Traffic layer
        if (!hasCollided && ((1 << collision.gameObject.layer) & trafficLayer) != 0)
        {
            hasCollided = true;

            if (fireParticleSystem != null)
                fireParticleSystem.Play();

            // Disable player controls if needed:
            //currentSpeed = 0f;

            oneShotSource.PlayOneShot(tune7);

            // Switch scenes after 2 seconds
            Invoke("SwitchSceneAfterCollision", fireDuration);
        }
    }

    void SwitchSceneAfterCollision()
    {
        SceneManager.LoadScene(switchSceneIndex); // Load scene by index
        // SceneManager.LoadScene("YourSceneName"); // Alternatively, load scene by name
    }

}
