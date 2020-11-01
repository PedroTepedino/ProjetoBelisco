using System;
using UnityEngine;

namespace Belisco
{
    public class RoomTransitionSpawner : MonoBehaviour
    {
        [SerializeField] private RoomParameters _previousRoom = null;
        public RoomParameters PreviousRoom => _previousRoom;

        public static RoomTransitionSpawner Instantiate() => new GameObject("[RoomTransitionSpawner]").AddComponent<RoomTransitionSpawner>();

        public void SetRoomParameter(RoomParameters roomParameters)
        {
            _previousRoom = roomParameters;
        }
        
        private void OnValidate()
        {
            FixName();
        }

        public void FixName()
        {
            var roomName = _previousRoom != null ? PreviousRoom.ThisSceneAsset.name : "**NULL_ROOM**";
            this.name = $"[TRANSITION][{roomName}]->[{this.gameObject.scene.name}]";
        }
    }
}