using System.Collections;
using UnityEngine;

public class Door : MonoBehaviour, IInteractable
{
    public enum DoorState { Closed, Opening, Open, Closing }

    [Header("Rotation")]
    [Tooltip("Transform that pivots when the door swings. Defaults to this GameObject.")]
    [SerializeField] private Transform pivot;
    [Tooltip("Local axis the door rotates around.")]
    [SerializeField] private Vector3 rotationAxis = Vector3.up;
    [Tooltip("Angle in degrees the door rotates when opening.")]
    [SerializeField] private float openAngle = 90f;
    [Tooltip("Seconds it takes to fully open or close.")]
    [SerializeField] private float swingDuration = 0.5f;

    [Header("State")]
    [SerializeField] private bool startOpen = false;
    [SerializeField] private bool locked = false;

    [Header("Audio (optional)")]
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip openSound;
    [SerializeField] private AudioClip closeSound;
    [SerializeField] private AudioClip lockedSound;

    public DoorState State { get; private set; } = DoorState.Closed;
    public bool IsLocked { get => locked; set => locked = value; }

    private Quaternion closedRotation;
    private Quaternion openRotation;
    private Coroutine swingRoutine;

    void Awake()
    {
        if (pivot == null) pivot = transform;
        closedRotation = pivot.localRotation;
        openRotation = closedRotation * Quaternion.AngleAxis(openAngle, rotationAxis.normalized);

        if (startOpen)
        {
            pivot.localRotation = openRotation;
            State = DoorState.Open;
        }
    }

    public void Interact()
    {
        if (locked)
        {
            PlayClip(lockedSound);
            return;
        }

        switch (State)
        {
            case DoorState.Closed:
            case DoorState.Closing:
                Open();
                break;
            case DoorState.Open:
            case DoorState.Opening:
                Close();
                break;
        }
    }

    public void Open()
    {
        if (locked || State == DoorState.Open || State == DoorState.Opening) return;
        PlayClip(openSound);
        StartSwing(openRotation, DoorState.Opening, DoorState.Open);
    }

    public void Close()
    {
        if (State == DoorState.Closed || State == DoorState.Closing) return;
        PlayClip(closeSound);
        StartSwing(closedRotation, DoorState.Closing, DoorState.Closed);
    }

    public void Toggle() => Interact();

    private void StartSwing(Quaternion target, DoorState transitional, DoorState finalState)
    {
        if (swingRoutine != null) StopCoroutine(swingRoutine);
        swingRoutine = StartCoroutine(SwingTo(target, transitional, finalState));
    }

    private IEnumerator SwingTo(Quaternion target, DoorState transitional, DoorState finalState)
    {
        State = transitional;
        Quaternion start = pivot.localRotation;
        float elapsed = 0f;
        float duration = Mathf.Max(0.0001f, swingDuration);

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            float t = Mathf.SmoothStep(0f, 1f, elapsed / duration);
            pivot.localRotation = Quaternion.Slerp(start, target, t);
            yield return null;
        }

        pivot.localRotation = target;
        State = finalState;
        swingRoutine = null;
    }

    private void PlayClip(AudioClip clip)
    {
        if (clip == null || audioSource == null) return;
        audioSource.PlayOneShot(clip);
    }
}
