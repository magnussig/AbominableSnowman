using UnityEngine;

public class CameraScript : MonoBehaviour {

    private GameObject target;
    private GameObject environment;
    private bool isFollowingPlayer;
    private bool isAtPlayer;
    private bool hasStartedMovement;
    private float startTime;
    private float travelDistance;
    private Vector3 startPosition;
    private Vector3 endPosition;

    [SerializeField] private Vector3 offset;
    [SerializeField] private float speed;
    [SerializeField] private float xMin, xMax, yMin, yMax;

    public bool IsFollowingPlayer {
        get {
            return isFollowingPlayer;
        }
        set {
            bool old = isFollowingPlayer;
            isFollowingPlayer = value;

            if (isFollowingPlayer != old) {
                isAtPlayer = false;
                hasStartedMovement = false;
            }
        }
    }
    public bool IsAtPlayer { get { return isAtPlayer; } }

	void Start () {
        isFollowingPlayer = true;
        isAtPlayer = true;
        hasStartedMovement = false;
        target = GameObject.FindWithTag("Player");
        environment = GameObject.FindWithTag("Environment");
    }

	void Update () {
        if (isFollowingPlayer && !isAtPlayer) {
            if (!hasStartedMovement)
                UpdateVariables();
            MoveToPlayer();
        }
        else if (isFollowingPlayer)
        {
            FollowPlayer();
        }
        else
            MoveAroundEnvironment();
	}

    void FollowPlayer() {
        transform.position = new Vector3
                                (
                                    target.transform.position.x,
                                    target.transform.position.y,
                                    transform.position.z
                                ) + offset;
    }

    void MoveAroundEnvironment() {
        float x_mov = Input.GetAxis("Horizontal") * Time.deltaTime * speed;

        float newx = transform.position.x + x_mov;

        if (newx < xMin || newx > xMax) return;

        Vector3 newpos = new Vector3(newx, transform.position.y, transform.position.z);
        transform.position = newpos;
    }

    void MoveToPlayer() {
        hasStartedMovement = true;
        transform.position = Vector3.MoveTowards(transform.localPosition, endPosition, speed * Time.deltaTime);

        if (transform.position.Equals(endPosition))
            isAtPlayer = true;
    }

    void UpdateVariables() {
        startTime = Time.time;

        if (isFollowingPlayer) {
            travelDistance = Mathf.Abs(transform.position.x - target.transform.position.x);
            startPosition = transform.position;
            endPosition = new Vector3(target.transform.position.x, target.transform.position.y, transform.position.z) + offset;
        }
    }
}
