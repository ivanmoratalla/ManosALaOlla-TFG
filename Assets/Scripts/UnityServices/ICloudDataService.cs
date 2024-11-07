using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public interface ICloudDataService
{
    public Task SaveStarsForLevelIfHigher(int level, int stars);

    public Task<int> LoadStarsForLevel(int level);

    public Task UpdateMaxLevelIfNeeded(int maxLevel);

    public Task<int> LoadMaxUnlockedLevel();
}
