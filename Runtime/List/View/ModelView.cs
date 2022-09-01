using UnityEngine;

namespace MVVM.Collections
{
    /// <summary>
    /// Base class for item model view(in lists), use ReactiveProperty in nested classes and set values in SetModel method
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public abstract class ModelView<T> : MonoBehaviour
    {
        /// <summary>
        /// Set or Update model
        /// </summary>
        public abstract void SetModel(T model);
    }
}