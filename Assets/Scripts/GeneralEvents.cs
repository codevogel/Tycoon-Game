using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class GeneralEvents : MonoBehaviour
{
    public UnityEvent OnAwake, OnOnEnable, OnStart, OnTrigger, OnCollision, OnOnDisable, OnOnDestroy;

    private void Awake() { OnAwake.Invoke(); }
    private void OnEnable() { OnOnEnable.Invoke(); }
    private void Start() { OnStart.Invoke(); }
    private void OnTriggerEnter(Collider other) { OnTrigger.Invoke(); }
    private void OnCollisionEnter(Collision collision) { OnCollision.Invoke(); }
    private void OnDisable() { OnOnDisable.Invoke(); }
    private void OnDestroy() { OnOnDestroy.Invoke(); }
}
