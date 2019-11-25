using UnityEngine;

namespace Controls
{
	internal class Action
	{
		internal string Name { get; private set; } = string.Empty;
		internal KeyCode Key { get; private set; } = KeyCode.None;

		internal Action(string name, KeyCode keyCode)
		{
			Name = name;
			Key = keyCode;
		}

		internal void SetNewKey(KeyCode keyCode)
		{
			Key = keyCode;
		}
	}
}