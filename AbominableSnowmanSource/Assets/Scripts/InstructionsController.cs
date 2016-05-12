using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class InstructionsController : MonoBehaviour {

    [SerializeField] private float widthPadding;
    [SerializeField] private float heightPadding;
    [SerializeField] private float mouseXPadding;

    private RectTransform panelRect;
    private RectTransform textRect;
    private Text instruction;

	void Awake() {
        panelRect = GetComponent<RectTransform>();

        GameObject g = transform.GetChild(0).gameObject;
        textRect = g.GetComponent<RectTransform>();
        instruction = g.GetComponent<Text>();
    }

    public void ShowInstruction(string instruction) {
        this.instruction.text = instruction;
        panelRect.sizeDelta = new Vector2(textRect.sizeDelta.x + widthPadding, this.instruction.preferredHeight + heightPadding);
        ChangeInstructionPosition();
    }

    void ChangeInstructionPosition() {
        int direction = Input.mousePosition.x > Screen.width/2.0f ? -1 : 1;
        transform.position = new Vector3(Input.mousePosition.x + (direction * mouseXPadding), Input.mousePosition.y, Input.mousePosition.z);
    }
}
