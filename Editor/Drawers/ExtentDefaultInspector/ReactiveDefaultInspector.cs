using UnityEngine;
using UnityEditor;

namespace MVVM
{
#if REACTIVE_DEFAULT_INSPECTOR
    [CustomEditor(typeof(MonoBehaviour), true)]
    public class ReactiveDefaultInspector : UnityEditor.Editor
    {
        private ReactiveLinkEditor _reactiveLinkEditor;
        private void OnEnable()
        {
            _reactiveLinkEditor = new ReactiveLinkEditor((MonoBehaviour)target);
        }

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            _reactiveLinkEditor.OnInspectorGUI();
        }
    }
#endif
}