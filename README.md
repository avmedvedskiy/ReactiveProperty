
# ReactiveProperty
Custom realization of Reactive Property to implement MVVM patterns
Using zero reflection in runtime, use prebaked generation code for get properties

Examples
```csharp
public ReactiveProperty<bool> boolProperty;
public ReactiveProperty<int> intProperty { get; } = new();

public readonly ReactiveProperty<string> stringProperty;

public ReactiveProperty<CurrencyEnumNew> enumProperty;
```

View
Default View, will be subscribed when OnEnable
```csharp
public class TestView : View<int>
{
    protected override void UpdateView(int value)
    {
        Debug.Log(value);
    }
}
```
Custom Views with many sync properties, need manual subscribe to properties
```csharp
public class TestEditorView : MonoBehaviour
{
    [Serializable]
    public class SubSerializeClass
    {
        public SyncReactiveProperty<string> stringSyncProp;
        public int someFields;
    }

    [SerializeField] private SyncReactiveProperty<string> syncStringProperty;

    [SerializeField] private SyncReactiveProperty<CurrencyEnumNew> syncCurrencyProperty;
    [SerializeField] private SyncReactiveProperty<float> syncFloatProperty;

    [SerializeField] private SubSerializeClass testSerializationClass;
}
```

In inspector use dropdown list for select property

![image](https://user-images.githubusercontent.com/17832838/185577063-f0d305fd-32aa-44e6-9626-ad7ed83fd994.png)

![image](https://user-images.githubusercontent.com/17832838/185577429-cfc27b16-0d5f-4c53-bc5d-78f40f437234.png)

![image](https://user-images.githubusercontent.com/17832838/185577521-9e5c0a26-5182-405e-ae77-dd8d8bfb14c1.png)

For all classes contained ReactiveProperty will be generated Dictionary map with string key and property value
```csharp
public class Resolver_TestBeh : IResolver
{
    private Dictionary<string, Func<TestBeh, IReactiveProperty>> map = new()
    {
        { "intProperty", o => o.intProperty },
        { "boolProperty", o => o.boolProperty },
        { "stringProperty", o => o.stringProperty },
    };

    public IReactiveProperty Map(UnityEngine.Object target, string name)
    {
        return map[name].Invoke(target as TestBeh);
    }
}

public static class BindersLoader
{
    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
    private static void InitResolvers()
    {
        Binders.AddResolvers(new()
        {
            { typeof(TestBeh), new Resolver_TestBeh() },
            { typeof(TestBeh2), new Resolver_TestBeh2() },
        });
    }
}
```

https://user-images.githubusercontent.com/17832838/185593616-2fe9a97f-a6e2-4ffb-84ab-c753d45cb417.mp4


