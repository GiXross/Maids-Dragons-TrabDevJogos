using UnityEngine;

public interface TriggerInterface
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Trigger();
    void AfterTrigger();
    void DisableMovement();
    void EnableMovement();
}
