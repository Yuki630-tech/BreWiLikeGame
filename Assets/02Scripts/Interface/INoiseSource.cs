using UniRx;
using UnityEngine;

public interface INoiseSource
{
    public IReadOnlyReactiveProperty<bool> IsNoisy { get; }
}
