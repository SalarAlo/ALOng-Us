using UnityEngine;

public class PlayerCam : SingletonNetwork<PlayerCam>
{
    [SerializeField] private float sensX;
    [SerializeField] private float sensY;
    [SerializeField] private Transform  orientation;
    private float xRot;
    private float yRot;

    public override void Awake()
    {
        base.Awake();
        
        Debug.Log("PlayerCam, AWake");
    }

    private void Start() {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void LateUpdate() {
        if (!orientation) return;

        float mouseX = Input.GetAxisRaw("Mouse X") * Time.deltaTime * sensX;
        float mouseY = Input.GetAxisRaw("Mouse Y") * Time.deltaTime * sensY;
        
        yRot += mouseX;
        xRot -= mouseY;

        xRot = Mathf.Clamp(xRot, -80f, 80f);

        transform.rotation = Quaternion.Euler(xRot, yRot, 0);
        orientation.rotation = Quaternion.Euler(0, yRot, 0);
    }

    public void SetOrientation(Transform orientation) {
        this.orientation = orientation;
    }
}
