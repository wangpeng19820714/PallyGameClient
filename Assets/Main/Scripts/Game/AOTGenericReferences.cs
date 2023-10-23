using System.Collections.Generic;
public class AOTGenericReferences : UnityEngine.MonoBehaviour
{

	// {{ AOT assemblies
	public static readonly IReadOnlyList<string> PatchedAOTAssemblyList = new List<string>
	{
		"Unity.InputSystem.dll",
		"mscorlib.dll",
	};
	// }}

	// {{ constraint implement type
	// }} 

	// {{ AOT generic types
	// System.Action<UnityEngine.InputSystem.InputAction.CallbackContext>
	// System.Action<object>
	// System.Collections.Generic.ArraySortHelper<object>
	// System.Collections.Generic.Comparer<object>
	// System.Collections.Generic.Dictionary.Enumerator<int,GameFramework.UI.UIInfo>
	// System.Collections.Generic.Dictionary.KeyCollection.Enumerator<int,GameFramework.UI.UIInfo>
	// System.Collections.Generic.Dictionary.KeyCollection<int,GameFramework.UI.UIInfo>
	// System.Collections.Generic.Dictionary.ValueCollection.Enumerator<int,GameFramework.UI.UIInfo>
	// System.Collections.Generic.Dictionary.ValueCollection<int,GameFramework.UI.UIInfo>
	// System.Collections.Generic.Dictionary<int,GameFramework.UI.UIInfo>
	// System.Collections.Generic.EqualityComparer<GameFramework.UI.UIInfo>
	// System.Collections.Generic.EqualityComparer<int>
	// System.Collections.Generic.ICollection<System.Collections.Generic.KeyValuePair<int,GameFramework.UI.UIInfo>>
	// System.Collections.Generic.ICollection<object>
	// System.Collections.Generic.IComparer<object>
	// System.Collections.Generic.IEnumerable<System.Collections.Generic.KeyValuePair<int,GameFramework.UI.UIInfo>>
	// System.Collections.Generic.IEnumerable<System.Collections.Generic.KeyValuePair<object,object>>
	// System.Collections.Generic.IEnumerable<object>
	// System.Collections.Generic.IEnumerator<System.Collections.Generic.KeyValuePair<int,GameFramework.UI.UIInfo>>
	// System.Collections.Generic.IEnumerator<object>
	// System.Collections.Generic.IEqualityComparer<int>
	// System.Collections.Generic.IList<object>
	// System.Collections.Generic.KeyValuePair<int,GameFramework.UI.UIInfo>
	// System.Collections.Generic.KeyValuePair<object,object>
	// System.Collections.Generic.List.Enumerator<object>
	// System.Collections.Generic.List<object>
	// System.Collections.Generic.ObjectComparer<object>
	// System.Collections.Generic.ObjectEqualityComparer<GameFramework.UI.UIInfo>
	// System.Collections.Generic.ObjectEqualityComparer<int>
	// System.Collections.ObjectModel.ReadOnlyCollection<object>
	// System.Comparison<object>
	// System.Predicate<UnityEngine.InputSystem.InputControlScheme>
	// System.Predicate<object>
	// System.Runtime.CompilerServices.ConditionalWeakTable.CreateValueCallback<object,object>
	// System.Runtime.CompilerServices.ConditionalWeakTable.Enumerator<object,object>
	// System.Runtime.CompilerServices.ConditionalWeakTable<object,object>
	// UnityEngine.InputSystem.Utilities.ReadOnlyArray.Enumerator<UnityEngine.InputSystem.InputControlScheme>
	// UnityEngine.InputSystem.Utilities.ReadOnlyArray<UnityEngine.InputSystem.InputControlScheme>
	// }}

	public void RefMethods()
	{
	}
}