 [![Releases](https://img.shields.io/github/release/avmedvedskiy/ReactiveProperty.svg)](https://github.com/avmedvedskiy/ReactiveProperty/releases)
 
# ReactiveProperty
- [Introduction](#introduction)
- [Examples](#examples)
	- [View](#view)
	- [Custom Views](#custom-views)
	- [Inspector Drawer](#inspector-drawer)
	- [Video Example](#video-example)
- [Reactive Collection](#reactive-collection)
    - [List Example](#list-example)
- [Views Inspector](#views-inspector)
- [Generated Code](#generated-code)
 
## Introduction 

Custom realization of Reactive Property to implement MVVM patterns
Using zero reflection in runtime, use prebaked generation code for get properties

## Examples
### ViewModel
Monobehaviour ViewModel with Reactive Propertoes. 
- ReactiveProperty will be updated when value are changed 
- ReactiveEvent updated every time, even values are equals
```csharp

public ReactiveEvent dummyEvent;
public ReactiveEvent<bool> boolEvent;

public ReactiveProperty<bool> boolProperty;
public ReactiveProperty<int> intProperty { get; } = new();

public readonly ReactiveProperty<string> stringProperty;

public ReactiveProperty<CurrencyEnumNew> enumProperty;
```

### View
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
### Custom Views 
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
### Inspector Drawer
In inspector use dropdown list for select property

![image](https://user-images.githubusercontent.com/17832838/185577063-f0d305fd-32aa-44e6-9626-ad7ed83fd994.png)

![image](https://user-images.githubusercontent.com/17832838/185577429-cfc27b16-0d5f-4c53-bc5d-78f40f437234.png)

![image](https://user-images.githubusercontent.com/17832838/185577521-9e5c0a26-5182-405e-ae77-dd8d8bfb14c1.png)

### Video Example
https://user-images.githubusercontent.com/17832838/185593616-2fe9a97f-a6e2-4ffb-84ab-c753d45cb417.mp4

## Reactive Collection
For list in view models use ReactiveList<T> this will enable create views for all items in list

```csharp
public ReactiveList<T> _models { get; } = new();
```

For view create a nested class ListView, own Model class and own View(nested ModelView<Model>). ListView and View is a monobehaviour;
```csharp
//example
public class DaysView : ListView<DayView, DayModel>
{
}

[Serializable]
public class DayModel
{
    public int index;
    public bool received;
    public object rewards;
}

public class DayView : ModelView<DayModel>
{
    public ReactiveProperty<int> index;
    public ReactiveProperty<bool> received;
    [NonSerialized] public ReactiveProperty<object> rewards = new();

    public override void SetModel(DayModel model)
    {
        index.Value = model.index;
        received.Value = model.received;
        rewards.Value = model.rewards;
    }
}    
```
You can override all events from reactive list
```csharp
public abstract void OnAdd(T item);
public abstract void OnClear();
public abstract void OnInsert(int index, T item);
public abstract void OnRemoveAt(int index);
public abstract void OnReplace(int index, T item);
public abstract void OnValueChanged(int index, T item);
public abstract void OnInitialize(IReadOnlyList<T> values);
```
        
        
### List Example
Screens from Unity to show how its look in editor

![image](https://user-images.githubusercontent.com/17832838/187874350-5f25509a-8d20-47e9-866b-98d36ab3de18.png)

![image](https://user-images.githubusercontent.com/17832838/187874406-9bf67ab9-4e76-419e-b026-c46a38e763dc.png)

![image](https://user-images.githubusercontent.com/17832838/187874474-e67a6fae-0b1f-4d22-a992-fea1ec261b6a.png)

![image](https://user-images.githubusercontent.com/17832838/187874435-d9a8f28f-06d6-4be1-aa57-79d865086e28.png)

	
## Views Inspector
Custom window to manage all views on GameObject(include childs). Add new View Component, remove exists, change values
	
![image](https://user-images.githubusercontent.com/17832838/188481385-53332080-dfd8-4a89-9fc3-8f21fe705d30.png)

## Generated Code
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


