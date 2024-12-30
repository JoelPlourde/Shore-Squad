using ItemSystem;
using ItemSystem.EffectSystem;
using ItemSystem.EquipmentSystem;
using System.Globalization;

public static class EnumExtensions {
	public static string FormatItemEffectType(ItemEffectType itemEffectType) {
		return CultureInfo.CurrentCulture.TextInfo.ToTitleCase(itemEffectType.ToString().ToLower());
	}

	public static string FormatEnum(string input) {
    	return CultureInfo.CurrentCulture.TextInfo.ToTitleCase(input.ToLower());
    }

	public static string FormatItemType(ItemType itemType) {
        return itemType switch
        {
            ItemType.EQUIPMENT => "Equipment",
            ItemType.CONSUMABLE => "Consumable",
            _ => "Resource",
        };
    }

	public static string FormatEquipmentType(EquipmentData equipmentData) {
		switch (equipmentData.SlotType) {
			case SlotType.HEAD:
				return "headpiece";
			case SlotType.BODY:
				return "chestpiece";
			case SlotType.PANTS:
				return "leggings";
			case SlotType.GLOVES:
				return "gloves";
			case SlotType.BOOTS:
				return "boots";
			case SlotType.WEAPON:
				switch (equipmentData.WeaponType) {
					case WeaponType.SINGLE_HANDED:
						return "single-handed";
					case WeaponType.TWO_HANDED:
						return "two-handed";
					default:
						return "tool";
				}
			case SlotType.SHIELD:
				if (equipmentData.WeaponType != WeaponType.NONE) {
					return "off-hand";
				}
				return "shield";
			case SlotType.RING:
				return "ring";
			case SlotType.NECK:
				return "necklace";
			default:
				return "equipment";
		}
	}
}
