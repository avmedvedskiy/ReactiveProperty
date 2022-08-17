using UnityEngine;

namespace MVVM
{
    public interface IResolver
    {
        IReactiveProperty Map(Object target, string name);
    }
}