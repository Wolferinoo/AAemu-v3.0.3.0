using AAEmu.Game.Models.Game.Char;
using AAEmu.Game.Models.Game.Quests.Templates;

namespace AAEmu.Game.Models.Game.Quests.Acts
{
    public class QuestActSupplyCrimePoint : QuestActTemplate
    {
        public int Point { get; set; }

        public override bool Use(ICharacter character, Quest quest, int objective)
        {
            _log.Warn("QuestActSupplyCrimePoint");
            return false;
        }
    }
}
