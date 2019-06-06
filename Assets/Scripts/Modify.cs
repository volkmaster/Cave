using UnityEngine;
using UnityEngine.UI;

namespace WorldGenerator {
    public class Modify : MonoBehaviour {
        private Block currentBlock;
        public Text blockTypeText;

        private Vector2 rotation;
        private const int RAY_LENGTH = 30;
        private const int MOUSE_SENSITIVITY = 3;
        private const int MOVEMENT_SENSITIVITY = 1;

        // Start is called before the first frame update
        void Start() {
            AssignBlockType(new BlockGrass());
            rotation = new Vector2(
                transform.eulerAngles.y,
                -transform.eulerAngles.x
            );
        }

        // Update is called once per frame
        void Update() {
            // Select block type
            if (Input.GetKeyDown(KeyCode.Alpha1)) {
                AssignBlockType(new BlockGrass());
            } else if (Input.GetKeyDown(KeyCode.Alpha2)) {
                AssignBlockType(new BlockIron());
            } else if (Input.GetKeyDown(KeyCode.Alpha3)) {
                AssignBlockType(new BlockCoal());
            } else if (Input.GetKeyDown(KeyCode.Alpha4)) {
                AssignBlockType(new BlockGold());
            } else if (Input.GetKeyDown(KeyCode.Alpha5)) {
                AssignBlockType(new BlockStone());
            }

            // Add block on left mouse click
            if (Input.GetMouseButtonUp(0)) {
                if (Physics.Raycast(transform.position, transform.forward, out RaycastHit hit, RAY_LENGTH)) {
                    if (Terrain.GetBlock(hit, adjacent: true) is BlockAir) {
                        Terrain.SetBlock(hit, currentBlock, adjacent: true);
                    }
                }
            }

            // Remove block on right mouse click
            if (Input.GetMouseButtonUp(1)) {
                if (Physics.Raycast(transform.position, transform.forward, out RaycastHit hit, RAY_LENGTH)) {
                    Terrain.SetBlock(hit, new BlockAir());
                }
            }

            // Rotate scene on mouse movement
            rotation = new Vector2(
                rotation.x + Input.GetAxis("Mouse X") * MOUSE_SENSITIVITY,
                rotation.y + Input.GetAxis("Mouse Y") * MOUSE_SENSITIVITY
            );
            transform.localRotation = Quaternion.AngleAxis(rotation.x, Vector3.up);
            transform.localRotation *= Quaternion.AngleAxis(rotation.y, Vector3.left);

            // Move scene on key press
            transform.position += transform.forward * MOVEMENT_SENSITIVITY * Input.GetAxis("Vertical");
            transform.position += transform.right * MOVEMENT_SENSITIVITY * Input.GetAxis("Horizontal");
        }

        private void AssignBlockType(Block block) {
            currentBlock = block;
            blockTypeText.text = block.Description;
        }
    }
}