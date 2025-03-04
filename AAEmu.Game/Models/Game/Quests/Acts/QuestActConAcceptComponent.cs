﻿using AAEmu.Game.Models.Game.Char;
using AAEmu.Game.Models.Game.Quests.Templates;

namespace AAEmu.Game.Models.Game.Quests.Acts
{
    public class QuestActConAcceptComponent : QuestActTemplate
    {
        public uint QuestContextId { get; set; }

        public override bool Use(ICharacter character, Quest quest, int objective)
        {
            _log.Warn("QuestActConAcceptComponent: QuestContextId {0}", QuestContextId);
            return false;
        }
    }
}
