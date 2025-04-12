using UnityEngine; using UnityEngine.UI;

public class SubwaySurferStyleGame : MonoBehaviour { [Header("Player")] public CharacterController controller; public Transform player; public float forwardSpeed = 10f; public float laneDistance = 4f; private int desiredLane = 1; public float jumpForce = 8f; public float gravity = -20f; private Vector3 direction;

[Header("Camera")]
public Transform mainCamera;
private Vector3 camOffset;

[Header("Obstacles")]
public GameObject[] obstaclePrefabs;
public float distanceBetween = 15f;
private float spawnZ = 30f;

[Header("UI")]
public Text scoreText;
private float score;

void Start()
{
    if (player == null) player = this.transform;
    if (controller == null) controller = player.GetComponent<CharacterController>();
    if (mainCamera != null) camOffset = mainCamera.position - player.position;
}

void Update()
{
    RunLogic();
    CameraFollow();
    ScoreUpdate();
    SpawnObstacles();
}

void RunLogic()
{
    direction.z = forwardSpeed;

    if (Input.GetKeyDown(KeyCode.RightArrow)) desiredLane = Mathf.Min(desiredLane + 1, 2);
    if (Input.GetKeyDown(KeyCode.LeftArrow)) desiredLane = Mathf.Max(desiredLane - 1, 0);

    Vector3 targetPosition = player.position.z * Vector3.forward;
    if (desiredLane == 0) targetPosition += Vector3.left * laneDistance;
    else if (desiredLane == 2) targetPosition += Vector3.right * laneDistance;

    Vector3 moveVector = Vector3.zero;
    moveVector.x = (targetPosition - player.position).x * 10f;
    moveVector.y = direction.y;
    moveVector.z = direction.z;

    if (controller.isGrounded)
    {
        direction.y = -1f;
        if (Input.GetKeyDown(KeyCode.Space))
            direction.y = jumpForce;
    }
    else
    {
        direction.y += gravity * Time.deltaTime;
    }

    controller.Move(moveVector * Time.deltaTime);
}

void CameraFollow()
{
    if (mainCamera != null)
    {
        mainCamera.position = player.position + camOffset;
    }
}

void ScoreUpdate()
{
    score = player.position.z;
    if (scoreText != null)
        scoreText.text = "Score: " + Mathf.FloorToInt(score).ToString();
}

void SpawnObstacles()
{
    if (player.position.z + 60f > spawnZ)
    {
        GameObject obstacle = obstaclePrefabs[Random.Range(0, obstaclePrefabs.Length)];
        Vector3 spawnPos = new Vector3(RandomLaneX(), 0, spawnZ);
        Instantiate(obstacle, spawnPos, Quaternion.identity);
        spawnZ += distanceBetween;
    }
}

float RandomLaneX()
{
    int lane = Random.Range(0, 3);
    return (lane - 1) * laneDistance;
}

}

