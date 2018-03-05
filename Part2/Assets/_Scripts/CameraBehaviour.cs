using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraBehaviour : MonoBehaviour {


    //Camera will have different modes depending on what is happening (FIXME: for now these states are static but as we all know, statics are evil)
    public static CameraMode cameraMode = CameraMode.FollowPlayer; 


    public BoardManager BM;
    public Transform CameraDefaultPos;

    //Quaternion TiltedDownRotation = new Quaternion(40, 0 , -20 , 0);
    public float FollowPlayerXOffset = -12;
    public float FollowPlayerYOffset = 5;
    Player currentPlayer;

	
	// Update is called once per frame
	void LateUpdate () {
        if (!TurnManager.GameProperlyLoaded)
        {
            return;
        }
        switch (cameraMode)
        {
            case CameraMode.FollowPlayer:
                currentPlayer = BM.getCurrentPlayer();
                if (currentPlayer == null)
                {
                    Debug.LogError("Couldnt get a valid player, for camera to lookat");
                    return;
                }
                this.transform.position = new Vector3(currentPlayer.transform.position.x + FollowPlayerXOffset, currentPlayer.transform.position.y + FollowPlayerYOffset, currentPlayer.transform.position.z );
                this.transform.rotation = CameraDefaultPos.rotation;
                //rotation shouldnt change here/ this.transform.rotation = currentPlayer.transform.Find("CameraParkingLot").rotation;
                // this.transform.LookAt(currentPlayer.transform);
                break;
            case CameraMode.MapViewMode:
                this.transform.position = CameraDefaultPos.position;
                this.transform.rotation = CameraDefaultPos.rotation;
                break;
            case CameraMode.FollowStar:
                break;
            case CameraMode.ExploreMap:
                break;
            default:
                break;
        }
	}
}
