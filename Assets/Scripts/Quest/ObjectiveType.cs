namespace QuestSystem {
	// Description have been taken from https://www.gamasutra.com/blogs/JacobLauritsBesenbacherKjeldsen/20170818/303927/The_Quest_for_the_custom_quest_system.php
	public enum ObjectiveType {
		KILL,		// Probably the most basic quest type, the task is to kill something in the game, for example; kill 10 goblins.
		GATHER,		// Again very simple, the task is to gather x things in the game world, collecting berries or the like.
		ESCORT,		// The player must escort or follow a person or object from point A to B while keeping it safe.
		DELIVER,	// The player is the delivery boy, they must deliver an item to a person or point.
		DEFEND,		// The player has to defend a location from oncoming enemies, often for a set number of waves or time.
		PROFIT,		// The player must have a certain amount of resources to complete the quest, contrary to gather quests these resources are resources the player would otherwise be able to use himself.
		ACTIVATE,	// The player's task is to activate / interact with one or more objects in the game world. In some cases, this must be done in a certain order for a puzzle effect.
		SEARCH,     // Search an area, discover an area of the game world. This is useful for introducing areas of the map to the player and giving them a sense of accomplishment right of the bat, showing them a new quest hub or the like.
		TALK,       // The player's task is to talk to X NPCs.
	}
}