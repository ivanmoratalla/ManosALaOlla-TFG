using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public interface ISaveDataService
{
    public Task SaveStarsForLevel(int level, int stars);

    public Task<int> LoadStarsForLevel(int level);

}
