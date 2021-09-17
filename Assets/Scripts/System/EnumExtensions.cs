using ItemSystem.EffectSystem;
using System.Globalization;

public static class EnumExtensions {
	public static string FormatItemEffectType(ItemEffectType itemEffectType) {
		return CultureInfo.CurrentCulture.TextInfo.ToTitleCase(itemEffectType.ToString().ToLower());
	}
}
