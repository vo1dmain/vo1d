using UnityEngine;

namespace Controls
{
	internal static class SensitivitySettings
	{
		/// <summary>
		/// Чувствительность поворота по вертикали. Используется для доступа к значениям из других классов.
		/// </summary>
		internal static float SensVert { get => sensitivityVertical; set => sensitivityVertical = SetSensitivity(value); }

		/// <summary>
		/// Чувствительность поворота по горизонтали. Используется для доступа к значениям из других классов.
		/// </summary>
		internal static float SensHor { get => sensitivityHorizontal; set => sensitivityHorizontal = SetSensitivity(value); }

		/// <summary>
		/// Проверяет новое значение чувствительности на соответствие максимуму и минимуму.
		/// </summary>
		/// <param name="value">Новое значение чувствительности.</param>
		/// <returns>
		/// <para>MAX_SENSITIVITY - если <paramref name="value"/> больше MAX_SENSITIVITY.</para>
		/// <para>MIN_SENSITIVITY - если <paramref name="value"/> меньше MIN_SENSITIVITY.</para>
		/// <para><paramref name="value"/> - если оно находится в заданных пределах.</para>
		/// </returns>
		private static float SetSensitivity(float value)
		{
			if (value > MAX_SENSITIVITY)
			{
				return MAX_SENSITIVITY;
			}
			else if (value < MIN_SENSITIVITY)
			{
				return MIN_SENSITIVITY;
			}
			else
			{
				return value;
			}
		}

		/// <summary>
		/// Чувствительность по вертикали. Используется для изменения через редактор Unity.
		/// </summary>
		[SerializeField] private static float sensitivityVertical = 3f;

		/// <summary>
		/// Чувствительность по горизонтали. Используется для изменения через редактор Unity.
		/// </summary>
		[SerializeField] private static float sensitivityHorizontal = 3f;

		/// <summary>
		/// Максимально возможное значение чувствительности.
		/// </summary>
		private const float MAX_SENSITIVITY = 10f;

		/// <summary>
		/// Минимально возможное значение чувствительности.
		/// </summary>
		private const float MIN_SENSITIVITY = 1f;
	}
}
