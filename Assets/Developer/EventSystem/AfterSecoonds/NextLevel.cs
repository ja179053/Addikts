namespace Instruction
{
    using UnityEngine;
    using UnityEngine.SceneManagement;
    public class NextLevel : EventAfterTime
    {
        protected override void EventCompleted()
        {
            base.EventCompleted();
            LevelManager.NextLevel();
        }
    }
}
