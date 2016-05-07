using UnityEngine;

public abstract class Trap : MonoBehaviour {

    public bool isEnabled { get; set; }

    abstract public void executeTrapEffectOnEnemy(EnemyController enemy);
}
