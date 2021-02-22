using Cat.Application.BotUpdates.Commands.FakeBotUpdate;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Cat.Infrastructure.Fake.BotApiComponents.OperationalClient
{
    public interface IFakeOperationalClientRandomUtils
    {
        IEnumerable<FakeBotUpdate> GetUpdates();
        TimeSpan GetTimeout();
        bool GetBoolean(int difficultyClass);
    }

    public class FakeOperationalClientRandomUtils : IFakeOperationalClientRandomUtils
    {
        private const string FakeUpdateMessages = @"
                                    Hwæt! Wé Gárdena in géardagum þéodcyninga þrym gefrúnon·$msg_sep$
                                    hú ðá æþelingas ellen fremedon.$msg_sep$
                                    Oft Scyld Scéfing sceaþena þréatum monegum maégþum meodosetla oftéah·$msg_sep$
                                    egsode Eorle syððan aérest wearð féasceaft funden hé þæs frófre gebád·$msg_sep$
                                    wéox under wolcnum·$msg_sep$
                                    weorðmyndum þáh oð þæt him aéghwylc þára ymbsittendra ofer hronráde hýran scolde, gomban gyldan·$msg_sep$
                                    þæt wæs gód cyning.$msg_sep$
                                    Ðaém eafera wæs æfter cenned geong in geardum þone god sende folce tó frófre·$msg_sep$
                                    fyrenðearfe ongeat·$msg_sep$
                                    þæt híe aér drugon aldorléase lange hwíle·$msg_sep$
                                    him þæs líffréä wuldres wealdend woroldáre forgeaf: Béowulf wæs bréme --blaéd wíde sprang-- Scyldes eafera Scedelandum in.$msg_sep$
                                    Swá sceal geong guma góde gewyrcean fromum feohgiftum on fæder bearme þæt hine on ylde eft gewunigen wilgesíþas þonne wíg cume·$msg_sep$
                                    léode gelaésten: lofdaédum sceal in maégþa gehwaére man geþéön.$msg_sep$
                                    Him ðá Scyld gewát tó gescæphwíle felahrór féran on fréan waére·$msg_sep$
                                    hí hyne þá ætbaéron tó brimes faroðe swaése gesíþas swá hé selfa bæd þenden wordum wéold wine Scyldinga léof landfruma lange áhte·$msg_sep$
                                    þaér æt hýðe stód hringedstefna ísig ond útfús æþelinges fær·$msg_sep$
                                    álédon þá léofne þéoden béaga bryttan on bearm scipes maérne be mæste·$msg_sep$
                                    þaér wæs mádma fela of feorwegum frætwa gelaéded·$msg_sep$
                                    ne hýrde ic cýmlícor céol gegyrwan hildewaépnum ond heaðowaédum billum ond byrnum·$msg_sep$
                                    him on bearme læg mádma mænigo þá him mid scoldon on flódes aéht feor gewítan·$msg_sep$
                                    nalæs hí hine laéssan lácum téodan þéodgestréonum þonne þá dydon þe hine æt frumsceafte forð onsendon aénne ofer ýðe umborwesende·$msg_sep$
                                    þá gýt híe him ásetton segen gyldenne héah ofer héafod·$msg_sep$
                                    léton holm beran·$msg_sep$
                                    géafon on gársecg·$msg_sep$
                                    him wæs geómor sefa murnende mód·$msg_sep$
                                    men ne cunnon secgan tó sóðe seleraédenne hæleð under heofenum hwá þaém hlæste onféng.                           
                                ";

        private readonly Random _random;

        public FakeOperationalClientRandomUtils()
        {
            _random = new Random(Guid.NewGuid().GetHashCode());
        }

        public IEnumerable<FakeBotUpdate> GetUpdates()
        {
            var updatesCount = GetUpdatesCount();
            if (updatesCount == 0)
                return Enumerable.Empty<FakeBotUpdate>();

            var updates = new List<FakeBotUpdate>();
            for (var i = 0; i < updatesCount; i++)
            {
                updates.Add(new FakeBotUpdate { Message = GetUpdateMessage() });
            }

            return updates;
        }

        private int GetUpdatesCount()
        {
            var updatesCount = 0;

            var roll = _random.Next(1, 21);
            if (roll >= 14 && roll <= 17) updatesCount = 1;
            else if (roll == 17) updatesCount = 2;
            else if (roll == 19) updatesCount = 3;
            else if (roll == 20) updatesCount = _random.Next(3, 11);

            return updatesCount;
        }

        private string GetUpdateMessage()
        {
            var messages = FakeUpdateMessages.Split("$msg_sep$", StringSplitOptions.RemoveEmptyEntries).Select(_ => _.Trim()).ToArray();
            return messages[_random.Next(0, messages.Length)];
        }

        public TimeSpan GetTimeout()
        {
            var minSeconds = 0;
            var maxSeconds = 0;

            var roll = _random.Next(1, 21);
            if (roll == 1)
            {
                minSeconds = (int)TimeSpan.FromMinutes(45).TotalSeconds;
                maxSeconds = (int)TimeSpan.FromHours(3).TotalSeconds;
            }
            if (roll >= 2 && roll <= 8)
            {
                minSeconds = (int)TimeSpan.FromMinutes(5).TotalSeconds;
                maxSeconds = (int)TimeSpan.FromMinutes(30).TotalSeconds;
            }
            if (roll >= 9 && roll <= 15)
            {
                minSeconds = 45;
                maxSeconds = (int)TimeSpan.FromMinutes(2).TotalSeconds;
            }
            if (roll >= 16 && roll <= 18)
            {
                minSeconds = 10;
                maxSeconds = 15;
            }
            if (roll == 19)
            {
                minSeconds = 0;
                maxSeconds = 5;
            }

            var timeoutSeconds = _random.Next(minSeconds, maxSeconds + 1);
            return TimeSpan.FromSeconds(timeoutSeconds);
        }

        public bool GetBoolean(int difficultyClass)
        {
            if (difficultyClass > 20) difficultyClass = 20;
            if (difficultyClass < 1) difficultyClass = 1;
            var roll = _random.Next(1, 21);
            if (roll == 1) return false;
            return roll >= difficultyClass;
        }
    }
}
