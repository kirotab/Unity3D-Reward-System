using UnityEngine;
using System.Collections;

namespace Assets.Scripts
{
    public interface IChest
    {
        float TimeRemaining { get; set; }
        float TimeInterval { get; set; }
        int CurrentReward { get; set; }
        int MaxReward { get; set; }
        int RewardStep { get; set; }
        bool IsHandicapApplied { get; set; }
        bool IsBonusApplied { get; set; }
        int HandicapFactorPercent { get; set; }
        int BonusFactorPercent { get; set; }
        int CollectReward();
        void RestartInterval();
        void GenerateReward();
        void UpdateTimer();
    }
}