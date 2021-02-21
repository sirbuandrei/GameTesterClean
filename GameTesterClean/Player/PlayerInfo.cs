using System;

namespace GameTesterClean
{
    [Serializable]
    public class PlayerInfo
    {
        public Vector positionToSend { get; set; }
        public int ID { get; set; }
        public Vector velocityVector { get; set; }
        public string walkingDirection { get; set; }
        public int currentFrame { get; set; }
        public string nickname { get; set; }
        public string characterType { get; set; }

        public PlayerInfo()
        {
            ID = 0;
        }
    }
}
